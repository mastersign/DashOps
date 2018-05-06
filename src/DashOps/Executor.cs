using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mastersign.DashOps
{
    public class Executor
    {
        private class Execution
        {
            public readonly Process Process;
            public readonly IExecutable Executable;
            public readonly Action<ExecutionResult> OnExit;

            public Execution(Process process, IExecutable executable, Action<ExecutionResult> onExit)
            {
                Process = process;
                Executable = executable;
                OnExit = onExit;
            }
        }

        public class ExecutionResult
        {
            public readonly int StatusCode;
            public readonly string Output;
            public IExecutable Executable;

            public ExecutionResult(IExecutable executable, int statusCode, string output)
            {
                Executable = executable;
                StatusCode = statusCode;
                Output = output;
            }
        }

        private readonly Dictionary<Process, Execution> runningProcesses
            = new Dictionary<Process, Execution>();

        public Task<ExecutionResult> ExecuteAsync<T>(T executable)
            where T : IExecutable
        {
            var block = new ManualResetEvent(false);
            ExecutionResult result = null;
            void Unblock(ExecutionResult r)
            {
                result = r;
                block.Set();
            };
            Execute(executable, onExit: Unblock);
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
                        LogFileManager.PreliminaryLogFileName(executable, timestamp))
                    : null;

            var psArgs = BuildPowerShellArguments(logfile, timestamp,
                executable.WorkingDirectory, executable.Command, executable.Arguments,
                waitForKeyPress: executable.Visible);

            var psi = new ProcessStartInfo(CommandLine.PowerShellExe, psArgs)
            {
                WindowStyle = ProcessWindowStyle.Normal,
                WorkingDirectory = executable.WorkingDirectory,
                UseShellExecute = false,
            };
            if (!executable.Visible)
            {
                psi.CreateNoWindow = true;
            }
            var p = Process.Start(psi);
            if (p != null)
            {
                executable.CurrentLogFile = logfile;
                p.EnableRaisingEvents = true;
                lock (runningProcesses)
                {
                    runningProcesses[p] = new Execution(p, executable, onExit);
                    p.Exited += ProcessExitedHandler;
                }

                if (p.HasExited) ProcessExitedHandler(p, EventArgs.Empty);
            }
        }

        private void ProcessExitedHandler(object sender, EventArgs e)
        {
            var p = (Process)sender;
            Execution execution = null;
            lock (runningProcesses)
            {
                if (runningProcesses.TryGetValue(p, out execution)) runningProcesses.Remove(p);
            }
            if (execution != null)
            {
                var outputBuffer = new StringBuilder();
                var executable = execution.Executable;
                if (executable != null)
                {
                    var logFile = executable.CurrentLogFile;
                    if (logFile != null)
                    {
                        logFile = LogFileManager.FinalizeLogFileName(logFile, p.ExitCode);
                        if (File.Exists(logFile))
                        {
                            LogFileManager.PostprocessLogFile(logFile, outputBuffer);
                        }
                        else
                        {
                            executable.CurrentLogFile = null;
                        }
                    }
                    executable.NotifyExecutionFinished();
                }
                execution.OnExit?.Invoke(new ExecutionResult(executable, p.ExitCode, outputBuffer.ToString()));
            }
        }

        private string BuildPowerShellArguments(string logfile, DateTime timestamp,
            string workingDirectory, string command, string arguments, bool waitForKeyPress)
        {
            var psLines = new List<string>();
            if (logfile != null)
            {
                psLines.Add($"$_ = Start-Transcript -Path \"{logfile}\"");
            }
            psLines.Add($"$t0 = New-Object System.DateTime ({timestamp.Ticks})");
            psLines.Add("$tsf = \"yyyy-MM-dd HH:mm:ss\"");
            psLines.Add($"echo \"Directory:  {workingDirectory}\"");
            psLines.Add($"echo \"Command:    {command}\"");
            if (!string.IsNullOrWhiteSpace(arguments))
                psLines.Add($"echo \"Arguments:  {arguments.Replace("\"", "`\"")}\"");
            psLines.Add($"echo \"Start:      $($t0.toString($tsf))\"");
            psLines.Add("echo \"--------------------------------------------------------------------------------\"");
            psLines.Add("echo \"\"");
            psLines.Add($"& \"{command}\" {arguments}");
            psLines.Add("if ($LastExitCode -eq $null) { if ($?) { $ec = 0 } else { $ec = 1; echo \"\"; Write-Warning \"Command failed.\" } } else { $ec = $LastExitCode; if ($ec -ne 0) { echo \"\"; Write-Warning \"Exit Code: $ec\" } }");
            psLines.Add("$t = [DateTime]::Now");
            psLines.Add("echo \"\"");
            psLines.Add("echo \"--------------------------------------------------------------------------------\"");
            psLines.Add($"echo \"End:        $($t::Now.toString($tsf))\"");
            psLines.Add($"echo \"Duration:   $(($t - $t0).TotalSeconds) sec\"");
            if (logfile != null)
            {
                psLines.Add("$_ = Stop-Transcript");
                psLines.Add($"mv \"{logfile}\" \"{logfile}_$ec.log\"");
            }
            if (waitForKeyPress)
            {
                psLines.Add("echo \"Press any key to continue...\"");
                psLines.Add("$_ = $Host.UI.RawUI.ReadKey()");
            }
            psLines.Add("exit $ec");
            var cmd = string.Join(Environment.NewLine, psLines);
            var encodedCmd = EncodePowerShellCommand(cmd);
            return $"-NoLogo -ExecutionPolicy RemoteSigned -EncodedCommand {encodedCmd}";
        }

        private static string EncodePowerShellCommand(string cmd)
                    => Convert.ToBase64String(Encoding.Unicode.GetBytes(cmd));
    }
}
