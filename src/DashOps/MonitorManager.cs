using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;

namespace Mastersign.DashOps
{
    public class MonitorManager
    {
        private DispatcherTimer timer = new DispatcherTimer();

        public bool Paused
        {
            get => !timer.IsEnabled;
            set => timer.IsEnabled = !value;
        }

        public MonitorManager()
        {
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += TimerHandler;
            timer.IsEnabled = true;
        }

        private void TimerHandler(object sender, EventArgs e)
        {
            var projectView = App.Instance?.ProjectLoader?.ProjectView;
            var monitors = projectView?.MonitorViews;
            if (monitors == null) return;
            var now = DateTime.Now;
            foreach (var monitor in monitors)
            {
                ProcessMonitor(monitor, projectView.DefaultMonitorInterval, now);
            }
        }

        private void ProcessMonitor(MonitorView monitor, TimeSpan defaultInterval, DateTime now)
        {
            if (monitor.IsRunning) return;
            if (monitor.HasLastExecutionResult)
            {
                var nextExecutionTime = monitor.LastExecutionTime + monitor.GetInterval(defaultInterval);
                if (nextExecutionTime > now) return;
            }
            monitor.Check(now);
        }
    }
}
