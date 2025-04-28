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

                navigationViewMain.Navigate(typeof(MainPage));
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

        private Page currentPage;

        private void Navigation_Navigated(NavigationView sender, NavigatedEventArgs e)
        {
            currentPage = e.Page as Page;

            gridHeader.Visibility = pagesWithoutHeader.Contains(currentPage?.Name)
                ? Visibility.Collapsed
                : Visibility.Visible;
            labelPageTitle.Text = currentPage?.Title;
            UpdateMaxPageHeight();
        }

        private void SizeChangedHandler(object sender, SizeChangedEventArgs e)
        {
            if (e.HeightChanged) UpdateMaxPageHeight();
        }

        private void ClosingHandler(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ConfigEditorWindow.CloseInstance();
        }

        private void UpdateMaxPageHeight()
        {
            if (currentPage is null) return;
            var fit = currentPage is IFitPage;
            if (fit)
            {
                var header = navigationViewMain.Header as FrameworkElement;
                currentPage.MaxHeight = ActualHeight - (header?.ActualHeight ?? 0.0) - 54;
            }
            else
            {
                currentPage.MaxHeight = double.PositiveInfinity;
            }
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

        private void CommandSwitchPerspectiveHandler(object sender, ExecutedRoutedEventArgs e)
        {
            ProjectView projectView = ProjectView;
            projectView.CurrentPerspective = e.Parameter as PerspectiveView ?? projectView.CurrentPerspective;
        }

        private void CommandSwitchPerspectiveCheck(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ProjectView != null; // && e.Parameter != ProjectView.CurrentPerspective;
        }

        private void CommandEditProjectExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ConfigEditorWindow.Open(
                string.Format(Properties.Resources.Common.EditorTitle_1, ProjectView?.Title),
                (App.Current as App).ProjectLoader.ProjectPath,
                "dashops-v2");
        }
    }
}
