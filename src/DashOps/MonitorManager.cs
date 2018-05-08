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
        private readonly DispatcherTimer timer = new DispatcherTimer();

        public MonitorManager()
        {
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += TimerHandler;
            timer.IsEnabled = true;
        }

        private static void TimerHandler(object sender, EventArgs e)
        {
            var projectView = App.Instance?.ProjectLoader?.ProjectView;
            var monitors = projectView?.MonitorViews;
            if (monitors == null) return;
            if (projectView.IsMonitoringPaused) return;
            var now = DateTime.Now;
            foreach (var monitor in monitors)
            {
                ProcessMonitor(monitor, now);
            }
        }

        private static void ProcessMonitor(MonitorView monitor, DateTime now)
        {
            if (monitor.IsRunning) return;
            if (monitor.HasLastExecutionResult)
            {
                var nextExecutionTime = monitor.LastExecutionTime + monitor.Interval;
                if (nextExecutionTime > now) return;
            }
#pragma warning disable CS4014
            Core.ExecuteMonitor(monitor, now);
#pragma warning restore CS4014
        }
    }
}
