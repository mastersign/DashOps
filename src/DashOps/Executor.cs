using System.Diagnostics;
using System.IO;
using System.Text;

namespace Mastersign.DashOps
{
    public class Executor
    {
        private class Execution(IExecutable executable, Action<ExecutionResult> onExit)
        {
            public readonly IExecutable Executable = executable;
            public readonly Action<ExecutionResult> OnExit = onExit;
        }

        private readonly Dictionary<Process, Execution> runningProcesses = [];

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

            var psArgs = BuildPowerShellArguments(executable, logfile, timestamp);

            var psi = new ProcessStartInfo(CommandLine.PowerShellExe, psArgs)
            {
                WindowStyle = executable.Visible ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden,
                WorkingDirectory = executable.WorkingDirectory,
                UseShellExecute = true,
            };
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
                executable.CurrentLogFile = logfile;
                p.EnableRaisingEvents = true;
                lock (runningProcesses)
                {
                    runningProcesses[p] = new Execution(executable, onExit);
                    p.Exited += ProcessExitedHandler;
                }
                if (p.HasExited) ProcessExitedHandler(p, EventArgs.Empty);
            }
        }

        private void ProcessExitedHandler(object sender, EventArgs e)
        {
            var p = (Process)sender;
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
                success = executable.ExitCodes.Contains(exitCode);
                var rawLogFile = executable.CurrentLogFile;
                if (rawLogFile != null && LogFileManager.WaitForFileAccess(rawLogFile))
                {
                    var logFile = LogFileManager.FinalizeLogFileName(rawLogFile, success, exitCode);
                    LogFileManager.PostprocessLogFile(rawLogFile, logFile, outputBuffer);
                    output = outputBuffer.ToString();
                    if (success && executable.SuccessCheck != null)
                    {
                        var tmpLogFile = logFile;
                        success = executable.SuccessCheck(output);
                        logFile = LogFileManager.FinalizeLogFileName(rawLogFile, success, exitCode);
                        File.Move(tmpLogFile, logFile);
                    }
                    executable.CurrentLogFile = logFile;
                    File.Delete(rawLogFile);
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

        private static string BuildPowerShellArguments(IExecutable executable, string logfile, DateTime timestamp)
        {
            var psLines = new List<string>();
            psLines.Add($"$Host.UI.RawUI.WindowTitle = \"DashOps - {executable.Title}\"");
            if (logfile != null)
            {
                psLines.Add($"$_ = Start-Transcript -Path \"{logfile}\"");
            }
            psLines.Add($"$t0 = New-Object System.DateTime ({timestamp.Ticks})");
            psLines.Add("$tsf = \"yyyy-MM-dd HH:mm:ss\"");
            psLines.Add($"echo \"Directory:  {executable.WorkingDirectory}\"");
            psLines.Add($"echo \"Command:    {executable.Command}\"");
            if (!string.IsNullOrWhiteSpace(executable.Arguments))
                psLines.Add($"echo \"Arguments:  {executable.Arguments.Replace("\"", "`\"")}\"");
            psLines.Add($"echo \"Start:      $($t0.toString($tsf))\"");
            psLines.Add("echo \"--------------------------------------------------------------------------------\"");
            psLines.Add("echo \"\"");
            psLines.Add($"& \"{executable.Command}\" {executable.Arguments}");
            psLines.Add("if ($LastExitCode -eq $null) {");
            psLines.Add("  if ($?) { $ec = 0 } else { $ec = 1; echo \"\"; Write-Warning \"Command failed.\" }");
            psLines.Add("  $allowed = @(0)");
            psLines.Add("} else {");
            psLines.Add("  $ec = $LastExitCode");
            psLines.Add("  $allowed = @(" + string.Join(",", executable.ExitCodes) + ")");
            psLines.Add("  if (!($ec -in $allowed)) { echo \"\"; Write-Warning \"Exit Code: $ec\" } else { echo \"\"; echo \"Exit Code: $ec\" }");
            psLines.Add("}");
            psLines.Add("$t = [DateTime]::Now");
            psLines.Add("echo \"\"");
            psLines.Add("echo \"--------------------------------------------------------------------------------\"");
            psLines.Add($"echo \"End:        $($t::Now.toString($tsf))\"");
            psLines.Add($"echo \"Duration:   $(($t - $t0).ToString())\"");
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
            return $"-NoLogo -ExecutionPolicy RemoteSigned -EncodedCommand {encodedCmd}";
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
