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
            public readonly IExecutable Executable;
            public readonly Action<ExecutionResult> OnExit;

            public Execution(IExecutable executable, Action<ExecutionResult> onExit)
            {
                Executable = executable;
                OnExit = onExit;
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
            var executable = execution.Executable;
            if (executable != null)
            {
                var rawLogFile = executable.CurrentLogFile;
                if (rawLogFile != null && File.Exists(rawLogFile))
                {
                    var logFile = LogFileManager.FinalizeLogFileName(rawLogFile, p.ExitCode);
                    LogFileManager.PostprocessLogFile(rawLogFile, logFile, outputBuffer);
                    executable.CurrentLogFile = logFile;
                    File.Delete(rawLogFile);
                }
                else
                {
                    executable.CurrentLogFile = null;
                }
            }
            execution.OnExit?.Invoke(new ExecutionResult(executable, p.ExitCode, outputBuffer.ToString()));
            executable?.NotifyExecutionFinished();
        }

        private static string BuildPowerShellArguments(IExecutable executable, string logfile, DateTime timestamp)
        {
            var psLines = new List<string>();
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
            psLines.Add("if ($LastExitCode -eq $null) { if ($?) { $ec = 0 } else { $ec = 1; echo \"\"; Write-Warning \"Command failed.\" } } else { $ec = $LastExitCode; if ($ec -ne 0) { echo \"\"; Write-Warning \"Exit Code: $ec\" } }");
            psLines.Add("$t = [DateTime]::Now");
            psLines.Add("echo \"\"");
            psLines.Add("echo \"--------------------------------------------------------------------------------\"");
            psLines.Add($"echo \"End:        $($t::Now.toString($tsf))\"");
            psLines.Add($"echo \"Duration:   $(($t - $t0).TotalSeconds) sec\"");
            if (logfile != null)
            {
                psLines.Add("$_ = Stop-Transcript");
            }
            if (executable.Visible)
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
}
