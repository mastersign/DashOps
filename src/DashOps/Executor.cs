using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastersign.DashOps
{
    public class Executor
    {
        public bool IsValid(ActionView action) => true;

        public void Execute(ActionView action, string logPath = null)
        {
            var args = CommandLine.FormatArgumentList(action.Arguments);
            var expandedArgs = Environment.ExpandEnvironmentVariables(args);
            var cmdLines = new List<string>();
            if (logPath != null) cmdLines.Add($"$_ = Start-Transcript -Path \"{logPath}\"");
            cmdLines.Add($"echo \"Command: {action.Command}\"");
            cmdLines.Add($"echo \"Arguments: {expandedArgs}\"");
            cmdLines.Add($"& \"{action.Command}\" {expandedArgs}");
            cmdLines.Add("$success = $?");
            cmdLines.Add("$exitCode = $LastExitCode");
            if (logPath != null) cmdLines.Add("$_ = Stop-Transcript");
            cmdLines.Add("echo \"\"");
            cmdLines.Add("if (!$success) { if ($LastExitCode -ne $null) { Write-Warning \"Exit Code: $exitCode\" } else { Write-Warning \"Command failed.\" } }");
            cmdLines.Add("echo \"Press any key to continue...\"");
            cmdLines.Add("$_ = $Host.UI.RawUI.ReadKey()");
            var cmd = string.Join("; ", cmdLines);
            var encodedCmd = EncodePowerShellCommand(cmd);
            var psArgs = $"-NoLogo -ExecutionPolicy RemoteSigned -EncodedCommand {encodedCmd}";
            var psi = new ProcessStartInfo(CommandLine.PowerShellExe, psArgs)
            {
                WindowStyle = ProcessWindowStyle.Normal
            };
            Process.Start(psi);
        }

        private string EncodePowerShellCommand(string cmd)
            => Convert.ToBase64String(Encoding.Unicode.GetBytes(cmd));
    }
}
