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

        private void ExecuteActionCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is ActionView action)
            {
                var ts = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                App.Executor.Execute(action, App.ProjectLoader.ProjectView.Logs, ts + ".log");
            }
        }

        private void ExecuteActionCommandCheck(object sender, CanExecuteRoutedEventArgs e)
        {
            var action = e.Parameter as ActionView;
            e.CanExecute = action != null && App.Executor.IsValid(action);
        }
    }
}
