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

        private void RefreshProjectCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            App.ProjectLoader.ReloadProjectAndProjectView();
        }

        private void RefreshProjectCommandCheck(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExecuteActionCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (!(e.Parameter is ActionView action)) return;
            if (!action.Reassure || Reassure(action))
            {
                App.Executor.ExecuteAction(action);
            }
        }

        private void ExecuteActionCommandCheck(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter is ActionView action && App.Executor.IsValid(action);
        }

        private void HasLogCheck(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter is ActionView action && action.LastLogFile != null;
        }

        private static void ShowLogFile(string filepath)
            => System.Diagnostics.Process.Start(filepath);

        private void ShowLastActionLogHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is ActionView action)
            {
                ShowLogFile(action.LastLogFile);
            }
        }

        private static Image SuccessStatusIcon(bool success)
            => new Image
            {
                Source = success
                    ? new BitmapImage(new Uri("pack://application:,,,/images/StatusOK.png"))
                    : new BitmapImage(new Uri("pack://application:,,,/images/StatusError.png"))
            };

        private static MenuItem CreateActionLogMenuItem(string log)
        {
            var info = LogFileManager.GetInfo(log);
            var item = new MenuItem
            {
                Header = info.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                Icon = SuccessStatusIcon(info.IsSuccess),
            };

            item.Click += (s, ea) => ShowLogFile(log);
            return item;
        }

        private void ShowLogHistoryContextMenuHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is ActionView action)
            {
                var menu = new ContextMenu
                {
                    PlacementTarget = (UIElement)e.OriginalSource,
                    Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom,
                };
                var logs = action.LogFiles().OrderByDescending(n => n).Take(20);
                foreach (var log in logs)
                {
                    menu.Items.Add(CreateActionLogMenuItem(log));
                }
                menu.IsOpen = true;
            }
        }

        private void ShowActionInfoCheck(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ShowActionInfoHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is ActionView action)
            {
                MessageBox.Show(ActionInfo(action), "Action Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private static string ActionInfo(ActionView action)
        {
            var sb = new StringBuilder();
            sb.AppendLine(action.Description);
            sb.AppendLine();
            sb.Append("ID: ");
            sb.AppendLine(action.ActionId);
            sb.Append("Command: ");
            sb.AppendLine(action.ExpandedCommand);
            if (!string.IsNullOrWhiteSpace(action.ExpandedArguments))
            {
                sb.Append("Arguments: ");
                sb.AppendLine(action.ExpandedArguments);
            }
            sb.Append("Working Directory: ");
            sb.AppendLine(action.ExpandedWorkingDirectory);
            return sb.ToString();
        }

        private static bool Reassure(ActionView action)
        {
            var result = MessageBox.Show(
                "Are you sure you want to execute the following action?\n\n" + ActionInfo(action),
                "Execute Action", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            return result == MessageBoxResult.OK;
        }

    }
}
