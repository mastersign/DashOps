using Mastersign.DashOps.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Mastersign.DashOps
{
    public class ProjectLoader
    {
        private static readonly string[] SUPPORTED_VERSIONS = new[] { "1", "1.0" };

        private readonly string[] DEF_PERSPECTIVES = new[]
        {
            nameof(CommandAction.Verb),
            nameof(CommandAction.Service),
            nameof(CommandAction.Host),
        };

        public string ProjectPath { get; private set; }

        public Project Project { get; private set; }

        public ProjectView ProjectView { get; private set; }

        private Action<Action> Dispatcher { get; set; }

        public ProjectLoader(string projectPath, Action<Action> dispatcher)
        {
            if (projectPath == null) throw new ArgumentNullException(nameof(projectPath));
            Dispatcher = dispatcher;
            if (!File.Exists(projectPath))
            {
                throw new FileNotFoundException("Project file not found.", projectPath);
            }
            ProjectPath = Path.IsPathRooted(projectPath) ? projectPath : Path.Combine(Environment.CurrentDirectory, projectPath);

            var watcher = new FileSystemWatcher(Path.GetDirectoryName(ProjectPath), Path.GetFileName(ProjectPath));
            watcher.Changed += ProjectFileChangedHandler;
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;

            ProjectView = new ProjectView();
            InitializeDeserialization();
            LoadProject();
            UpdateProjectView();
            watcher.EnableRaisingEvents = true;
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

        public void ReloadProjectAndProjectView()
        {
            var selectedPerspective = ProjectView.CurrentPerspective?.Title;
            var selectedSubset = ProjectView.CurrentPerspective?.CurrentSubset?.Title;
            LoadProject();
            UpdateProjectView();
            if (selectedPerspective != null)
            {
                ProjectView.CurrentPerspective =
                    ProjectView.Perspectives.FirstOrDefault(p => p.Title == selectedPerspective);
                if (selectedSubset != null && ProjectView.CurrentPerspective != null)
                {
                    ProjectView.CurrentPerspective.CurrentSubset =
                        ProjectView.CurrentPerspective.Subsets.FirstOrDefault(s => s.Title == selectedSubset);
                }
            }
        }

        private Deserializer _deserializer;

        private void InitializeDeserialization()
        {
            _deserializer = new DeserializerBuilder()
                .WithNamingConvention(new HyphenatedNamingConvention())
                .Build();
        }

        private static readonly Regex VersionPattern = new Regex(@"^version\:\s+('|"")(?<version>.*?)\1\s*$");

        private static string FindVersionString(TextReader r)
        {
            var line = r.ReadLine();
            while (line != null)
            {
                var m = VersionPattern.Match(line);
                if (m.Success) return m.Groups["version"].Value;
                line = r.ReadLine();
            }
            return null;
        }

        private static void CheckVersionSupport(Stream s, out string version)
        {
            using (var r = new StreamReader(s, Encoding.UTF8, false, 1024, true))
            {
                version = FindVersionString(r);
            }
            if (version == null) throw new FormatException("No version attribute found.");
            if (!SUPPORTED_VERSIONS.Contains(version)) throw new FormatException("Version not supported.");
            s.Seek(0, SeekOrigin.Begin);
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
                CheckVersionSupport(s, out var version);
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

            void AddActionViews(IEnumerable<ActionView> actionViews)
            {
                foreach (var actionView in actionViews) ProjectView.ActionViews.Add(actionView);
            }
            if (Project.Actions != null) AddActionViews(Project.Actions.Select(ActionViewFromCommandAction));
            if (Project.ActionDiscovery != null) AddActionViews(Project.ActionDiscovery.SelectMany(DiscoverActions));
            if (Project.ActionPatterns != null) AddActionViews(Project.ActionPatterns.SelectMany(ExpandActionPattern));

            ApplyAutoAnnotations();
            foreach (var actionView in ProjectView.ActionViews)
            {
                if (actionView.Logs == null) actionView.Logs = ProjectView.Logs;
            }

            ProjectView.AddTagsPerspective();
            ProjectView.AddFacettePerspectives(DEF_PERSPECTIVES);
            ProjectView.AddFacettePerspectives(Project.Perspectives.ToArray());
        }

        private void ApplyAutoAnnotations()
        {
            if (Project.Auto == null) return;
            foreach (var actionView in ProjectView.ActionViews)
            {
                foreach (var annotation in Project.Auto.Where(a => a.Match(actionView)))
                {
                    annotation.Apply(actionView);
                }
            }
        }

        private ActionView ActionViewFromCommandAction(CommandAction action)
        {
            var wdPath = action.WorkingDirectory?.TrimEnd(
                Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) ?? string.Empty;
            wdPath = string.IsNullOrWhiteSpace(wdPath)
                ? Environment.CurrentDirectory
                : Path.IsPathRooted(wdPath)
                    ? wdPath
                    : Path.Combine(Environment.CurrentDirectory, wdPath);

            var actionView = new ActionView
            {
                Command = action.Command,
                Arguments = action.Arguments,
                WorkingDirectory = wdPath,
                Description = action.Description,
                Reassure = action.Reassure,
                Logs = action.Logs,
                Tags = action.Tags ?? Array.Empty<string>(),
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

        private IEnumerable<ActionView> DiscoverActions(CommandActionDiscovery actionDiscovery)
        {
            var basePath = actionDiscovery.BasePath?.TrimEnd(
                Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) ?? string.Empty;
            basePath = string.IsNullOrWhiteSpace(basePath)
                ? Environment.CurrentDirectory
                : Path.IsPathRooted(basePath)
                    ? basePath
                    : Path.Combine(Environment.CurrentDirectory, basePath);

            if (!Directory.Exists(basePath)) yield break;
            if (string.IsNullOrWhiteSpace(actionDiscovery.PathPattern)) yield break;

            Regex pathRegex;
            try
            {
                pathRegex = new Regex(actionDiscovery.PathPattern,
                    RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            }
            catch (ArgumentException exc)
            {
                MessageBox.Show("Error in regular expression: " + exc.Message,
                    "Parsing Regular Expression",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                yield break;
            }

            var groupNames = pathRegex.GetGroupNames();

            foreach (var file in Directory.EnumerateFiles(basePath, "*", SearchOption.AllDirectories))
            {
                Debug.Assert(file.StartsWith(basePath, StringComparison.OrdinalIgnoreCase));
                var relativePath = file.Substring(basePath.Length + 1);
                var m = pathRegex.Match(relativePath);
                if (!m.Success) continue;
                yield return ActionViewFromDiscoveredMatch(actionDiscovery, groupNames, m, file);
            }
        }

        private IEnumerable<ActionView> ExpandActionPattern(CommandActionPattern actionPattern)
        {
            var facettes = actionPattern.Facettes != null
                ? new Dictionary<string, string[]>(actionPattern.Facettes)
                : new Dictionary<string, string[]>();

            void AddFacetteValues(string key, string[] values)
            {
                if (values == null || values.Length == 0) return;
                if (facettes.TryGetValue(key, out string[] oldValues))
                    facettes[key] = oldValues.Concat(values).Distinct().ToArray();
                else
                    facettes[key] = values;
            }
            AddFacetteValues(nameof(CommandAction.Verb), actionPattern.Verb);
            AddFacetteValues(nameof(CommandAction.Service), actionPattern.Service);
            AddFacetteValues(nameof(CommandAction.Host), actionPattern.Host);

            var facetteKeys = facettes.Keys;
            var facette = new Dictionary<string, string>();

            IEnumerable<Dictionary<string, string>> AddDimension(IEnumerable<Dictionary<string, string>> values, string key)
            {
                return values.SelectMany(d => facettes[key], (d2, v) => new Dictionary<string, string>(d2) { { key, v } });
            }

            return facetteKeys.Aggregate(Enumerable.Repeat(new Dictionary<string, string>(), 1),
                AddDimension).Select(d => ActionViewFromPatternVariation(actionPattern, d));
        }

        private ActionView ActionViewFromDiscoveredMatch(
            CommandActionDiscovery actionDiscovery,
            string[] groupNames, Match m, string file)
        {
            var facettes = actionDiscovery.Facettes != null
                ? new Dictionary<string, string>(actionDiscovery.Facettes)
                : new Dictionary<string, string>();
            var verb = GetGroupValue(groupNames, m, nameof(CommandAction.Verb), actionDiscovery.Verb);
            var service = GetGroupValue(groupNames, m, nameof(CommandAction.Service), actionDiscovery.Service);
            var host = GetGroupValue(groupNames, m, nameof(CommandAction.Host), actionDiscovery.Host);
            foreach (var groupName in groupNames)
            {
                if (groupName == "0") continue;
                if (groupName == LowerCase(nameof(CommandAction.Verb)) ||
                    groupName == UpperCase(nameof(CommandAction.Verb)) ||
                    groupName == LowerCase(nameof(CommandAction.Service)) ||
                    groupName == UpperCase(nameof(CommandAction.Service)) ||
                    groupName == LowerCase(nameof(CommandAction.Host)) ||
                    groupName == UpperCase(nameof(CommandAction.Host))) continue;
                var g = m.Groups[groupName];
                if (!g.Success) continue;
                facettes[groupName] = g.Value;
            }
            if (verb != null) facettes[nameof(CommandAction.Verb)] = verb;
            if (service != null) facettes[nameof(CommandAction.Service)] = service;
            if (host != null) facettes[nameof(CommandAction.Host)] = host;

            var wdPath = actionDiscovery.WorkingDirectory?.TrimEnd(
                Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) ?? string.Empty;
            wdPath = ExpandTemplate(wdPath, facettes);
            wdPath = string.IsNullOrWhiteSpace(wdPath)
                ? Environment.CurrentDirectory
                : Path.IsPathRooted(wdPath)
                    ? wdPath
                    : Path.Combine(Environment.CurrentDirectory, wdPath);

            return new ActionView()
            {
                Description = ExpandTemplate(actionDiscovery.Description, facettes),
                Reassure = actionDiscovery.Reassure,
                Logs = ExpandTemplate(actionDiscovery.Logs, facettes),
                Command = file,
                Arguments = actionDiscovery.Arguments,
                WorkingDirectory = wdPath,
                Facettes = facettes,
                Tags = actionDiscovery.Tags ?? Array.Empty<string>()
            };
        }

        private static ActionView ActionViewFromPatternVariation(CommandActionPattern actionPattern,
            Dictionary<string, string> facettes)
        {
            var wdPath = actionPattern.WorkingDirectory?.TrimEnd(
                Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) ?? string.Empty;
            wdPath = ExpandTemplate(wdPath, facettes);
            wdPath = string.IsNullOrWhiteSpace(wdPath)
                ? Environment.CurrentDirectory
                : Path.IsPathRooted(wdPath)
                    ? wdPath
                    : Path.Combine(Environment.CurrentDirectory, wdPath);

            return new ActionView()
            {
                Description = ExpandTemplate(actionPattern.Description, facettes),
                Reassure = actionPattern.Reassure,
                Logs = ExpandTemplate(actionPattern.Logs, facettes),
                Command = ExpandTemplate(actionPattern.Command, facettes),
                Arguments = actionPattern.Arguments.Select(a => ExpandTemplate(a, facettes)).ToArray(),
                WorkingDirectory = wdPath,
                Facettes = facettes,
                Tags = actionPattern.Tags ?? Array.Empty<string>()
            };
        }

        private static string ExpandTemplate(string template, Dictionary<string, string> variables)
        {
            if (string.IsNullOrWhiteSpace(template)) return template;
            var result = template;
            foreach (var kvp in variables)
            {
                result = result.Replace("${" + kvp.Key + "}", kvp.Value);
                result = result.Replace("${" + kvp.Key.ToLowerInvariant() + "}", kvp.Value);
            }
            return result;
        }

        private static string GetGroupValue(string[] groupNames, Match m, string name, string def)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            var name1 = LowerCase(name);
            if (groupNames.Contains(name1) && m.Groups[name1].Success)
            {
                return m.Groups[name1].Value;
            }
            var name2 = UpperCase(name);
            if (groupNames.Contains(name2) && m.Groups[name2].Success)
            {
                return m.Groups[name2].Value;
            }
            return def;
        }

        private static string LowerCase(string s) => char.ToLowerInvariant(s[0]) + s.Substring(1);

        private static string UpperCase(string s) => char.ToUpperInvariant(s[0]) + s.Substring(1);
    }
}
