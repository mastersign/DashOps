using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastersign.DashOps
{
    public class Executor
    {
        private readonly Dictionary<Process, ActionView> runningProcesses = new Dictionary<Process, ActionView>();

        public bool IsValid(ActionView action) => true;

        public void ExecuteAction(ActionView action)
        {
            if (action.Logs != null && !Directory.Exists(action.Logs))
            {
                Directory.CreateDirectory(action.Logs);
            }
            var timestamp = DateTime.Now;
            var logfile = action.Logs != null
                    ? Path.Combine(action.Logs, action.CreatePreliminaryLogName(timestamp))
                    : null;

            var psArgs = BuildPowerShellArguments(logfile, timestamp,
                action.ExpandedWorkingDirectory, action.ExpandedCommand, action.ExpandedArguments,
                waitForKeyPress: true);

            var psi = new ProcessStartInfo(CommandLine.PowerShellExe, psArgs)
            {
                WindowStyle = ProcessWindowStyle.Normal,
                WorkingDirectory = action.ExpandedWorkingDirectory,
            };
            var p = Process.Start(psi);
            if (p != null)
            {
                p.EnableRaisingEvents = true;
                lock (runningProcesses)
                {
                    runningProcesses[p] = action;
                    p.Exited += ActionProcessExitedHandler;
                }

                if (p.HasExited) ActionProcessExitedHandler(p, EventArgs.Empty);
            }
        }

        private void ActionProcessExitedHandler(object sender, EventArgs e)
        {
            var p = (Process)sender;
            ActionView action = null;
            lock (runningProcesses)
            {
                if (runningProcesses.TryGetValue(p, out action)) runningProcesses.Remove(p);
            }
            if (action?.CurrentLogFile != null)
            {
                action.CurrentLogFile = action.FinalizeLogName(action.CurrentLogFile, p.ExitCode);
                if (File.Exists(action.LastLogFile))
                {
                    LogFileManager.PostprocessLogFile(action.CurrentLogFile);
                }
                else
                {
                    action.CurrentLogFile = null;
                }
                action.NotifyLogChange();
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
