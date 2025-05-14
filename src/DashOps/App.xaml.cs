using System.Diagnostics;
using System.IO;
using System.Reflection;
using Mastersign.DashOps.Exceptions;
using Mastersign.DashOps.Windows;
using Mastersign.WpfUiTools;
using static Mastersign.DashOps.UserInteraction;

namespace Mastersign.DashOps;

/// <summary>
/// Interaktionslogik für "App.xaml"
/// </summary>
public partial class App : Application
{
    public static Wpf.Ui.UiApplication UI => Wpf.Ui.UiApplication.Current;
    public static App Instance => Current as App;

    private const Theme DEFAULT_THEME = Theme.Auto;
    private const ThemeAccentColor DEFAULT_ACCENT = ThemeAccentColor.Default;

    private string projectFile;

    public IProjectLoader ProjectLoader { get; private set; }

    public bool SuppressMainWindow { get; private set; }

    public ThemeManager ThemeManager { get; private set; }

    public Executor Executor { get; private set; }

    public MonitorManager MonitorManager { get; private set; }

    private void ApplicationStartupHandler(object sender, StartupEventArgs e)
    {
        ThemeManager = new()
        {
            AppResources = Resources,
            ResourceAssembly = Assembly.GetExecutingAssembly(),
        };
        ThemeManager.RegisterIconImageResource(16, "LogoImage16");
        ThemeManager.RegisterIconImageResource(32, "LogoImage32");
        ThemeManager.RegisterIconImageResource(64, "LogoImage64");

        if (e.Args.Length == 1)
        {
            projectFile = e.Args[0];
            projectFile = Path.IsPathRooted(projectFile)
                ? projectFile
                : Path.Combine(Environment.CurrentDirectory, projectFile);
            if (!File.Exists(projectFile))
            {
                ShowMessage(
                    "Loading DashOps Project File",
                    $"The project file '{e.Args[0]}' could not be found.",
                    symbol: InteractionSymbol.Error,
                    showInTaskbar: true);
                Shutdown(1);
                return;
            }
        }
        else
        {
            var name1 = Path.Combine(Environment.CurrentDirectory, "dashops.yaml");
            var name2 = Path.Combine(Environment.CurrentDirectory, "dashops.yml");
            var name3 = Path.Combine(Environment.CurrentDirectory, "dashops.json");
            if (File.Exists(name1))
                projectFile = name1;
            else if (File.Exists(name2))
                projectFile = name2;
            else if (File.Exists(name3))
                projectFile = name3;
            else
            {
                ShowMessage(
                    "Loading DashOps Project File",
                    "No project file given as command line argument and no default project file " +
                    $"in the current working directory: {name1}",
                    symbol: InteractionSymbol.Error,
                    showInTaskbar: true);
                Shutdown(1);
                return;
            }
        }

        try
        {
            ProjectLoader = ProjectLoaderFactory.CreateProjectLoaderFor(projectFile, Dispatch);
        }
        catch (ProjectLoadException exc)
        {
            SuppressMainWindow = true;
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            ShowMessage(
                "Loading DashOps Project File" + (exc.FormatVersion != null ? " - Format " + exc.FormatVersion : ""),
                "An error occurred while loading the project file:"
                + Environment.NewLine
                + Environment.NewLine
                + exc.Message,
                symbol: InteractionSymbol.Error);
        }
        catch (UnsupportedProjectFormatException exc)
        {
            ShowMessage(
                "Loading DashOps Project File",
                "The format of the project file is not supported."
                + Environment.NewLine
                + Environment.NewLine
                + $"Application Version: {GetAppVersion()}"
                + Environment.NewLine
                + $"File Version: {exc.FormatVersion ?? "unknown"}",
                symbol: InteractionSymbol.Error,
                showInTaskbar: true);
            Shutdown(1);
            return;
        }
        catch (ProjectLoaderFactoryException exc)
        {
            ShowMessage(
                "Loading DashOps Project File",
                "Failed to load project."
                + Environment.NewLine
                + Environment.NewLine
                + exc.Message 
                + exc.InnerException is not null 
                    ? Environment.NewLine + exc.InnerException.Message
                    : string.Empty,
                symbol: InteractionSymbol.Error,
                showInTaskbar: true);
            Shutdown(1);
            return;
        }

        ThemeManager.SetTheme(
            ProjectLoader?.ProjectView?.Theme ?? DEFAULT_THEME,
            ProjectLoader?.ProjectView?.Color ?? DEFAULT_ACCENT);

        if (ProjectLoader?.ProjectView is not null)
        {
            ProjectLoader.ProjectView.ProjectUpdated += (sender, ea) =>
            {
                ThemeManager.SetTheme(
                    ProjectLoader.ProjectView.Theme,
                    ProjectLoader.ProjectView.Color);
            };
        }

        if (ProjectLoader is not null)
        {
            Executor = new Executor();
            MonitorManager = new MonitorManager();

            Exit += ApplicationExitHandler;
        } 
        else
        {
            ShutdownMode = ShutdownMode.OnLastWindowClose;
            OpenProjectEditor(shutdownOnClose: true);
        }
    }

    public void OpenProjectEditor(bool shutdownOnClose = false)
    {
        var window = ConfigEditorWindow.Open(
            string.Format(
                Mastersign.DashOps.Properties.Resources.Common.EditorTitle_1,
                ProjectLoader?.ProjectView?.Title ?? "Unknown"),
            projectFile,
            "dashops-v2",
            lastTime: shutdownOnClose);
        window.Closed += (sender, ea) =>
        {
            Shutdown(2);
        };
    }

    private void ApplicationExitHandler(object sender, ExitEventArgs e)
    {
        Executor.Dispose();
    }

    private string GetAppVersion()
    {
        var a = Assembly.GetExecutingAssembly();
        var fvi = FileVersionInfo.GetVersionInfo(a.Location);
        return $"{fvi.ProductMajorPart}.{fvi.ProductMinorPart}.{fvi.ProductBuildPart}";
    }

    private void Dispatch(Action action)
    {
        Dispatcher.BeginInvoke(action);
    }
}
