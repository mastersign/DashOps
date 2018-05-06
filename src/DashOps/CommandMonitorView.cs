using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mastersign.DashOps
{
    partial class CommandMonitorView : IExecutable
    {
        private static App App => (App)Application.Current;

        public string CommandId => IdBuilder.BuildId(Command + " " + Arguments);

        public string CommandLabel => Command
            + (string.IsNullOrWhiteSpace(Arguments)
                ? string.Empty
                : " " + Arguments);

        public override string ToString() => $"[{CommandId}] {Title}: {CommandLabel}";

        public string CurrentLogFile { get; set; }

        public override async Task<bool> Check()
        {
            var result = await App.Executor.ExecuteAsync(this);
            var success = result.StatusCode == 0
                && RequiredPatterns.All(p => p.IsMatch(result.Output))
                && !ForbiddenPatterns.Any(p => p.IsMatch(result.Output));
            return success;
        }

        public IEnumerable<string> LogFiles()
            => LogFileManager.FindLogFiles(this, Logs);

        public string LastLogFile
            => LogFileManager.FindLogFiles(this, Logs)
                .OrderByDescending(f => f)
                .FirstOrDefault();

        public bool Visible => false;

        public LogFileInfo LastLogFileInfo()
        {
            var lastLogFile = LastLogFile;
            return lastLogFile != null
                ? LogFileManager.GetInfo(lastLogFile)
                : null;
        }

        public void NotifyExecutionFinished()
        {
            // nothing
        }
    }
}
