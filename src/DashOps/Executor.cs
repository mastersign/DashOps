using System.Diagnostics;
using System.IO;
using System.Text;

namespace Mastersign.DashOps
{
    public class Executor : IDisposable
    {
        private class Execution(IExecutable executable, Action<ExecutionResult> onExit)
        {
            public readonly IExecutable Executable = executable;
            public readonly Action<ExecutionResult> OnExit = onExit;
        }

        private readonly Dictionary<Process, Execution> runningProcesses = [];

        private readonly InterProcessConnector interProcessConnector = new();

        public void Dispose()
        {
            interProcessConnector.Dispose();
        }

        public Task<ExecutionResult> ExecuteAsync<T>(T executable)
            where T : IExecutable
        {
            var block = new ManualResetEvent(false);
            ExecutionResult result = null;
            void Unblock(ExecutionResult r)
            {
                result = r;
                block.Set();
            }
            Execute(executable, Unblock);
            var t = new Task<ExecutionResult>(() =>
            {
                block.WaitOne();
                return result;
            });
            t.Start();
            return t;
        }

        public void Execute<T>(T executable, Action<ExecutionResult> onExit = null)
            where T : IExecutable
        {
            if (executable.Logs != null && !Directory.Exists(executable.Logs))
            {
                Directory.CreateDirectory(executable.Logs);
            }
            var timestamp = DateTime.Now;
            var logfile = executable.Logs != null
                    ? Path.Combine(
                        executable.Logs,
                        executable.PreliminaryLogFileName(timestamp))
                    : null;

            if (executable.UseWindowsTerminal)
            {
                StartProcessWithWindowsTerminal(executable, timestamp, logfile, onExit);
            }
            else
            {
                StartProcessDirect(executable, timestamp, logfile, onExit);
            }
        }

        private void StartProcessDirect(IExecutable executable, DateTime timestamp, string logfile, Action<ExecutionResult> onExit)
        {
            var cmd = !string.IsNullOrWhiteSpace(executable.PowerShellExe)
                    ? executable.PowerShellExe
                    : executable.UsePowerShellCore
                        ? CommandLine.PowerShellCoreExe
                        : CommandLine.WindowsPowerShellExe;
            var cmdArgs = BuildPowerShellArguments(executable, logfile, timestamp);
            var psi = BuildProcessStartInfo(executable, cmd, cmdArgs);
            Process p;
            try
            {
                p = Process.Start(psi);
            }
            catch (Exception e)
            {
                p = null;
                onExit(new ExecutionResult(executable,
                    startFailed: true,
                    success: false,
                    exitCode: 0,
                    output: e.GetType().Name + Environment.NewLine + e.Message));
            }
            if (p != null)
            {
                WatchProcessForExit(p, executable, logfile, onExit);
            }
        }

        private void StartProcessWithWindowsTerminal(IExecutable executable, DateTime timestamp, string logfile, Action<ExecutionResult> onExit)
        {
            string cmd = CommandLine.WindowsTerminalExe;
            var cmdArgs = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(executable.WindowsTerminalArguments))
            {
                cmdArgs.Append(executable.WindowsTerminalArguments);
                cmdArgs.Append(" ");
            }
            cmdArgs.Append('"');
            cmdArgs.Append(
                !string.IsNullOrWhiteSpace(executable.PowerShellExe)
                    ? executable.PowerShellExe
                    : executable.UsePowerShellCore
                        ? CommandLine.PowerShellCoreExe
                        : CommandLine.WindowsPowerShellExe);
            cmdArgs.Append("\" ");

            var pipeName = interProcessConnector.StartSession((pipeName, data) =>
            {
                interProcessConnector.KillSession(pipeName);
                if (data.Length == 0)
                {
                    onExit(new ExecutionResult(executable,
                        startFailed: true,
                        success: false,
                        exitCode: 0,
                        output: "Timeout while waiting for PowerShell process ID."));
                    return;
                }
                if (data.Length != 4)
                {
                    onExit(new ExecutionResult(executable,
                        startFailed: true,
                        success: false,
                        exitCode: 0,
                        output: "Received unexpected PID data from PowerShell process"));
                    return;
                }
                var pid = BitConverter.ToInt32(data, 0);
                Process p;
                try
                {
                    p = Process.GetProcessById(pid);
                }
                catch (Exception e)
                {
                    onExit(new ExecutionResult(executable,
                        startFailed: true,
                        success: false,
                        exitCode: 0,
                        output: "Failed to retrieve PowerShell process by PID." + Environment.NewLine
                            + Environment.NewLine
                            + e.GetType().Name + Environment.NewLine
                            + e.Message));
                    return;
                }
                WatchProcessForExit(p, executable, logfile, onExit);
            });

