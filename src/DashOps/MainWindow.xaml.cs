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
using Mastersign.DashOps.Pages;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace Mastersign.DashOps
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        private static App CurrentApp => (App)System.Windows.Application.Current;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                WatchSystemTheme();
                ApplicationThemeManager.ApplySystemTheme();

                DataContext = CurrentApp?.ProjectLoader?.ProjectView;

                navigationViewMain.Navigate(typeof(HomePage));
            };

            StateChanged += WindowStateChangedHandler;
        }

        private void WatchSystemTheme()
        {
            SystemThemeWatcher.Watch(
                this,                                  // Window class
                WindowBackdropType.Mica,
                updateAccents: true                    // Whether to change accents automatically
            );
        }

        private void WindowStateChangedHandler(object sender, EventArgs e)
        {
            ShowInTaskbar = WindowState != WindowState.Minimized;
        }

        private static readonly string[] pagesWithoutHeader = ["home", "main"];

        private void Navigation_Navigated(NavigationView sender, NavigatedEventArgs e)
        {
            gridHeader.Visibility = pagesWithoutHeader.Contains((e.Page as Page).Name)
                ? Visibility.Collapsed
                : Visibility.Visible;
            labelPageTitle.Text = (e.Page as Page)?.Title;
        }

        private void CommandApplicationCloseExecutedHandler(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void CommandApplicationCloseCanExecuteHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private ProjectView ProjectView => CurrentApp?.ProjectLoader?.ProjectView;

        private void SwitchPerspectiveCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            ProjectView projectView = ProjectView;
            projectView.CurrentPerspective = e.Parameter as PerspectiveView ?? projectView.CurrentPerspective;
            foreach (var p in projectView.Perspectives)
            {
                p.IsSelected = p == projectView.CurrentPerspective;
            }
        }

        private void SwitchPerspectiveCommandCheck(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ProjectView != null; // && e.Parameter != ProjectView.CurrentPerspective;
        }

        private void CommandEditProjectExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute = CurrentApp?.Runtime?.Config != null;
            e.CanExecute = true;
        }

        private void CommandEditProjectExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Not Implemented");
            //try
            //{
            //    CurrentApp.Runtime.Config.EditSetup();
            //}
            //catch (DefaultEditorNotFoundException exc)
            //{
            //    System.Windows.MessageBox.Show(
            //        Properties.Resources.Common.EditorNotFound_Message
            //        + Environment.NewLine + Environment.NewLine
            //        + exc.EditorExecutable,
            //        Properties.Resources.CommandsPage.EditCommand_Title,
            //        System.Windows.MessageBoxButton.OK,
            //        MessageBoxImage.Error);
            //}
        }

    }
}
