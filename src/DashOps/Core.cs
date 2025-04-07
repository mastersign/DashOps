using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI = Wpf.Ui.Controls;

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
                DashOpsCommands.ShowLogHistoryContextMenu.RaiseCanExecuteChanged();
                //CommandManager.InvalidateRequerySuggested();
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

        public static async Task<bool> CheckMonitor(MonitorView monitor, DateTime startTime)
        {
            return await monitor.Check(startTime);
        }

        public static void ShowLastLog(ILogged logged)
            => ShowLogFile(logged.FindLastLogFile());

        public static void ShowHistoryContextMenu(FrameworkElement source)
        {
            var logged = source.Tag as ILogged;
            var menu = new ContextMenu
            {
                PlacementTarget = source,
                Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom,
            };
            var logs = logged.FindLogFiles().OrderByDescending(n => n).Take(20);
            foreach (var log in logs)
            {
                menu.Items.Add(CreateActionLogMenuItem(log));
            }
            menu.IsOpen = true;
        }

        private static MenuItem CreateActionLogMenuItem(string log)
        {
            var info = LogFileManager.GetInfo(log);

            var item = new UI.MenuItem
            {
                Header = info.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                Icon = new UI.SymbolIcon(
                    info.HasResult && info.Success
                        ? UI.SymbolRegular.Checkmark12
                        : UI.SymbolRegular.ErrorCircle12),
            };

            item.Click += (s, ea) => Core.ShowLogFile(log);
            return item;
        }

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
