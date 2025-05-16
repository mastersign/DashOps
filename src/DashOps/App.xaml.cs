using System.Diagnostics;
using System.IO;
using System.Reflection;
using Mastersign.DashOps.Exceptions;
using Mastersign.DashOps.Windows;
using static Mastersign.DashOps.Properties.Resources.Common;
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
        ShutdownMode = ShutdownMode.OnExplicitShutdown;

        var args = new CommandLineArguments(e.Args);

        ThemeManager = new()
        {
            AppResources = Resources,
            ResourceAssembly = Assembly.GetExecutingAssembly(),
        };
        ThemeManager.RegisterIconImageResource(16, "LogoImage16");
        ThemeManager.RegisterIconImageResource(32, "LogoImage32");
        ThemeManager.RegisterIconImageResource(64, "LogoImage64");

        if (args.HasErrors)
        {
            ShowMessage(
                CommandLine_Title,
                string.Format(
                    CommandLine_InvalidArguments,
                    string.Join(Environment.NewLine, args.ParsingErrors)),
                symbol: InteractionSymbol.Error,
                showInTaskbar: true);
            Shutdown(1);
            return;
        }

        if (args.ShowCommandLineHelp)
        {
            ShowMessage(
                CommandLine_Title,
                CommandLine_Help,
                symbol: InteractionSymbol.Info,
                showInTaskbar: true);
            Shutdown(1);
            return;
        }

        if (!string.IsNullOrWhiteSpace(args.ProjectFile))
        {
            projectFile = args.ProjectFile;
            projectFile = Path.IsPathRooted(projectFile)
                ? projectFile
                : Path.Combine(Environment.CurrentDirectory, projectFile);
            if (!File.Exists(projectFile))
            {
                ShowMessage(
                    LoadProject_Title,
                    string.Format(LoadProject_FileNotFound_1),
                    symbol: InteractionSymbol.Error,
                    showInTaskbar: true);
                Shutdown(1);
                return;
            }
        }
        else
        {
            var cwdName1 = Path.Combine(Environment.CurrentDirectory, "Dashboard.dops");
            var cwdName2 = Path.Combine(Environment.CurrentDirectory, "dashops.yaml");
            var cwdName3 = Path.Combine(Environment.CurrentDirectory, "dashops.yml");
            var cwdName4 = Path.Combine(Environment.CurrentDirectory, "dashops.json");
            var userName1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Dashboard.dops");
            var userName2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Dashboard.dops");
            if (File.Exists(cwdName1))
                projectFile = cwdName1;
            else if (File.Exists(cwdName2))
                projectFile = cwdName2;
            else if (File.Exists(cwdName3))
                projectFile = cwdName3;
            else if (File.Exists(cwdName4))
                projectFile = cwdName4;
            else if (File.Exists(userName1))
                projectFile = userName1;
            else if (File.Exists(userName2))
                projectFile = userName2;
            else
            {
                var result = AskYesOrNoQuestion(
                    LoadProject_Title,
                    string.Format(LoadProject_NoProject_1, cwdName1 + Environment.NewLine + userName1),
                    symbol: InteractionSymbol.Question,
                    showInTaskbar: true);
                if (result)
                {
                    projectFile = userName1;
                    WriteResourceFileToFileSystem("resources/project-template.yaml", userName1);
                }
                else
                {
                    Shutdown(1);
                    return;
                }
            }
        }

        if (!args.PreserveWorkingDirectory)
        {
            Environment.CurrentDirectory = Path.GetDirectoryName(projectFile);
        }

        try
        {
            ProjectLoader = ProjectLoaderFactory.CreateProjectLoaderFor(projectFile, Dispatch);
        }
        catch (ProjectLoadException exc)
        {
            SuppressMainWindow = true;
            ShowMessage(
                LoadProject_Title + (exc.FormatVersion != null ? $" - {Format} " + exc.FormatVersion : ""),
                string.Format(LoadProject_Error_1, exc.Message),
                symbol: InteractionSymbol.Error);
        }
        catch (UnsupportedProjectFormatException exc)
        {
            ShowMessage(
                LoadProject_Title,
                string.Format(LoadProject_UnsupportedVersion_2, GetAppVersion(), exc.FormatVersion ?? Unknown),
                symbol: InteractionSymbol.Error,
                showInTaskbar: true);
            Shutdown(1);
            return;
        }
        catch (ProjectLoaderFactoryException exc)
        {
            ShowMessage(
                LoadProject_Title,
                string.Format(
                    LoadProject_LoadFailed_1, 
                    exc.Message + exc.InnerException is not null 
                        ? Environment.NewLine + exc.InnerException.Message
                        : string.Empty),
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

        ShutdownMode = ShutdownMode.OnLastWindowClose;
        if (ProjectLoader is not null)
        {
            Executor = new Executor();
            MonitorManager = new MonitorManager();

            Exit += ApplicationExitHandler;
        } 
        else
        {
            OpenProjectEditor(shutdownOnClose: true);
        }
    }

    public static void WriteResourceFileToFileSystem(string resourcePath, string targetPath)
    {
        using var res = Assembly.GetExecutingAssembly().GetManifestResourceStream(
            typeof(App).Namespace + "." + resourcePath.Replace("/", "."));
        using var trg = File.Open(targetPath, FileMode.Create, FileAccess.Write, FileShare.None);
        res.CopyTo(trg);
    }

    public void OpenProjectEditor(bool shutdownOnClose = false)
    {
        var window = ConfigEditorWindow.Open(
            string.Format(
                EditorTitle_1,
                ProjectLoader?.ProjectView?.Title ?? Unknown),
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
