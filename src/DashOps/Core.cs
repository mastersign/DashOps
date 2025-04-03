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
            => UserInteraction.AskYesOrNoQuestion(
                "Execute Action",
                "Are you sure you want to execute the following logged?\n\n" + ActionInfo(action),
                symbol: InteractionSymbol.Reassurance);

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
            UserInteraction.ShowMessage(
                "Action Info",
                ActionInfo(action),
                symbol: InteractionSymbol.Info);
        }

        private static string ActionInfo(ActionView action)
        {
            var sb = new StringBuilder();
            sb.AppendLine(action.Title);
            sb.AppendLine();
            sb.Append("ID: ");
            sb.AppendLine(action.CommandId);
            sb.Append("Reassure Before Execution: ");
            sb.AppendLine(action.Reassure ? "yes" : "no");
            sb.Append("Create Log: ");
            sb.AppendLine(action.Logs != null ? "yes" : "no");
            sb.Append("Keep Window Open: ");
            sb.AppendLine(action.KeepOpen ? "yes" : "no");
            sb.Append("Always Close Window: ");
            sb.AppendLine(!action.KeepOpen && action.AlwaysClose ? "yes" : "no");
            sb.Append("Run in Background: ");
            sb.AppendLine(action.Visible ? "no" : "yes");
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
            UserInteraction.ShowMessage(
                "Monitor Info",
                MonitorInfo(monitor),
                symbol: InteractionSymbol.Info);
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

            if (monitor is WebMonitorView webMonitor)
            {
                sb.Append("Url: ");
                sb.AppendLine(webMonitor.Url);
                sb.Append("Timeout: ");
                sb.AppendLine(webMonitor.Timeout.Seconds.ToString());
                sb.Append("Allowed Status Codes: ");
                sb.AppendLine(string.Join(", ", webMonitor.StatusCodes));
                if (webMonitor.Headers != null)
                {
                    sb.AppendLine("Headers:");
                    foreach (var key in webMonitor.Headers.Keys.OrderBy(k => k))
                    {
                        sb.AppendLine($"  - {key} = {webMonitor.Headers[key]}");
                    }
                }
                else
                {
                    sb.AppendLine("Headers: none");
                }
            }
            sb.Append("Required Patterns:");
            sb.AppendLine(monitor.RequiredPatterns?.Length.ToString() ?? "0");
            sb.Append("Forbidden Patterns:");
            sb.AppendLine(monitor.ForbiddenPatterns?.Length.ToString() ?? "0");
            return sb.ToString();
        }
    }
}
