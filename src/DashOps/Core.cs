using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Mastersign.DashOps
{
    public static class Core
    {
        public static Window MainWindow { get; set; }

        public static void RefreshProject()
            => App.Instance.ProjectLoader.ReloadProjectAndProjectView();

        public static async Task ExecuteAction(ActionView action)
        {
            if (!action.Reassure || Reassure(action))
            {
                var result = await action.ExecuteAsync();
                DashOpsCommands.ShowLastLog.RaiseCanExecuteChanged();
                CommandManager.InvalidateRequerySuggested();
                if (result.StartFailed)
                {
                    UserInteraction.ShowMessage(
                        "Execute Action",
                        "Starting action failed with:"
                        + Environment.NewLine
                        + Environment.NewLine
                        + result.Output,
                        symbol: InteractionSymbol.Error);
                }
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
            var panel = new StackPanel();
            panel.Children.Add(new TextBlock
            {
                Text = "Are you sure you want to execute the following action?"
            });
            panel.Children.Add(new CompactActionInfoControl { DataContext = action });

            return UserInteraction.AskYesOrNoQuestion(
                "Execute Action",
                panel,
                symbol: InteractionSymbol.Reassurance,
                owner: MainWindow);
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
            UserInteraction.ShowMessage(
                "Action Info",
                new ActionInfoControl { DataContext = action },
                symbol: InteractionSymbol.CommandAction,
                owner: MainWindow);
        }

        public static void ShowMonitorInfo(MonitorView monitor)
        {
            UIElement infoControl;
            InteractionSymbol symbol;
            if (monitor is CommandMonitorView)
            {
                infoControl = new CommandMonitorInfoControl { DataContext = monitor };
                symbol = InteractionSymbol.CommandMonitor;
            } 
            else if (monitor is WebMonitorView)
            {
                infoControl = new WebMonitorInfoControl { DataContext = monitor };
                symbol = InteractionSymbol.WebMonitor;
            }
            else
            {
                throw new NotSupportedException("Unknown monitor type");
            }
            UserInteraction.ShowMessage(
                "Monitor Info",
                infoControl,
                symbol: symbol,
                owner: MainWindow);
        }
    }
}
