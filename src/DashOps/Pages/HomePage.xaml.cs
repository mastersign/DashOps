using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mastersign.DashOps.Pages
{
    /// <summary>
    /// Interaktionslogik für HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private App App => Application.Current as App;

        public HomePage()
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





        private async void MonitorDoubleClickHandler(object sender, MouseButtonEventArgs e)
        {
            var label = sender as Label;
            if (label?.DataContext is MonitorView monitor)
            {
                if (monitor.IsRunning) return;
                await monitor.Check(DateTime.Now);
            }
        }
    }
}
