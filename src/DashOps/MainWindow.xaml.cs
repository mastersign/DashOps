using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;
using Mastersign.DashOps.Pages;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Screen = System.Windows.Forms.Screen;

namespace Mastersign.DashOps
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        private static App CurrentApp => (App)Application.Current;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                WatchSystemTheme();
                ApplicationThemeManager.ApplySystemTheme();

                DataContext = CurrentApp?.ProjectLoader?.ProjectView;

                SetWindowPosition();

                navigationViewMain.Navigate(typeof(MainPage));
            };

            StateChanged += WindowStateChangedHandler;
        }

        private void SetWindowPosition()
        {
            var ws = CurrentApp.ProjectLoader.ProjectView?.MainWindow;
            if (ws is null) return;

            var screen = ws.ScreenNo.HasValue && ws.ScreenNo.Value >= 0 && ws.ScreenNo.Value < Screen.AllScreens.Length
                ? Screen.AllScreens[ws.ScreenNo.Value]
                : Screen.PrimaryScreen;

            if (ws.Mode == WindowMode.Fixed)
            {
                var x = screen.WorkingArea.Left + ws.Left ?? 0;
                var y = screen.WorkingArea.Top + ws.Top ?? 0;
                Left = x;
                Top = y;
                Width = Math.Min(screen.WorkingArea.Width - x, ws.Width ?? 1024);
                Height = Math.Min(screen.WorkingArea.Height - y, ws.Height ?? 720);
            }
            else if (ws.Mode == WindowMode.Auto)
            {
                Left = screen.WorkingArea.Left;
                Top = screen.WorkingArea.Top;
                Width = ws.Width ?? screen.WorkingArea.Width switch
                {
                    > 2560 => 1024,
                    > 1600 => screen.WorkingArea.Width / 2,
                    > 1024 => 1024,
                    _ => screen.WorkingArea.Width,
                };
                Height = ws.Height ?? screen.WorkingArea.Height switch
                {
                    > 1280 => 720,
                    _ => screen.WorkingArea.Height,
                };
            }
            else
            {
                if (ws.Width.HasValue) Width = ws.Width.Value;
                if (ws.Height.HasValue) Height = ws.Height.Value;
            }
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
