using System.Windows.Media;
using System.Windows.Media.Imaging;
using Mastersign.DashOps.Pages;
using Wpf.Ui.Appearance;
using Screen = System.Windows.Forms.Screen;
using UI = Wpf.Ui.Controls;

namespace Mastersign.DashOps
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UI.FluentWindow
    {
        private static App CurrentApp => (App)Application.Current;

        public MainWindow()
        {
            InitializeComponent();
            
            Loaded += (sender, args) =>
            {
                var project = CurrentApp?.ProjectLoader?.ProjectView;
                DataContext = project;
                if (project != null) {
                    project.ProjectUpdated += (sender, ea) => ApplyTheme();
                    ApplicationThemeManager.Changed += (theme, accent) => UpdateIcons(
                        theme == ApplicationTheme.Dark ? ColorTheme.Dark : ColorTheme.Light,
                        project.Color);
                }
                
                ApplyTheme();
                SetWindowPosition();

                navigationViewMain.Navigate(typeof(MainPage));
            };

            StateChanged += WindowStateChangedHandler;
            Core.MainWindow = this;
        }

        private void UpdateIcons(ColorTheme theme, ThemePaletteColor color)
        {
            IconManager.Initialize(theme, color);
            IconManager.LoadIcon(this);
            var resPrefix = "pack://application:,,,/DashOps;component/WpfResources/Icons";
            var resources = App.Current.Resources;
            resources["LogoImage16"] = new BitmapImage(new Uri($"{resPrefix}/{color}_{theme}_16.png"));
            resources["LogoImage32"] = new BitmapImage(new Uri($"{resPrefix}/{color}_{theme}_32.png"));
            resources["LogoImage64"] = new BitmapImage(new Uri($"{resPrefix}/{color}_{theme}_64.png"));
        }

        private void ApplyTheme()
        {
            var project = DataContext as ProjectView;
            SystemThemeWatcher.UnWatch(this);
            var theme = project?.Theme ?? ColorTheme.System;
            var color = project?.Color ?? ThemePaletteColor.Default;
            if (theme == ColorTheme.System)
            {
                SystemThemeWatcher.Watch(
                    this,                                              // Window class
                    UI.WindowBackdropType.Mica,
                    updateAccents: true  // Whether to change accents automatically
                );
                ApplicationThemeManager.ApplySystemTheme(updateAccent: true);
                theme = SystemThemeManager.GetCachedSystemTheme() == SystemTheme.Dark
                    ? ColorTheme.Dark
                    : ColorTheme.Light;
            }
            else
            {
                ApplicationThemeManager.Apply(theme switch
                {
                    ColorTheme.Dark => ApplicationTheme.Dark,
                    ColorTheme.Light => ApplicationTheme.Light,
                    _ => ApplicationTheme.Unknown,
                });
            }
            if (color == ThemePaletteColor.Default)
            {
                ApplicationAccentColorManager.ApplySystemAccent();
            }
            else
            {
                var paletteColor = FindResource($"Palette{color}Color");
                if (paletteColor != null)
                {
                    ApplicationAccentColorManager.Apply((Color)paletteColor, theme == ColorTheme.Dark ? ApplicationTheme.Dark : ApplicationTheme.Light);
                }
                else
                {
                    ApplicationAccentColorManager.ApplySystemAccent();
                }
            }
            //UpdateIcons(theme, color);
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

        private void WindowStateChangedHandler(object sender, EventArgs e)
        {
            // ShowInTaskbar = WindowState != WindowState.Minimized;
        }

        private void WindowDpiChangedHandler(object sender, DpiChangedEventArgs e)
        {
            
        }

        private static readonly string[] pagesWithoutHeader = ["home", "main"];

        private Page currentPage;

        private void NavigatedHandler(UI.NavigationView sender, UI.NavigatedEventArgs e)
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
