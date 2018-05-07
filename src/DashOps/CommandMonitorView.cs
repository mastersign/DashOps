using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Mastersign.DashOps
{
    partial class CommandMonitorView : IExecutable
    {
        public bool Visible => false;

        public override string CommandId => IdBuilder.BuildId(Command + " " + Arguments);

        public string CommandLabel => Command
            + (string.IsNullOrWhiteSpace(Arguments)
                ? string.Empty
                : " " + Arguments);

        public override string ToString() => $"[{CommandId}] {Title}: {CommandLabel}";

        public override async Task<bool> Check(DateTime startTime)
        {
            NotifyExecutionBegin(startTime);
            var result = await App.Instance.Executor.ExecuteAsync(this);
            var success = result.StatusCode == 0
                && RequiredPatterns.All(p => p.IsMatch(result.Output))
                && !ForbiddenPatterns.Any(p => p.IsMatch(result.Output));
            NotifyExecutionFinished(success);
            OnLogIconChanged();
            return success;
        }

        protected override void NotifyExecutionFinished(bool success)
        {
            base.NotifyExecutionFinished(success);
            if (!HasExecutionResultChanged)
            {
                System.IO.File.Delete(CurrentLogFile);
                CurrentLogFile = null;
            }
        }

        public void NotifyExecutionFinished()
        {
            // nothing
        }
    }
}
