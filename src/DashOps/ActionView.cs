using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Mastersign.DashOps
{
    partial class ActionView : IExecutable
    {
        public bool HasFacette(string name)
            => Facettes?.ContainsKey(name) ?? false;

        public string[] GetFacettes()
            => Facettes?.Keys.ToArray() ?? Array.Empty<string>();

        public bool HasFacetteValue(string name, string value)
            => string.Equals(GetFacetteValue(name), value);

        public string GetFacetteValue(string name)
            => Facettes != null
                ? Facettes.TryGetValue(name, out var value) ? value : null
                : null;

        public string CommandLabel => Command
            + (string.IsNullOrWhiteSpace(Arguments)
                ? string.Empty
                : " " + Arguments);

        public string CommandId => IdBuilder.BuildId(Command + " " + Arguments);

        public override string ToString() => $"[{CommandId}] {Description}: {CommandLabel}";

        public IEnumerable<string> LogFiles()
            => LogFileManager.FindLogFiles(this, Logs);

        public string LastLogFile
            => LogFileManager.FindLogFiles(this, Logs)
                .OrderByDescending(f => f)
                .FirstOrDefault();

        public LogFileInfo LastLogFileInfo()
        {
            var lastLogFile = LastLogFile;
            return lastLogFile != null
                ? LogFileManager.GetInfo(lastLogFile)
                : null;
        }

        public ControlTemplate LogIcon
        {
            get
            {
                var logInfo = LastLogFileInfo();
                var resourceName =
                    logInfo != null
                        ? logInfo.HasExitCode
                            ? logInfo.IsSuccess ? "IconLogOK" : "IconLogError"
                            : "IconLog"
                        : "IconLogEmpty";
                return resourceName != null
                    ? App.Instance.FindResource(resourceName) as ControlTemplate
                    : null;
            }
        }

        public void NotifyExecutionFinished()
        {
            OnPropertyChanged(nameof(LogIcon));
            LogIconChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler LogIconChanged;

        public Task ExecuteAsync() => App.Instance.Executor.ExecuteAsync(this);
    }
}
