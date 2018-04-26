using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Mastersign.DashOps
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        public ProjectLoader ProjectLoader { get; private set; }

        public Executor Executor { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string projectFile;
            if (e.Args.Length == 1)
            {
                if (File.Exists(e.Args[0]))
                    projectFile = e.Args[0];
                else
                {
                    MessageBox.Show(
                        $"The project file '{e.Args[0]}' could not be found.",
                        "Loading DashOps Project File",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
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
                    MessageBox.Show(
                        $"No project file given as command line argument and no default project file in the current working directory: {name1}",
                        "Loading DashOps Project File",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    Shutdown(1);
                    return;
                }
            }
            try
            {
                ProjectLoader = new ProjectLoader(projectFile, Dispatch);
            }
            catch (Exception exc)
            {
                var msg = string.Empty;
#if DEBUG
                msg += exc.ToString();
#else
                msg += exc.Message;
#endif
                while (exc.InnerException != null)
                {
                    exc = exc.InnerException;
#if DEBUG
                    msg += Environment.NewLine + exc.ToString();
#else
                    msg += Environment.NewLine + exc.Message;
#endif
                }
                MessageBox.Show(msg,
                    "Loading DashOps Project File",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                Shutdown(1);
                return;
            }
            Executor = new Executor();
        }

        private void Dispatch(Action action)
        {
            Dispatcher.BeginInvoke(action);
        }
    }
}
