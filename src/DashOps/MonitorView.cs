using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Mastersign.DashOps
{
    partial class MonitorView : ILogged
    {
        public virtual Task<bool> Check(DateTime startTime)
        {
            throw new NotImplementedException();
        }

        protected virtual void NotifyExecutionBegin(DateTime startTime)
        {
            LastExecutionTime = startTime;
            IsRunning = true;
            OnStatusChanged();
        }

        protected virtual void NotifyExecutionFinished(bool success)
        {
            HasExecutionResultChanged = !HasLastExecutionResult || LastExecutionResult != success;
            LastExecutionResult = success;
            HasLastExecutionResult = true;
            IsRunning = false;
            OnStatusChanged();
            OnLogIconChanged();
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
            OnPropertyChanged(nameof(Status));
            StatusChanged?.Invoke(this, EventArgs.Empty);
        }

        public int RequiredPatternCount => RequiredPatterns?.Length ?? 0;

        public int ForbiddenPatternCount => ForbiddenPatterns?.Length ?? 0;

        #region ILogged

        public virtual string CommandId => throw new NotImplementedException();

        public virtual ControlTemplate LogIcon
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

        public event EventHandler LogIconChanged;

        protected void OnLogIconChanged()
        {
            OnPropertyChanged(nameof(LogIcon));
            LogIconChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