            cmdArgs.Append(BuildPowerShellArguments(executable, logfile, timestamp, pipeName));

            var psi = BuildProcessStartInfo(executable, cmd, cmdArgs.ToString());
            try
            {
                Process.Start(psi);
            }
            catch (Exception e)
            {
                interProcessConnector.KillSession(pipeName);
                onExit(new ExecutionResult(executable,
                    startFailed: true,
                    success: false,
                    exitCode: 0,
                    output: "Failed to start PowerShell process in Windows Terminal." + Environment.NewLine
                        + Environment.NewLine
                        + e.GetType().Name + Environment.NewLine + e.Message));
            }
        }

        private static ProcessStartInfo BuildProcessStartInfo(IExecutable executable, string cmd, string cmdArgs)
        {
            var psi = new ProcessStartInfo(cmd, cmdArgs)
            {
                CreateNoWindow = !executable.Visible,
                WorkingDirectory = executable.WorkingDirectory,
                UseShellExecute = false,
            };
            if (executable.Environment is not null)
            {
                foreach (var kvp in executable.Environment)
                {
                    psi.Environment[kvp.Key] = kvp.Value;
                }
            }
            if (executable.ExePaths != null && executable.ExePaths.Length > 0)
            {
                var oldPaths = psi.Environment.TryGetValue("PATH", out var oldPath)
                    ? oldPath.Split(Path.PathSeparator)
                    : [];
                psi.Environment["PATH"] = string.Join(new string(Path.PathSeparator, 1),
                    executable.ExePaths.Concat(oldPaths));
            }
            return psi;
        }

        private void WatchProcessForExit(Process p, IExecutable executable, string logfile, Action<ExecutionResult> onExit)
        {
            executable.CurrentLogFile = logfile;
            p.EnableRaisingEvents = true;
            lock (runningProcesses)
            {
                runningProcesses[p] = new Execution(executable, onExit);
                p.Exited += (sender, ea) => ProcessExitedHandler((Process)sender);
            }
            if (p.HasExited) ProcessExitedHandler(p);
        }

        private void ProcessExitedHandler(Process p)
        {
            Execution execution;
            lock (runningProcesses)
            {
                if (runningProcesses.TryGetValue(p, out execution)) runningProcesses.Remove(p);
            }
            if (execution == null) return;

            var outputBuffer = new StringBuilder();
            string output = null;
            var exitCode = p.ExitCode;
            var success = exitCode == 0;
            var executable = execution.Executable;
            if (executable != null)
            {
                success = executable.ExitCodes.Length == 0 || executable.ExitCodes.Contains(exitCode);
                var rawLogFile = executable.CurrentLogFile;
                if (rawLogFile != null && LogFileManager.WaitForFileAccess(rawLogFile))
                {
                    var logFile = LogFileManager.FinalizeLogFileName(rawLogFile, success, exitCode);
                    try
                    {
                        LogFileManager.PostprocessLogFile(rawLogFile, logFile, outputBuffer);
                    }
                    catch (IOException ex)
                    {
                        Debug.WriteLine($"Failed to post-process temporary log file: {ex.Message}");
                    }
                    output = outputBuffer.ToString();
                    if (success && executable.SuccessCheck != null)
                    {
                        var tmpLogFile = logFile;
                        success = executable.SuccessCheck(output);
                        logFile = LogFileManager.FinalizeLogFileName(rawLogFile, success, exitCode);
                        try
                        {
                            File.Move(tmpLogFile, logFile);
                        }
                        catch (IOException ex)
                        {
                            Debug.WriteLine($"Failed to rename temporary log file: {ex.Message}");
                        }
                    }
                    if (File.Exists(logFile))
                    {
                        executable.CurrentLogFile = logFile;
                    }
                    try
                    {
                        File.Delete(rawLogFile);
                    }
                    catch (IOException ex)
                    {
                        Debug.WriteLine($"Failed to delete temporary log file: {ex.Message}");
                    }
                }
                else
                {
                    executable.CurrentLogFile = null;
                }
            }
            execution.OnExit?.Invoke(new ExecutionResult(
                executable,
                startFailed: false,
                success, exitCode, output));
            executable?.NotifyExecutionFinished();
        }

