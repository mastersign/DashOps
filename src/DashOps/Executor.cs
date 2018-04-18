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
        public bool IsValid(ActionView action)
        {
            return true;
        }

        public void Execute(ActionView action)
        {
            var args = CommandLine.FormatArgumentList(action.Arguments);
            var expandedArgs = Environment.ExpandEnvironmentVariables(args);
            var cmd = $"& \"{action.Command}\" {expandedArgs}; echo \"Press any key to continue...\"; $_ = $Host.UI.RawUI.ReadKey();";
            var encodedCmd = EncodePowerShellCommand(cmd);
            var psArgs = $"-NoLogo -ExecutionPolicy RemoteSigned -EncodedCommand {encodedCmd}";
            var psi = new ProcessStartInfo("powershell", psArgs)
            {
                WindowStyle = ProcessWindowStyle.Normal
            };
            Process.Start(psi);
        }

        private string EncodePowerShellCommand(string cmd)
            => Convert.ToBase64String(Encoding.Unicode.GetBytes(cmd));
    }
}
