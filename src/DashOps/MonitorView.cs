using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Mastersign.DashOps
{
    partial class MonitorView
    {
        public virtual Task<bool> Check(DateTime startTime)
        {
            throw new NotImplementedException();
        }

        public TimeSpan GetInterval(TimeSpan defaultInterval)
            => Interval < TimeSpan.Zero ? defaultInterval : Interval;

        protected virtual void NotifyExecutionBegin(DateTime startTime)
        {
            LastExecutionTime = startTime;
            IsRunning = true;
        }

        protected virtual void NotifyExecutionFinished(bool success)
        {
            HasExecutionResultChanged = !HasLastExecutionResult || LastExecutionResult != success;
            Debug.WriteLine($"{Title}: {LastExecutionResult} -> {success} ({HasExecutionResultChanged})");
            LastExecutionResult = success;
            HasLastExecutionResult = true;
            IsRunning = false;
            OnStatusIconChanged();
        }

        public virtual ControlTemplate StatusIcon
        {
            get
            {
                var resourceName =
                    HasLastExecutionResult
                        ? LastExecutionResult
                            ? HasExecutionResultChanged ? "IconStatusOKNew" : "IconStatusOK"
                            : HasExecutionResultChanged ? "IconStatusErrorNew" : "IconStatusError"
                        : "IconStatusNotStarted";
                return resourceName != null
                    ? App.Instance.FindResource(resourceName) as ControlTemplate
                    : null;
            }
        }

        public event EventHandler StatusIconChanged;

        protected void OnStatusIconChanged()
        {
            OnPropertyChanged(nameof(StatusIcon));
            StatusIconChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
