namespace Mastersign.DashOps.ViewModel;

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
        Application.Current.Dispatcher.Invoke(() => {
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
}
