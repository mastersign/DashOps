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

namespace Mastersign.DashOps
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private App App => Application.Current as App;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = App?.ProjectLoader?.ProjectView;
        }

        private ProjectView ProjectView => (ProjectView)DataContext;

        private void GoToPageCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            ProjectView.CurrentPerspective = e.Parameter as PerspectiveView ?? ProjectView.CurrentPerspective;
        }

        private void GoToPageCommandCheck(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter != ProjectView.CurrentPerspective;
        }

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

        private static MenuItem CreateActionLogMenuItem(string log)
        {
            var info = LogFileManager.GetInfo(log);
            var item = new MenuItem
            {
                Header = info.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                Icon = SuccessStatusIcon(info.IsSuccess),
            };

            item.Click += (s, ea) => Core.ShowLogFile(log);
            return item;
        }

        private static Image SuccessStatusIcon(bool success)
            => new Image
            {
                Source = success
                    ? new BitmapImage(new Uri("pack://application:,,,/images/StatusOK.png"))
                    : new BitmapImage(new Uri("pack://application:,,,/images/StatusError.png"))
            };

        private async void MonitorDoubleClickHandler(object sender, MouseButtonEventArgs e)
        {
            var label = sender as Label;
            var result = label?.DataContext is CommandMonitorView monitor 
                         && await monitor.Check(DateTime.Now);
            MessageBox.Show(result.ToString());
        }
    }
}
