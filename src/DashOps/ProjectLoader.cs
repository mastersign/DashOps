using Mastersign.DashOps.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Mastersign.DashOps
{
    public class ProjectLoader
    {
        private readonly string[] DEF_PERSPECTIVES = new[]
        {
            nameof(CommandAction.Verb),
            nameof(CommandAction.Service),
            nameof(CommandAction.Host),
        };

        public string ProjectPath { get; private set; }

        public Project Project { get; private set; }

        public ProjectView ProjectView { get; private set; }

        private FileSystemWatcher _watcher;

        private Action<Action> Dispatcher { get; set; }

        public ProjectLoader(string projectPath, Action<Action> dispatcher)
        {
            Dispatcher = dispatcher;
            if (!File.Exists(projectPath))
            {
                throw new FileNotFoundException("Project file not found.", projectPath);
            }
            ProjectPath = Path.IsPathRooted(projectPath) ? projectPath : Path.Combine(Environment.CurrentDirectory, projectPath);
            System.Windows.MessageBox.Show(ProjectPath);
            _watcher = new FileSystemWatcher(Path.GetDirectoryName(ProjectPath), Path.GetFileName(ProjectPath));
            _watcher.Changed += ProjectFileChangedHandler;
            _watcher.Created += ProjectFileChangedHandler;
            _watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;
            ProjectView = new ProjectView();
            InitializeDeserialization();
            LoadProject();
            UpdateProjectView();
            _watcher.EnableRaisingEvents = true;
        }

        private void ProjectFileChangedHandler(object sender, FileSystemEventArgs e)
        {
            if (Dispatcher != null)
            {
                Dispatcher(ReloadProjectAndProjectView);
            }
            else
            {
                ReloadProjectAndProjectView();
            }
        }

        private void ReloadProjectAndProjectView()
        {
            LoadProject();
            UpdateProjectView();
        }

        private Deserializer _deserializer;

        private void InitializeDeserialization()
        {
            _deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();
        }

        private void LoadProject()
        {
            Stream s = null;
            var w = new Stopwatch();
            w.Start();
            Exception exc = null;
            while (s == null && w.ElapsedMilliseconds < 2000)
            {
                try
                {
                    s = File.Open(ProjectPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    exc = null;
                }
                catch (IOException ioe)
                {
                    exc = ioe;
                }
            }
            w.Stop();
            if (exc != null) throw exc;
            try
            {
                using (var r = new StreamReader(s, Encoding.UTF8))
                {
                    Project = _deserializer.Deserialize<Project>(r);
                }
            }
            finally
            {
                s.Dispose();
            }
        }

        private ActionView ActionViewFromCommandAction(CommandAction action)
        {
            var actionView = new ActionView
            {
                Command = action.Command,
                Arguments = action.Arguments,
                Description = action.Description,
                Tags = action.Tags,
                Facettes = action.Facettes != null
                    ? new Dictionary<string, string>(action.Facettes)
                    : new Dictionary<string, string>(),
            };
            if (!string.IsNullOrWhiteSpace(action.Verb))
            {
                actionView.Facettes[nameof(CommandAction.Verb)] = action.Verb;
            }
            if (!string.IsNullOrWhiteSpace(action.Service))
            {
                actionView.Facettes[nameof(CommandAction.Service)] = action.Service;
            }
            if (!string.IsNullOrWhiteSpace(action.Host))
            {
                actionView.Facettes[nameof(CommandAction.Host)] = action.Host;
            }
            return actionView;
        }

        private void UpdateProjectView()
        {
            ProjectView.ActionViews.Clear();
            ProjectView.Perspectives.Clear();
            ProjectView.Title = Project?.Title ?? "Unknown";
            ProjectView.Logs = Project?.Logs;
            if (Project == null) return;

            if (ProjectView.Logs != null)
            {
                if (!Path.IsPathRooted(ProjectView.Logs))
                {
                    ProjectView.Logs = Path.Combine(Environment.CurrentDirectory, ProjectView.Logs);
                }
                if (!Directory.Exists(ProjectView.Logs))
                {
                    Directory.CreateDirectory(ProjectView.Logs);
                }
            }

            foreach (var action in Project.Actions)
            {
                ProjectView.ActionViews.Add(ActionViewFromCommandAction(action));
            }

            ProjectView.AddTagsPerspective();
            ProjectView.AddFacettePerspectives(DEF_PERSPECTIVES);
            ProjectView.AddFacettePerspectives(Project.Perspectives.ToArray());
        }
    }
}
