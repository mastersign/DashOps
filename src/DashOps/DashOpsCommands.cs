using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Mastersign.DashOps
{
    public static class DashOpsCommands
    {
        public static readonly RoutedUICommand RefreshProject
            = new RoutedUICommand("Refresh Project", "Refresh Project", typeof(MainWindow));

        public static readonly RoutedUICommand ExecuteAction
            = new RoutedUICommand("Execute Action", "Execute Action", typeof(MainWindow));

        public static readonly RoutedUICommand ShowLastActionLog
            = new RoutedUICommand("Show Last Action Log", "Show Last Action Log", typeof(MainWindow));

        public static readonly RoutedUICommand ShowActionInfo
            = new RoutedUICommand("Show Action Info", "Show Action Info", typeof(MainWindow));
    }
}
