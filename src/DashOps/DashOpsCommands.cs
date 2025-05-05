using System.Windows.Input;
using Mastersign.DashOps.Properties.Resources;

namespace Mastersign.DashOps
{
#pragma warning disable CS4014
    public static class DashOpsCommands
    {
        public static RoutedUICommand EditProject { get; }
            = new(Common.Command_EditProject, nameof(EditProject), typeof(DashOpsCommands));

        public static readonly DelegateCommand RefreshProject
            = new(Core.RefreshProject);

        public static readonly DelegateCommand ToggleMonitorsPaused
            = new(Core.ToggleMonitorsPaused, () => App.Instance?.ProjectLoader?.ProjectView != null);

        public static RoutedUICommand SwitchPerspective { get; }
            = new(Common.Command_SwitchPerspective, nameof(SwitchPerspective), typeof(DashOpsCommands));

        public static readonly DelegateCommand<ActionView> ShowActionInfo
            = new(Core.ShowActionInfo);

        public static readonly DelegateCommand<MonitorView> ShowMonitorInfo
            = new(Core.ShowMonitorInfo);

        public static readonly DelegateCommand<ActionView> ExecuteAction
            = new(action => Core.ExecuteAction(action));

        public static readonly DelegateCommand<ILogged> ShowLastLog
            = new(Core.ShowLastLog, logged => logged.HasLogFile());

        public static readonly DelegateCommand<FrameworkElement> ShowLogHistoryContextMenu
            = new(Core.ShowHistoryContextMenu, source => ((ILogged)source.Tag).HasLogFile());

    }
#pragma warning restore CS4014
}
