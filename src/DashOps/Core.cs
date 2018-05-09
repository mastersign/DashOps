using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Mastersign.DashOps
{
    public static class Core
    {
        public static void RefreshProject()
            => App.Instance.ProjectLoader.ReloadProjectAndProjectView();

        public static async Task ExecuteAction(ActionView action)
        {
            if (!action.Reassure || Reassure(action))
            {
                await action.ExecuteAsync();
                DashOpsCommands.ShowLastLog.RaiseCanExecuteChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public static void ToggleMonitorsPaused()
        {
            var projectView = App.Instance?.ProjectLoader?.ProjectView;
            if (projectView == null) return;
            projectView.IsMonitoringPaused = !projectView.IsMonitoringPaused;
        }

        private static bool Reassure(ActionView action)
        {
            var result = MessageBox.Show(
                "Are you sure you want to execute the following logged?\n\n" + ActionInfo(action),
                "Execute Action", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            return result == MessageBoxResult.OK;
        }

        public static async Task<bool> ExecuteMonitor(MonitorView monitor, DateTime startTime)
        {
            var result = await monitor.Check(startTime);
            DashOpsCommands.ShowLastLog.RaiseCanExecuteChanged();
            CommandManager.InvalidateRequerySuggested();
            return result;
        }

        public static void ShowLastLog(ILogged logged)
            => ShowLogFile(logged.FindLastLogFile());

        public static void ShowLogFile(string filepath)
            => System.Diagnostics.Process.Start(filepath);

        public static void ShowActionInfo(ActionView action)
        {
            MessageBox.Show(ActionInfo(action), "Action Info",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private static string ActionInfo(ActionView action)
        {
            var sb = new StringBuilder();
            sb.AppendLine(action.Title);
            sb.AppendLine();
            sb.Append("ID: ");
            sb.AppendLine(action.CommandId);
            sb.Append("Command: ");
            sb.AppendLine(action.Command);
            if (!string.IsNullOrWhiteSpace(action.Arguments))
            {
                sb.Append("Arguments: ");
                sb.AppendLine(action.Arguments);
            }
            sb.Append("Working Directory: ");
            sb.AppendLine(action.WorkingDirectory);
            return sb.ToString();
        }

        public static void ShowMonitorInfo(MonitorView monitor)
        {
            MessageBox.Show(MonitorInfo(monitor), "Monitor Info",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private static string MonitorInfo(MonitorView monitor)
        {
            var sb = new StringBuilder();
            sb.AppendLine(monitor.Title);
            sb.AppendLine();
            sb.Append("ID: ");
            sb.AppendLine(monitor.CommandId);
            sb.Append("Interval: ");
            sb.AppendLine(monitor.Interval.Seconds.ToString());
            if (monitor is CommandMonitorView cmdMonitor)
            {
                sb.Append("Command: ");
                sb.AppendLine(cmdMonitor.Command);
                if (!string.IsNullOrWhiteSpace(cmdMonitor.Arguments))
                {
                    sb.Append("Arguments: ");
                    sb.AppendLine(cmdMonitor.Arguments);
                }

                sb.Append("Working Directory: ");
                sb.AppendLine(cmdMonitor.WorkingDirectory);
            }
            return sb.ToString();
        }
    }
}
