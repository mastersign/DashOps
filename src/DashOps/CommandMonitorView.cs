using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastersign.DashOps
{
    partial class CommandMonitorView : IExecutable
    {
        public bool Visible => false;

        public bool KeepOpen => false;

        public bool AlwaysClose => false;

        private string _commandId;

        public override string CommandId
            => _commandId ?? (_commandId = IdBuilder.BuildId(Command + " " + Arguments));

        public string CommandLabel => Command
            + (string.IsNullOrWhiteSpace(Arguments)
                ? string.Empty
                : " " + Arguments);

        public override string ToString() => $"[{CommandId}] {Title}: {CommandLabel}";

        public override async Task<bool> Check(DateTime startTime)
        {
            NotifyExecutionBegin(startTime);
            var result = await App.Instance.Executor.ExecuteAsync(this);
            var success = result.StatusCode == 0;
            NotifyExecutionFinished(success);
            OnLogIconChanged();
            return success;
        }

        public Func<string, bool> SuccessCheck
            => output => RequiredPatterns.All(p => p.IsMatch(output)) &&
                         !ForbiddenPatterns.Any(p => p.IsMatch(output));

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
