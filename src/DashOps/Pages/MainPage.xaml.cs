using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace Mastersign.DashOps.Pages
{
    /// <summary>
    /// Interaktionslogik für HomePage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private App App => Application.Current as App;

        public MainPage()
        {
            InitializeComponent();
            DataContext = App?.ProjectLoader?.ProjectView;
        }

        private ProjectView ProjectView => (ProjectView)DataContext;


        private void HasLogCheck(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (e.Parameter as ILogged)?.HasLogFile() ?? false;
        }

        private void ShowLogHistoryContextMenuHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is ILogged item)
            {
                var menu = new ContextMenu
                {
                    PlacementTarget = (UIElement)e.OriginalSource,
                    Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom,
                };
                var logs = item.FindLogFiles().OrderByDescending(n => n).Take(20);
                foreach (var log in logs)
                {
                    menu.Items.Add(CreateActionLogMenuItem(log));
                }
                menu.IsOpen = true;
            }
        }

        private static System.Windows.Controls.MenuItem CreateActionLogMenuItem(string log)
        {
            var info = LogFileManager.GetInfo(log);
            
            var item = new Wpf.Ui.Controls.MenuItem
            {
                Header = info.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                Icon = new SymbolIcon(
                    info.HasResult && info.Success
                        ? SymbolRegular.Checkmark12
                        : SymbolRegular.ErrorCircle12),
            };

            item.Click += (s, ea) => Core.ShowLogFile(log);
            return item;
        }

        private async void MonitorDoubleClickHandler(object sender, MouseButtonEventArgs e)
        {
            var label = sender as Label;
            if (label?.DataContext is MonitorView monitor)
            {
                if (monitor.IsRunning) return;
                await monitor.Check(DateTime.Now);
            }
        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
