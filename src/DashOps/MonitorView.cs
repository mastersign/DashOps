using System.Diagnostics.Tracing;
using Mastersign.DashOps.Model_v2;
using static Mastersign.DashOps.Model_v2.Helper;

namespace Mastersign.DashOps
{
    partial class MonitorView : ILogged
    {
        public virtual Task<bool> Check(DateTime startTime)
        {
            throw new NotImplementedException();
        }

        protected virtual void NotifyMonitorBegin(DateTime startTime)
        {
            LastExecutionTime = startTime;
            IsRunning = true;
            OnStatusChanged();
        }

        protected virtual void NotifyMonitorFinished(bool success)
        {
            HasExecutionResultChanged = !HasLastExecutionResult || LastExecutionResult != success;
            LastExecutionResult = success;
            HasLastExecutionResult = true;
            IsRunning = false;
            OnStatusChanged();
        }

        public virtual string Status
        {
            get
            {
                return IsRunning
                    ? "running"
                    : HasLastExecutionResult
                        ? LastExecutionResult
                            ? HasExecutionResultChanged
                                ? "recent_success"
                                : "success"
                            : HasExecutionResultChanged
                                ? "recent_error"
                                : "error"
                        : "unknown";
            }
        }

        public event EventHandler StatusChanged;

        protected void OnStatusChanged()
        {
            App.Current.Dispatcher.Invoke(() => {
                OnPropertyChanged(nameof(Status));
                StatusChanged?.Invoke(this, EventArgs.Empty);
                DashOpsCommands.ShowLastLog.RaiseCanExecuteChanged();
                DashOpsCommands.ShowLogHistoryContextMenu.RaiseCanExecuteChanged();
            });
        }

        public int RequiredPatternCount => RequiredPatterns?.Length ?? 0;

        public int ForbiddenPatternCount => ForbiddenPatterns?.Length ?? 0;

        #region ILogged

        public virtual string CommandId => throw new NotImplementedException();

        #endregion

        public void UpdateStatusFromLogFile()
        {
            var logInfo = this.GetLastLogFileInfo();
            if (logInfo != null && logInfo.HasResult)
            {
                LastExecutionResult = logInfo.Success;
                HasLastExecutionResult = true;
            }
        }

        public void UpdateWith(
            MonitorBase settings, 
            IReadOnlyList<AutoMonitorSettings> autoSettings, 
            DefaultMonitorSettings defaults, 
            IReadOnlyDictionary<string, string> variables)
        {
            Title = ExpandTemplate(settings.Title, variables);

            Interval = TimeSpan.FromSeconds(
                Coalesce([settings.Interval, .. autoSettings.Select(s => s.Interval), defaults.Interval]));
            Deactivated = Coalesce([settings.Deactivated, ..autoSettings.Select(s => s.Deactivated), defaults.Deactivated]);

            NoLogs = Coalesce([settings.NoLogs, .. autoSettings.Select(s => s.NoLogs), defaults.NoLogs]);
            Logs = NoLogs ? null : BuildAbsolutePath(ExpandEnv(ExpandTemplate(
                Coalesce([settings.Logs, .. autoSettings.Select(s => s.Logs), defaults.Logs]),
                variables)));
            NoExecutionInfo = Coalesce([settings.NoExecutionInfo, .. autoSettings.Select(s => s.NoExecutionInfo), defaults.NoExecutionInfo]);

            RequiredPatterns = BuildTextPatterns(
                Coalesce([settings.RequiredPatterns, .. autoSettings.Select(s => s.RequiredPatterns), defaults.RequiredPatterns]));
            ForbiddenPatterns = BuildTextPatterns(
                Coalesce([settings.ForbiddenPatterns, .. autoSettings.Select(s => s.ForbiddenPatterns), defaults.ForbiddenPatterns]));
        }
    }
}