        private static string BuildPowerShellArguments(IExecutable executable, string logfile, DateTime timestamp, string pidPipeName = null)
        {
            var psLines = new List<string>();
            psLines.Add($"$Host.UI.RawUI.WindowTitle = \"DashOps - {executable.Title}\"");
            if (logfile != null)
            {
                psLines.Add($"$_ = Start-Transcript -Path \"{logfile}\"");
            }
            if (executable.PrintExecutionInfo)
            {
                psLines.Add($"$t0 = New-Object System.DateTime ({timestamp.Ticks})");
                psLines.Add("$tsf = \"yyyy-MM-dd HH:mm:ss\"");
                psLines.Add("echo \"Process ID: $pid\"");
            }
            if (pidPipeName != null)
            {
                if (executable.PrintExecutionInfo)
                {
                    psLines.Add($"echo \"PID Pipe:   {pidPipeName}\"");
                }
                psLines.Add("$pidData = [System.BitConverter]::GetBytes($pid)");
                psLines.Add("try {");
                psLines.Add($"  $pidPipe = [System.IO.Pipes.NamedPipeClientStream]::new('.', '{pidPipeName}', [System.IO.Pipes.PipeDirection]::InOut)");
                psLines.Add("  $pidPipe.Connect(2000)");
                psLines.Add("  $pidPipe.Write($pidData, 0, $pidData.Length)");
                psLines.Add("  $pidPipe.WaitForPipeDrain()");
                psLines.Add("  $pidPipe.Close()");
                psLines.Add("  $pidPipe = $null");
                psLines.Add("} catch {");
                psLines.Add("  Write-Warning \"Failed to report PID to DashOps: $_\"");
                psLines.Add("}");
            }
            if (executable.PrintExecutionInfo)
            {
                psLines.Add($"echo \"Directory:  {executable.WorkingDirectory}\"");
                psLines.Add($"echo \"Command:    {executable.Command}\"");
                if (!string.IsNullOrWhiteSpace(executable.Arguments))
                    psLines.Add($"echo \"Arguments:  {executable.Arguments.Replace("\"", "`\"")}\"");
                psLines.Add($"echo \"Start:      $($t0.toString($tsf))\"");
                psLines.Add("echo \"--------------------------------------------------------------------------------\"");
                psLines.Add("echo \"\"");
            }
            psLines.Add($"& \"{executable.Command}\" {executable.Arguments}");
            psLines.Add("if ($LastExitCode -eq $null) {");
            psLines.Add("  if ($?) { $ec = 0 } else { $ec = 1; echo \"\"; Write-Warning \"Command failed.\" }");
            psLines.Add("  $allowed = @(0)");
            psLines.Add("} else {");
            psLines.Add("  $ec = $LastExitCode");
            psLines.Add("  $allowed = @(" + string.Join(",", executable.ExitCodes) + ")");
            psLines.Add("  if (!($ec -in $allowed)) { echo \"\"; Write-Warning \"Exit Code: $ec\" } else { echo \"\"; echo \"Exit Code: $ec\" }");
            psLines.Add("}");
            if (executable.PrintExecutionInfo)
            {
                psLines.Add("$t = [DateTime]::Now");
                psLines.Add("echo \"\"");
                psLines.Add("echo \"--------------------------------------------------------------------------------\"");
                psLines.Add($"echo \"End:        $($t::Now.toString($tsf))\"");
                psLines.Add($"echo \"Duration:   $(($t - $t0).ToString())\"");
            }
            if (logfile != null)
            {
                psLines.Add("$_ = Stop-Transcript");
            }
            if (executable.Visible)
            {
                if (executable.KeepOpen)
                {
                    psLines.Add("echo \"Press any key to continue...\"");
                    psLines.Add("$_ = $Host.UI.RawUI.ReadKey()");
                }
                else if (!executable.AlwaysClose)
                {
                    psLines.Add("if (!($ec -in $allowed)) {");
                    psLines.Add("  echo \"Press any key to continue...\"");
                    psLines.Add("  $_ = $Host.UI.RawUI.ReadKey()");
                    psLines.Add("}");
                }
            }
            psLines.Add("exit $ec");
            var cmd = string.Join(Environment.NewLine, psLines);
            var encodedCmd = EncodePowerShellCommand(cmd);

            var psArgsBuilder = new StringBuilder();
            psArgsBuilder.Append("-NoLogo");
            if (!executable.UsePowerShellProfile) psArgsBuilder.Append(" -NoProfile");
            psArgsBuilder.Append(" -ExecutionPolicy ");
            psArgsBuilder.Append(executable.PowerShellExecutionPolicy);
            psArgsBuilder.Append(" -EncodedCommand ");
            psArgsBuilder.Append(encodedCmd);
            return psArgsBuilder.ToString();
        }

        private static string EncodePowerShellCommand(string cmd)
                    => Convert.ToBase64String(Encoding.Unicode.GetBytes(cmd));
    }

    public class ExecutionResult(IExecutable executable, bool startFailed, bool success, int exitCode, string output)
    {
        public IExecutable Executable = executable;
        public readonly bool StartFailed = startFailed;
        public readonly bool Success = success;
        public readonly int ExitCode = exitCode;
        public readonly string Output = output;
    }
}
