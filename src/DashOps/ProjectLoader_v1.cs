using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Mastersign.DashOps.Model_v1;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Mastersign.DashOps
{
    public class ProjectLoader_v1 : IProjectLoader
    {
        private static readonly string[] SUPPORTED_VERSIONS = ["1", "1.0", "1.1", "1.2", "1.3"];

        private readonly string[] DEF_PERSPECTIVES =
        [
            nameof(CommandAction.Verb),
            nameof(CommandAction.Service),
            nameof(CommandAction.Host),
        ];

        public string ProjectPath { get; }

        public Project Project { get; private set; }

        public ProjectView ProjectView { get; }

        private Action<Action> Dispatcher { get; }

        public ProjectLoader_v1(string projectPath, Action<Action> dispatcher)
        {
            if (projectPath == null) throw new ArgumentNullException(nameof(projectPath));
            if (!Path.IsPathRooted(projectPath)) throw new ArgumentException("Project path is not rooted", nameof(projectPath));
            Dispatcher = dispatcher;
            ProjectPath = projectPath;

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

        private IDeserializer _deserializer;

        private void InitializeDeserialization()
        {
            _deserializer = new DeserializerBuilder()
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .Build();
        }

        public static bool IsCompatible(Stream s, out string version)
            => ProjectVersionDetection.IsVersionSupported(s, SUPPORTED_VERSIONS, out version);

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
                if (!IsCompatible(s, out version))
                {
                    throw new FormatException("Project version is not supported.");
                }
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
                UserInteraction.ShowMessage(
                    "Loading DashOps Project File" + (version != null ? " - Format " + version : ""),
                    "An error occurred while loading the project file:"
                    + Environment.NewLine
                    + Environment.NewLine
                    + msg,
                    symbol: InteractionSymbol.Error);
            }
        }

        private void UpdateProjectView()
        {
            ProjectView.ActionViews.Clear();
            foreach (var p in ProjectView.Perspectives) { p.Dispose(); }
            ProjectView.Perspectives.Clear();
            ProjectView.MonitorViews.Clear();
            ProjectView.FormatVersion = Project?.Version ?? "0.0";
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
                actionView.Logs = BuildLogDirPath(actionView.Logs, actionView.NoLogs);
                if (Project.KeepActionOpen) actionView.KeepOpen = true;
                if (Project.AlwaysCloseAction) actionView.AlwaysClose = true;
            }

            ProjectView.IsMonitoringPaused = Project.PauseMonitors;
            var defaultMonitorInterval = new TimeSpan(0, 0, Project.DefaultMonitorInterval);
            var defaultWebMonitorTimeout = new TimeSpan(0, 0, Project.DefaultWebMonitorTimeout);
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
                monitorView.Logs = BuildLogDirPath(monitorView.Logs, monitorView.NoLogs);
                var logInfo = monitorView.GetLastLogFileInfo();
                if (logInfo != null && logInfo.HasResult)
                {
                    monitorView.LastExecutionResult = logInfo.Success;
                    monitorView.HasLastExecutionResult = true;
                }

                if (monitorView.Interval < TimeSpan.Zero) monitorView.Interval = defaultMonitorInterval;
                if (monitorView is WebMonitorView webMonitorView)
                {
                    if (webMonitorView.Timeout < TimeSpan.Zero) webMonitorView.Timeout = defaultWebMonitorTimeout;
                }
            }

            ProjectView.AddTagsPerspective();
            foreach (var facet in DEF_PERSPECTIVES)
            {
                ProjectView.AddFacetPerspective(facet);
            }
            foreach (var facet in Project.Perspectives)
            {
                ProjectView.AddFacetPerspective(facet);
            }
        }

        private string BuildLogDirPath(string logs, bool noLogs)
        {
            if (Project.NoLogs || noLogs) return null;
            logs = logs ?? Project.Logs;
            if (logs != null && !Path.IsPathRooted(logs))
            {
                logs = Path.Combine(Environment.CurrentDirectory, logs);
            }
            return logs;
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
            var facets = new Dictionary<string, string>();
            if (action.Facettes is not null)
            {
                foreach (var kvp in action.Facettes) facets.Add(kvp.Key, kvp.Value);
            }
            if (action.Facets is not null)
            {
                foreach (var kvp in action.Facets) facets.Add(kvp.Key, kvp.Value);
            }
            var actionView = new ActionView
            {
                Command = ExpandEnv(action.Command),
                Arguments = FormatArguments(action.Arguments),
                WorkingDirectory = BuildAbsolutePath(action.WorkingDirectory),
                Environment = [],
                ExitCodes = action.ExitCodes != null && action.ExitCodes.Length > 0
                    ? action.ExitCodes
                    : new[] { 0 },
                Title = action.Description,
                Reassure = action.Reassure,
                Visible = !action.Background,
                Logs = ExpandEnv(action.Logs),
                NoLogs = action.NoLogs,
                KeepOpen = action.KeepOpen,
                AlwaysClose = action.AlwaysClose,
                Tags = action.Tags ?? [],
                Facets = facets,
            };
            if (!string.IsNullOrWhiteSpace(action.Verb))
            {
                actionView.Facets[nameof(CommandAction.Verb)] = action.Verb;
            }
            if (!string.IsNullOrWhiteSpace(action.Service))
            {
                actionView.Facets[nameof(CommandAction.Service)] = action.Service;
            }
            if (!string.IsNullOrWhiteSpace(action.Host))
            {
                actionView.Facets[nameof(CommandAction.Host)] = action.Host;
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
            var facets = new Dictionary<string, string[]>();
            if (actionPattern.Facettes is not null)
            {
                foreach (var kvp in actionPattern.Facettes) facets.Add(kvp.Key, kvp.Value);
            }
            if (actionPattern.Facets is not null)
            {
                foreach (var kvp in actionPattern.Facets) facets.Add(kvp.Key, kvp.Value);
            }

            void AddFacetValues(string key, string[] values)
            {
                if (values == null || values.Length == 0) return;
                if (facets.TryGetValue(key, out string[] oldValues))
                    facets[key] = oldValues.Concat(values).Distinct().ToArray();
                else
                    facets[key] = values;
            }
            AddFacetValues(nameof(CommandAction.Verb), actionPattern.Verb);
            AddFacetValues(nameof(CommandAction.Service), actionPattern.Service);
            AddFacetValues(nameof(CommandAction.Host), actionPattern.Host);

            return EnumerateVariations(facets)
                .Select(d => ActionViewFromPatternVariation(actionPattern, d));
        }

        private ActionView ActionViewFromDiscoveredMatch(
            CommandActionDiscovery actionDiscovery,
            string[] groupNames, Match m, string file)
        {
            var facets = new Dictionary<string, string>();
            if (actionDiscovery.Facettes is not null)
            {
                foreach (var kvp in actionDiscovery.Facettes) facets.Add(kvp.Key, kvp.Value);
            }
            if (actionDiscovery.Facets is not null)
            {
                foreach (var kvp in actionDiscovery.Facets) facets.Add(kvp.Key, kvp.Value);
            }

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
                facets[groupName] = g.Value;
            }
            if (verb != null) facets[nameof(CommandAction.Verb)] = verb;
            if (service != null) facets[nameof(CommandAction.Service)] = service;
            if (host != null) facets[nameof(CommandAction.Host)] = host;

            return new()
            {
                Title = ExpandTemplate(actionDiscovery.Description, facets),
                Reassure = actionDiscovery.Reassure,
                Visible = !actionDiscovery.Background,
                Logs = ExpandEnv(ExpandTemplate(actionDiscovery.Logs, facets)),
                NoLogs = actionDiscovery.NoLogs,
                KeepOpen = actionDiscovery.KeepOpen,
                AlwaysClose = actionDiscovery.AlwaysClose,
                Command = file,
                Arguments = FormatArguments(actionDiscovery.Arguments),
                WorkingDirectory = BuildAbsolutePath(
                    ExpandTemplate(actionDiscovery.WorkingDirectory, facets)),
                Environment = [],
                ExitCodes = actionDiscovery.ExitCodes != null && actionDiscovery.ExitCodes.Length > 0
                    ? actionDiscovery.ExitCodes
                    : [0],
                Facets = ExpandDictionaryTemplate(facets, facets),
                Tags = actionDiscovery.Tags ?? []
            };
        }

        private static ActionView ActionViewFromPatternVariation(
            CommandActionPattern actionPattern, Dictionary<string, string> facets)
            => new()
            {
                Title = ExpandTemplate(actionPattern.Description, facets),
                Reassure = actionPattern.Reassure,
                Visible = !actionPattern.Background,
                Logs = ExpandEnv(ExpandTemplate(actionPattern.Logs, facets)),
                NoLogs = actionPattern.NoLogs,
                KeepOpen = actionPattern.KeepOpen,
                AlwaysClose = actionPattern.AlwaysClose,
                Command = ExpandEnv(ExpandTemplate(actionPattern.Command, facets)),
                Arguments = FormatArguments(
                    actionPattern.Arguments?.Select(a => ExpandTemplate(a, facets))),
                WorkingDirectory = BuildAbsolutePath(
                    ExpandTemplate(actionPattern.WorkingDirectory, facets)),
                Environment = [],
                ExitCodes = actionPattern.ExitCodes != null && actionPattern.ExitCodes.Length > 0
                    ? actionPattern.ExitCodes
                    : [0],
                Facets = ExpandDictionaryTemplate(facets, facets),
                Tags = actionPattern.Tags ?? []
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
                ExitCodes = monitor.ExitCodes != null && monitor.ExitCodes.Length > 0
                    ? monitor.ExitCodes
                    : [0],
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
                : [];

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
                ExitCodes = monitorDiscovery.ExitCodes != null && monitorDiscovery.ExitCodes.Length > 0
                    ? monitorDiscovery.ExitCodes
                    : [0],
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
                ExitCodes = monitorPattern.ExitCodes != null && monitorPattern.ExitCodes.Length > 0
                    ? monitorPattern.ExitCodes
                    : [0],
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
                Timeout = new TimeSpan(0, 0, monitor.Timeout),
                ServerCertificateHash = monitor.ServerCertificateHash,
                NoTlsVerify = monitor.NoTlsVerify,
                StatusCodes = monitor.StatusCodes != null && monitor.StatusCodes.Length > 0
                    ? monitor.StatusCodes
                    : [200, 201, 202, 203, 204],
                RequiredPatterns = BuildPatterns(monitor.RequiredPatterns),
                ForbiddenPatterns = BuildPatterns(monitor.ForbiddenPatterns),
            };

        private static IEnumerable<MonitorView> ExpandWebMonitorPattern(WebMonitorPattern monitorPattern)
            => monitorPattern.Variables != null
                ? EnumerateVariations(monitorPattern.Variables)
                    .Select(d => MonitorViewFromPatternVariation(monitorPattern, d))
                : [];

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
                Timeout = new TimeSpan(0, 0, monitorPattern.Timeout),
                ServerCertificateHash = monitorPattern.ServerCertificateHash,
                NoTlsVerify = monitorPattern.NoTlsVerify,
                StatusCodes = monitorPattern.StatusCodes != null && monitorPattern.StatusCodes.Length > 0
                    ? monitorPattern.StatusCodes
                    : [200, 201, 202, 203, 204],
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
                UserInteraction.ShowMessage(
                    "Parsing Regular Expression",
                    "Error in regular expression: " + exc.Message,
                    symbol: InteractionSymbol.Warning);
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

        private static Regex[] BuildPatterns(IEnumerable<string> patterns)
        {
            try
            {
                return patterns?.Select(p => new Regex(p,
                        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline,
                        TimeSpan.FromMilliseconds(1000))
                    ).ToArray() ?? [];
            }
            catch (ArgumentException exc)
            {
                UserInteraction.ShowMessage(
                    "Parsing Regular Expression",
                    "Error in regular expression: " + exc.Message,
                    symbol: InteractionSymbol.Error);
                return [];
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
            if (dict == null) return [];
            var result = new Dictionary<TKey, TValue>(dict);
            foreach (var kvp in dict)
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
