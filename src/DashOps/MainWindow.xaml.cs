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

        private string LogDir => App.ProjectLoader.ProjectView.Logs;

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
                App.Executor.Execute(action, LogDir);
            }
        }

        private void ExecuteActionCommandCheck(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter is ActionView action && App.Executor.IsValid(action);
        }

        private void ShowLastActionLogCheck(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter is ActionView action && action.LastLogFile(LogDir) != null;
        }

        private void ShowLastActionLogHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is ActionView action)
            {
                System.Diagnostics.Process.Start(action.LastLogFile(LogDir));
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
