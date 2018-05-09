using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Mastersign.DashOps
{
    #pragma warning disable CS4014
    public static class DashOpsCommands
    {
        public static readonly DelegateCommand RefreshProject
            = new DelegateCommand(Core.RefreshProject);

        public static readonly DelegateCommand ToggleMonitorsPaused
            = new DelegateCommand(Core.ToggleMonitorsPaused, () => App.Instance?.ProjectLoader?.ProjectView != null);

        public static readonly DelegateCommand<ActionView> ExecuteAction
            = new DelegateCommand<ActionView>(action => Core.ExecuteAction(action));

        public static readonly DelegateCommand<ILogged> ShowLastLog
            = new DelegateCommand<ILogged>(Core.ShowLastLog, logged => logged.HasLogFile());

        public static readonly RoutedUICommand ShowLogHistoryContextMenu
            = new RoutedUICommand("Show Log History Context Menu", "Show Log History Context Menu", typeof(MainWindow));

        public static readonly DelegateCommand<ActionView> ShowActionInfo
            = new DelegateCommand<ActionView>(Core.ShowActionInfo);

        public static readonly DelegateCommand<MonitorView> ShowMonitorInfo
            = new DelegateCommand<MonitorView>(Core.ShowMonitorInfo);
    }
#pragma warning restore CS4014
}
