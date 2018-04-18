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
        public static readonly RoutedUICommand ExecuteAction
            = new RoutedUICommand("Execute Action", "Execute Action", typeof(MainWindow));
    }
}
