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

        public string ProjectPath { get;  }

        public Project Project { get; private set; }

        public ProjectView ProjectView { get;  }

        private Action<Action> Dispatcher { get; }

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
            string version = null;
            try
            {
                if (exc != null) throw exc;
                CheckVersionSupport(s, out version);
                using (var r = new StreamReader(s, Encoding.UTF8))
                {
                    Project = _deserializer.Deserialize<Project>(r);
                }
            }
            catch (Exception exc2)
            {
                var msg = string.Empty;
#if DEBUG
                msg += exc2.ToString();
#else
                msg += exc2.Message;
#endif
                while (exc2.InnerException != null)
                {
                    exc2 = exc2.InnerException;
#if DEBUG
                    msg += Environment.NewLine + exc2.ToString();
#else
                    msg += Environment.NewLine + exc2.Message;
#endif
                }
                MessageBox.Show(msg,
                    "Loading DashOps Project File" + (version != null ? " v" + version : ""),
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void UpdateProjectView()
        {
            ProjectView.ActionViews.Clear();
            ProjectView.Perspectives.Clear();
            ProjectView.MonitorViews.Clear();
            ProjectView.Title = Project?.Title ?? "Unknown";
            var defaultLogDir = ExpandEnv(Project?.Logs);
            if (Project == null) return;

            if (defaultLogDir != null)
            {
                if (!Path.IsPathRooted(defaultLogDir))
                {
                    defaultLogDir = Path.Combine(Environment.CurrentDirectory, defaultLogDir);
                }
                if (!Directory.Exists(defaultLogDir))
                {
                    Directory.CreateDirectory(defaultLogDir);
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
                if (actionView.Logs == null) actionView.Logs = defaultLogDir;
                if (Project.NoLogs || actionView.NoLogs) actionView.Logs = null;
            }

            ProjectView.AddTagsPerspective();
            ProjectView.AddFacettePerspectives(DEF_PERSPECTIVES);
            ProjectView.AddFacettePerspectives(Project.Perspectives.ToArray());

            ProjectView.DefaultMonitorInterval = new TimeSpan(0, 0, Project.DefaultMonitorInterval);
            void AddMonitorViews(IEnumerable<MonitorView> monitorViews)
            {
                foreach (var monitorView in monitorViews) ProjectView.MonitorViews.Add(monitorView);
            }
            if (Project.Monitors != null) AddMonitorViews(Project.Monitors.Select(MonitorViewFromCommandMonitor));
            if (Project.MonitorDiscovery != null) AddMonitorViews(Project.MonitorDiscovery.SelectMany(DiscoverMonitors));
            if (Project.MonitorPatterns != null) AddMonitorViews(Project.MonitorPatterns.SelectMany(ExpandCommandMonitorPattern));
            if (Project.WebMonitors != null) AddMonitorViews(Project.WebMonitors.Select(MonitorViewFromWebMonitor));
            if (Project.WebMonitorPatterns != null) AddMonitorViews(Project.WebMonitorPatterns.SelectMany(ExpandWebMonitorPattern));
            foreach (var monitorView in ProjectView.MonitorViews)
            {
                if (monitorView.Logs == null) monitorView.Logs = Project.Logs;
                if (Project.NoLogs || monitorView.NoLogs) monitorView.Logs = null;
                var logInfo = monitorView.GetLastLogFileInfo();
                if (logInfo != null)
                {
                    monitorView.LastExecutionResult = logInfo.IsSuccess;
                    monitorView.HasLastExecutionResult = true;
                }
            }
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
            var actionView = new ActionView
            {
                Command = ExpandEnv(action.Command),
                Arguments = FormatArguments(action.Arguments),
                WorkingDirectory = BuildAbsolutePath(action.WorkingDirectory),
                Title = action.Description,
                Reassure = action.Reassure,
                Visible = !action.Background,
                Logs = ExpandEnv(action.Logs),
                NoLogs = action.NoLogs,
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
            var basePath = BuildAbsolutePath(actionDiscovery.BasePath);
            if (!Directory.Exists(basePath)) yield break;
            var pathRegex = BuildRegexFromPattern(actionDiscovery.PathPattern,
                RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            if (pathRegex == null) yield break;

            var groupNames = pathRegex.GetGroupNames();
            foreach (var discovery in DiscoverFiles(basePath, pathRegex))
            {
                var file = discovery.Item1;
                var match = discovery.Item2;
                yield return ActionViewFromDiscoveredMatch(actionDiscovery, groupNames, match, file);
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

            return EnumerateVariations(facettes)
                .Select(d => ActionViewFromPatternVariation(actionPattern, d));
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

            return new ActionView()
            {
                Title = ExpandTemplate(actionDiscovery.Description, facettes),
                Reassure = actionDiscovery.Reassure,
                Visible = !actionDiscovery.Background,
                Logs = ExpandEnv(ExpandTemplate(actionDiscovery.Logs, facettes)),
                NoLogs = actionDiscovery.NoLogs,
                Command = file,
                Arguments = FormatArguments(actionDiscovery.Arguments),
                WorkingDirectory = BuildAbsolutePath(
                    ExpandTemplate(actionDiscovery.WorkingDirectory, facettes)),
                Facettes = facettes,
                Tags = actionDiscovery.Tags ?? Array.Empty<string>()
            };
        }

        private static ActionView ActionViewFromPatternVariation(
            CommandActionPattern actionPattern, Dictionary<string, string> facettes)
            => new ActionView()
            {
                Title = ExpandTemplate(actionPattern.Description, facettes),
                Reassure = actionPattern.Reassure,
                Visible = !actionPattern.Background,
                Logs = ExpandEnv(ExpandTemplate(actionPattern.Logs, facettes)),
                NoLogs = actionPattern.NoLogs,
                Command = ExpandEnv(ExpandTemplate(actionPattern.Command, facettes)),
                Arguments = FormatArguments(
                    actionPattern.Arguments?.Select(a => ExpandTemplate(a, facettes))),
                WorkingDirectory = BuildAbsolutePath(
                    ExpandTemplate(actionPattern.WorkingDirectory, facettes)),
                Facettes = facettes,
                Tags = actionPattern.Tags ?? Array.Empty<string>()
            };

        private MonitorView MonitorViewFromCommandMonitor(CommandMonitor monitor)
            => new CommandMonitorView
            {
                Title = monitor.Title,
                Logs = ExpandEnv(monitor.Logs),
                NoLogs = monitor.NoLogs,
                Interval = new TimeSpan(0, 0, monitor.Interval),
                Command = ExpandEnv(monitor.Command),
                Arguments = FormatArguments(monitor.Arguments),
                WorkingDirectory = BuildAbsolutePath(monitor.WorkingDirectory),
                RequiredPatterns = BuildPatterns(monitor.RequiredPatterns),
                ForbiddenPatterns = BuildPatterns(monitor.ForbiddenPatterns)
            };

        private static IEnumerable<MonitorView> DiscoverMonitors(CommandMonitorDiscovery monitorDiscovery)
        {
            var basePath = BuildAbsolutePath(monitorDiscovery.BasePath);
            if (!Directory.Exists(basePath)) yield break;
            var pathRegex = BuildRegexFromPattern(monitorDiscovery.PathPattern,
                RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            if (pathRegex == null) yield break;

            var groupNames = pathRegex.GetGroupNames();
            foreach (var discovery in DiscoverFiles(basePath, pathRegex))
            {
                var file = discovery.Item1;
                var match = discovery.Item2;
                yield return MonitorViewFromDiscoveredMatch(monitorDiscovery, groupNames, match, file);
            }
        }

        private static IEnumerable<MonitorView> ExpandCommandMonitorPattern(CommandMonitorPattern monitorPattern)
            => monitorPattern.Variables != null
                ? EnumerateVariations(monitorPattern.Variables)
                    .Select(d => MonitorViewFromPatternVariation(monitorPattern, d))
                : Enumerable.Empty<MonitorView>();

        private static MonitorView MonitorViewFromDiscoveredMatch(CommandMonitorDiscovery monitorDiscovery,
            string[] groupNames, Match m, string file)
        {
            var variables = new Dictionary<string, string>();
            foreach (var groupName in groupNames)
            {
                if (groupName == "0") continue;
                var g = m.Groups[groupName];
                if (!g.Success) continue;
                variables[groupName] = g.Value;
            }

            return new CommandMonitorView
            {
                Title = ExpandTemplate(monitorDiscovery.Title, variables),
                Logs = ExpandEnv(ExpandTemplate(monitorDiscovery.Logs, variables)),
                NoLogs = monitorDiscovery.NoLogs,
                Interval = new TimeSpan(0, 0, monitorDiscovery.Interval),
                Command = file,
                Arguments = FormatArguments(
                    monitorDiscovery.Arguments?.Select(a => ExpandTemplate(a, variables))),
                WorkingDirectory = BuildAbsolutePath(monitorDiscovery.WorkingDirectory),
                RequiredPatterns = BuildPatterns(monitorDiscovery.RequiredPatterns),
                ForbiddenPatterns = BuildPatterns(monitorDiscovery.ForbiddenPatterns)
            };
        }

        private static MonitorView MonitorViewFromPatternVariation(
            CommandMonitorPattern monitorPattern, Dictionary<string, string> variables)
            => new CommandMonitorView
            {
                Title = ExpandTemplate(monitorPattern.Title, variables),
                Logs = ExpandEnv(ExpandTemplate(monitorPattern.Logs, variables)),
                NoLogs = monitorPattern.NoLogs,
                Interval = new TimeSpan(0, 0, monitorPattern.Interval),
                Command = ExpandEnv(ExpandTemplate(monitorPattern.Command, variables)),
                Arguments = FormatArguments(
                    monitorPattern.Arguments?.Select(a => ExpandTemplate(a, variables))),
                WorkingDirectory = BuildAbsolutePath(monitorPattern.WorkingDirectory),
                RequiredPatterns = BuildPatterns(monitorPattern.RequiredPatterns),
                ForbiddenPatterns = BuildPatterns(monitorPattern.ForbiddenPatterns)
            };

        private static MonitorView MonitorViewFromWebMonitor(WebMonitor monitor)
            => new WebMonitorView
            {
                Title = monitor.Title,
                Logs = ExpandEnv(monitor.Logs),
                NoLogs = monitor.NoLogs,
                Interval = new TimeSpan(0, 0, monitor.Interval),
                Url = monitor.Url,
                Headers = monitor.Headers,
                StatusCodes = monitor.StatusCodes != null && monitor.StatusCodes.Length > 0
                    ? monitor.StatusCodes
                    : new[] { 200, 201, 202, 203, 204 },
                RequiredPatterns = BuildPatterns(monitor.RequiredPatterns),
                ForbiddenPatterns = BuildPatterns(monitor.ForbiddenPatterns),
            };

        private static IEnumerable<MonitorView> ExpandWebMonitorPattern(WebMonitorPattern monitorPattern)
            => monitorPattern.Variables != null
                ? EnumerateVariations(monitorPattern.Variables)
                    .Select(d => MonitorViewFromPatternVariation(monitorPattern, d))
                : Enumerable.Empty<MonitorView>();

        private static MonitorView MonitorViewFromPatternVariation(
            WebMonitorPattern monitorPattern, Dictionary<string, string> variables)
            => new WebMonitorView
            {
                Title = ExpandTemplate(monitorPattern.Title, variables),
                Logs = ExpandEnv(ExpandTemplate(monitorPattern.Logs, variables)),
                NoLogs = monitorPattern.NoLogs,
                Interval = new TimeSpan(0, 0, monitorPattern.Interval),
                Url = ExpandTemplate(monitorPattern.Url, variables),
                Headers = ExpandDictionaryTemplate(monitorPattern.Headers, variables),
                StatusCodes = monitorPattern.StatusCodes != null && monitorPattern.StatusCodes.Length > 0
                    ? monitorPattern.StatusCodes
                    : new[] { 200, 201, 202, 203, 204 },
                RequiredPatterns = BuildPatterns(monitorPattern.RequiredPatterns),
                ForbiddenPatterns = BuildPatterns(monitorPattern.ForbiddenPatterns)
            };

        private static string BuildAbsolutePath(string dir)
        {
            dir = ExpandEnv(dir);
            dir = dir?.TrimEnd(
                Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) ?? string.Empty;
            return string.IsNullOrWhiteSpace(dir)
                ? Environment.CurrentDirectory
                : Path.IsPathRooted(dir)
                    ? dir
                    : Path.Combine(Environment.CurrentDirectory, dir);
        }

        private static Regex BuildRegexFromPattern(string pattern, RegexOptions options)
        {
            if (string.IsNullOrWhiteSpace(pattern)) return null;
            try
            {
                return new Regex(pattern,
                    RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            }
            catch (ArgumentException exc)
            {
                MessageBox.Show("Error in regular expression: " + exc.Message,
                    "Parsing Regular Expression",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
        }

        private static IEnumerable<Tuple<string, Match>> DiscoverFiles(string basePath, Regex pattern)
        {
            foreach (var file in Directory.EnumerateFiles(basePath, "*", SearchOption.AllDirectories))
            {
                Debug.Assert(file.StartsWith(basePath, StringComparison.OrdinalIgnoreCase));
                var relativePath = file.Substring(basePath.Length + 1);
                var m = pattern.Match(relativePath);
                if (!m.Success) continue;
                yield return Tuple.Create(file, m);
            }
        }

        private static Regex[] BuildPatterns(string[] patterns)
        {
            try
            {
                return patterns?.Select(p => new Regex(p,
                        RegexOptions.Compiled | RegexOptions.CultureInvariant,
                        TimeSpan.FromMilliseconds(1000))
                    ).ToArray() ?? Array.Empty<Regex>();
            }
            catch (ArgumentException exc)
            {
                MessageBox.Show("Error in regular expression: " + exc.Message,
                    "Parsing Regular Expression",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return Array.Empty<Regex>();
            }
        }

        private static IEnumerable<Dictionary<string, string>> EnumerateVariations(Dictionary<string, string[]> dimensions)
        {
            IEnumerable<Dictionary<string, string>> AddDimension(IEnumerable<Dictionary<string, string>> values, string key)
            {
                return values.SelectMany(d => dimensions[key], (d2, v) => new Dictionary<string, string>(d2) { { key, v } });
            }
            return dimensions.Keys.Aggregate(Enumerable.Repeat(new Dictionary<string, string>(), 1), AddDimension);
        }

        private static Dictionary<string, string> ExpandDictionaryTemplate(Dictionary<string, string> dict, Dictionary<string, string> variables)
            => MapValues(dict, v => ExpandTemplate(v, variables));

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

        private static string ExpandEnv(string template)
            => string.IsNullOrWhiteSpace(template)
                ? template
                : Environment.ExpandEnvironmentVariables(template);

        private static string FormatArguments(IEnumerable<string> arguments)
            => arguments != null
                ? CommandLine.FormatArgumentList(
                    arguments.Select(ExpandEnv).ToArray())
                : null;

        private static Dictionary<TKey, TValue> MapValues<TKey, TValue>(
            Dictionary<TKey, TValue> dict, Func<TValue, TValue> f)
        {
            if (dict == null) return new Dictionary<TKey, TValue>();
            var result = new Dictionary<TKey, TValue>(dict);
            foreach (var kvp in result)
            {
                result[kvp.Key] = f(kvp.Value);
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
