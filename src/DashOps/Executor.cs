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
        private Dictionary<Process, ActionView> runningProcesses = new Dictionary<Process, ActionView>();

        public bool IsValid(ActionView action) => true;

        public void Execute(ActionView action)
        {
            if (action.Logs != null && !Directory.Exists(action.Logs))
            {
                Directory.CreateDirectory(action.Logs);
            }
            bool CanLog() => action.Logs != null;
            string logFile = Path.Combine(action.Logs, action.CreatePreliminaryLogName());
            if (CanLog()) action.CurrentLogFile = logFile;

            var psLines = new List<string>();
            if (CanLog())
            {
                psLines.Add($"$_ = Start-Transcript -Path \"{logFile}\"");
            }
            psLines.Add("$t0 = [DateTime]::Now");
            psLines.Add("$tsf = \"yyyy-MM-dd HH:mm:ss\"");
            psLines.Add($"echo \"Command:   {action.ExpandedCommand}\"");
            if (!string.IsNullOrWhiteSpace(action.ExpandedArguments))
                psLines.Add($"echo \"Arguments: {action.ExpandedArguments.Replace("\"", "`\"")}\"");
            psLines.Add($"echo \"Start: $($t0.toString($tsf))\"");
            psLines.Add("echo \"\"");
            psLines.Add($"& \"{action.ExpandedCommand}\" {action.ExpandedArguments}");
            psLines.Add("if ($LastExitCode -eq $null) { if ($?) { $ec = 0 } else { $ec = 1; echo \"\"; Write-Warning \"Command failed.\" } } else { $ec = $LastExitCode; if ($ec -ne 0) { echo \"\"; Write-Warning \"Exit Code: $ec\" } }");
            psLines.Add("$t = [DateTime]::Now");
            psLines.Add("echo \"\"");
            psLines.Add($"echo \"End: $($t::Now.toString($tsf))\"");
            psLines.Add($"echo \"Duration: $(($t - $t0).TotalSeconds) sec\"");
            if (CanLog())
            {
                psLines.Add("$_ = Stop-Transcript");
                psLines.Add($"mv \"{logFile}\" \"{logFile}_$ec.log\"");
            }
            psLines.Add("echo \"Press any key to continue...\"");
            psLines.Add("$_ = $Host.UI.RawUI.ReadKey()");
            psLines.Add("exit $ec");
            var cmd = string.Join(Environment.NewLine, psLines);
            var encodedCmd = EncodePowerShellCommand(cmd);
            var psArgs = $"-NoLogo -ExecutionPolicy RemoteSigned -EncodedCommand {encodedCmd}";
            var psi = new ProcessStartInfo(CommandLine.PowerShellExe, psArgs)
            {
                WindowStyle = ProcessWindowStyle.Normal,
            };
            var p = Process.Start(psi);
            p.EnableRaisingEvents = true;
            lock (runningProcesses)
            {
                runningProcesses[p] = action;
                p.Exited += ProcessExitedHandler;
            }
            if (p.HasExited) ProcessExitedHandler(p, EventArgs.Empty);
        }

        private void ProcessExitedHandler(object sender, EventArgs e)
        {
            var p = (Process)sender;
            ActionView action = null;
            lock (runningProcesses)
            {
                if (runningProcesses.TryGetValue(p, out action)) runningProcesses.Remove(p);
            }
            if (action != null && action.CurrentLogFile != null)
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

        private string EncodePowerShellCommand(string cmd)
            => Convert.ToBase64String(Encoding.Unicode.GetBytes(cmd));
    }
}
