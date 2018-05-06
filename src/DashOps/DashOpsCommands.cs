using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Mastersign.DashOps
{
    public static class DashOpsCommands
    {
        public static readonly DelegateCommand RefreshProject
            = new DelegateCommand(Core.RefreshProject);

        public static readonly DelegateCommand<ActionView> ExecuteAction
            = new DelegateCommand<ActionView>(Core.ExecuteAction, action => action.CanExecute);

        public static readonly DelegateCommand<ActionView> ShowLastActionLog
            = new DelegateCommand<ActionView>(Core.ShowLastActionLog, action => action.LastLogFile != null);

        public static readonly RoutedUICommand ShowLogHistoryContextMenu
            = new RoutedUICommand("Show Log History Context Menu", "Show Log History Context Menu", typeof(MainWindow));

        public static readonly DelegateCommand<ActionView> ShowActionInfo
            = new DelegateCommand<ActionView>(Core.ShowActionInfo);
    }
}
