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

namespace Mastersign.DashOps.Pages
{
    /// <summary>
    /// Interaktionslogik für HomePage.xaml
    /// </summary>
    public partial class MainPage : Page, IFitPage
    {
        public MainPage()
        {
            InitializeComponent();
            DataContext = App.Instance?.ProjectLoader?.ProjectView;
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
