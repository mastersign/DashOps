using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
            OnStatusIconChanged();
        }

        protected virtual void NotifyExecutionFinished(bool success)
        {
            HasExecutionResultChanged = !HasLastExecutionResult || LastExecutionResult != success;
            LastExecutionResult = success;
            HasLastExecutionResult = true;
            IsRunning = false;
            OnStatusIconChanged();
            OnLogIconChanged();
        }

        public virtual ControlTemplate StatusIcon
        {
            get
            {
                var resourceName =
                    IsRunning
                        ? "IconStatusProgress"
                        : HasLastExecutionResult
                            ? LastExecutionResult
                                ? HasExecutionResultChanged ? "IconStatusOKNew" : "IconStatusOK"
                                : HasExecutionResultChanged ? "IconStatusErrorNew" : "IconStatusError"
                            : "IconStatusNotStarted";
                return App.Instance.FindResource(resourceName) as ControlTemplate;
            }
        }

        public event EventHandler StatusIconChanged;

        protected void OnStatusIconChanged()
        {
            OnPropertyChanged(nameof(StatusIcon));
            StatusIconChanged?.Invoke(this, EventArgs.Empty);
        }

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
                return App.Instance.FindResource(resourceName) as ControlTemplate;
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
