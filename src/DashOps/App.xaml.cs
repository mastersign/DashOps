using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
            if (e.Args.Length < 1)
            {
                MessageBox.Show("No project file given. Pass a path to a project file as command line argument.");
                Shutdown(1);
            }
            if (e.Args.Length > 1)
            {
                MessageBox.Show("Only one command line argument is allowed.");
                Shutdown(1);
            }
            try
            {
                ProjectLoader = new ProjectLoader(e.Args[0], Dispatch);
            }
            catch (Exception exc)
            {
                var msg = exc.Message;
                while (exc.InnerException != null)
                {
                    exc = exc.InnerException;
                    msg += Environment.NewLine + exc.Message;
                }
                MessageBox.Show(msg);
                Shutdown(1);
            }
            Executor = new Executor();
        }

        private void Dispatch(Action action)
        {
            Dispatcher.BeginInvoke(action);
        }
    }
}
