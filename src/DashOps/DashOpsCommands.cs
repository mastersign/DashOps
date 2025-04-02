using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Mastersign.DashOps.Properties.Resources;

namespace Mastersign.DashOps
{
#pragma warning disable CS4014
    public static class DashOpsCommands
    {
        public static RoutedUICommand EditProject { get; }
            = new RoutedUICommand(Common.Command_EditProject, nameof(EditProject), typeof(DashOpsCommands));

        public static readonly DelegateCommand RefreshProject
            = new DelegateCommand(Core.RefreshProject);

        public static readonly DelegateCommand ToggleMonitorsPaused
            = new DelegateCommand(Core.ToggleMonitorsPaused, () => App.Instance?.ProjectLoader?.ProjectView != null);

        public static RoutedUICommand SwitchPerspective { get; }
            = new RoutedUICommand("Switch Perspective", nameof(SwitchPerspective), typeof(DashOpsCommands));

        public static readonly DelegateCommand<ActionView> ExecuteAction
            = new DelegateCommand<ActionView>(action => Core.ExecuteAction(action));

        public static readonly DelegateCommand<ILogged> ShowLastLog
            = new DelegateCommand<ILogged>(Core.ShowLastLog, logged => logged.HasLogFile());

        public static readonly RoutedUICommand ShowLogHistoryContextMenu
            = new RoutedUICommand("Show Log History Context Menu", nameof(ShowLogHistoryContextMenu), typeof(DashOpsCommands));

        public static readonly DelegateCommand<ActionView> ShowActionInfo
            = new DelegateCommand<ActionView>(Core.ShowActionInfo);

        public static readonly DelegateCommand<MonitorView> ShowMonitorInfo
            = new DelegateCommand<MonitorView>(Core.ShowMonitorInfo);
    }
#pragma warning restore CS4014
}
