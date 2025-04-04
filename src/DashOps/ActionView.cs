using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Mastersign.DashOps
{
    partial class ActionView : IExecutable, ILogged
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

        private string _commandId;

        public string CommandId 
            => _commandId ?? (_commandId = IdBuilder.BuildId(Command + " " + Arguments));

        public override string ToString() => $"[{CommandId}] {Title}: {CommandLabel}";

        public ControlTemplate LogIcon
        {
            get
            {
                var logInfo = this.GetLastLogFileInfo();
                var resourceName =
                    logInfo != null
                        ? logInfo.HasResult
                            ? logInfo.Success ? "IconLogOK" : "IconLogError"
                            : "IconLog"
                        : "IconLogEmpty";
                return Application.Current.FindResource(resourceName) as ControlTemplate;
            }
        }

        public void NotifyExecutionFinished()
        {
            OnPropertyChanged(nameof(LogIcon));
            LogIconChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler LogIconChanged;

        public Task ExecuteAsync() => App.Instance.Executor.ExecuteAsync(this);

        public Func<string, bool> SuccessCheck => null;

        public string ExitCodesFormatted => string.Join(", ", ExitCodes);
    }
}
