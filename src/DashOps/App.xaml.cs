using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wpf.Ui.Appearance;
using static Mastersign.DashOps.UserInteraction;

namespace Mastersign.DashOps
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        public static App Instance => Current as App;

        public IProjectLoader ProjectLoader { get; private set; }

        public Executor Executor { get; private set; }

        public MonitorManager MonitorManager { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ApplicationThemeManager.ApplySystemTheme();

            string projectFile;
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
                if (File.Exists(name1))
                    projectFile = name1;
                else if (File.Exists(name2))
                    projectFile = name2;
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
            ProjectLoader = ProjectLoaderFactory.CreateProjectLoaderFor(projectFile, Dispatch, out string version);
            if (ProjectLoader is null)
            {
                ShowMessage(
                    "Loading DashOps Project File",
                    "The format of the project file is not supported."
                    + Environment.NewLine
                    + Environment.NewLine
                    + $"Application Version: {GetAppVersion()}"
                    + Environment.NewLine
                    + $"File Version: {version ?? "unknown"}",
                    symbol: InteractionSymbol.Error,
                    showInTaskbar: true);
                Shutdown(1);
                return;
            }
            Executor = new Executor();
            MonitorManager = new MonitorManager();
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
}
