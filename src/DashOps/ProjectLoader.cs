using Mastersign.DashOps.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                .WithNamingConvention(new HyphenatedNamingConvention())
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
            foreach (var actionDiscovery in Project.ActionDiscovery)
            {
                foreach(var actionView in DiscoverActions(actionDiscovery))
                {
                    ProjectView.ActionViews.Add(actionView);
                }
            }

            ProjectView.AddTagsPerspective();
            ProjectView.AddFacettePerspectives(DEF_PERSPECTIVES);
            ProjectView.AddFacettePerspectives(Project.Perspectives.ToArray());
        }

        private IEnumerable<ActionView> DiscoverActions(CommandActionDiscovery actionDiscovery)
        {
            var basePath = actionDiscovery.BasePath ?? string.Empty;
            basePath = string.IsNullOrWhiteSpace(basePath)
                ? Environment.CurrentDirectory
                : Path.IsPathRooted(basePath)
                    ? basePath
                    : Path.Combine(Environment.CurrentDirectory, basePath);
            var pathRegex = new Regex(actionDiscovery.PathRegex,
                RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            var groupNames = pathRegex.GetGroupNames();

            string lowerCase(string s) => char.ToLowerInvariant(s[0]) + s.Substring(1);
            string upperCase(string s) => char.ToUpperInvariant(s[0]) + s.Substring(1);

            string getGroupValue(Match m, string name, string def)
            {
                if (string.IsNullOrWhiteSpace(name)) return null;
                var name1 = lowerCase(name);
                if (groupNames.Contains(name1) && m.Groups[name1].Success)
                {
                    return m.Groups[name1].Value;
                }
                var name2 = upperCase(name);
                if (groupNames.Contains(name2) && m.Groups[name2].Success)
                {
                    return m.Groups[name2].Value;
                }
                return def;
            }

            string expandTemplate(string template, Dictionary<string, string> variables)
            {
                var result = template;
                foreach (var kvp in variables)
                {
                    result = result.Replace("${" + kvp.Key + "}", kvp.Value);
                    result = result.Replace("${" + kvp.Key.ToLowerInvariant() + "}", kvp.Value);
                }
                return result;
            }

            foreach (var file in Directory.EnumerateFiles(basePath, "*", SearchOption.AllDirectories))
            {
                Debug.Assert(file.StartsWith(basePath, StringComparison.OrdinalIgnoreCase));
                var relativePath = file.Substring(basePath.Length);
                var m = pathRegex.Match(relativePath);
                if (!m.Success) continue;
                var verbGroup = groupNames.Contains("verb")
                    ? m.Groups["verb"]
                    : groupNames.Contains("Verb")
                        ? m.Groups["Verb"]
                        : null;
                var facettes = actionDiscovery.Facettes != null
                    ? new Dictionary<string, string>(actionDiscovery.Facettes)
                    : new Dictionary<string, string>();
                var verb = getGroupValue(m, nameof(CommandAction.Verb), actionDiscovery.Verb);
                var service = getGroupValue(m, nameof(CommandAction.Service), actionDiscovery.Service);
                var host = getGroupValue(m, nameof(CommandAction.Host), actionDiscovery.Host);
                foreach (var groupName in groupNames)
                {
                    if (groupName == "0") continue;
                    if (groupName == lowerCase(nameof(CommandAction.Verb)) ||
                        groupName == upperCase(nameof(CommandAction.Verb)) ||
                        groupName == lowerCase(nameof(CommandAction.Service)) ||
                        groupName == upperCase(nameof(CommandAction.Service)) ||
                        groupName == lowerCase(nameof(CommandAction.Host)) ||
                        groupName == upperCase(nameof(CommandAction.Host))) continue;
                    var g = m.Groups[groupName];
                    if (!g.Success) continue;
                    facettes[groupName] = g.Value;
                }
                if (verb != null) facettes[nameof(CommandAction.Verb)] = verb;
                if (service != null) facettes[nameof(CommandAction.Service)] = service;
                if (host != null) facettes[nameof(CommandAction.Host)] = host;
                var tags = actionDiscovery.Tags;
                yield return new ActionView()
                {
                    Description = expandTemplate(actionDiscovery.DescriptionTemplate, facettes),
                    Command = file,
                    Arguments = actionDiscovery.Arguments,
                    Facettes = facettes,
                    Tags = tags
                };
            }
        }
    }
}
