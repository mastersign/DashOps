using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Mastersign.DashOps.Model_v2;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using static Mastersign.DashOps.Model_v2.Helper;

namespace Mastersign.DashOps
{
    public class ProjectLoader_v2 : IProjectLoader
    {
        private static readonly string[] SUPPORTED_VERSIONS = ["2.0"];

        public string ProjectPath { get; }

        public Project Project { get; private set; }

        public ProjectView ProjectView { get; }

        private Action<Action> Dispatcher { get; }

        public ProjectLoader_v2(string projectPath, Action<Action> dispatcher)
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
                .WithAttributeOverride(typeof(DefaultActionSettings), nameof(DefaultActionSettings.PowerShellExe), new YamlMemberAttribute { Alias = "PowershellExe" })
                .WithAttributeOverride(typeof(DefaultActionSettings), nameof(DefaultActionSettings.PowerShellExecutionPolicy), new YamlMemberAttribute { Alias = "PowershellExecutionPolicy" })
                .WithAttributeOverride(typeof(DefaultActionSettings), nameof(DefaultActionSettings.UsePowerShellCore), new YamlMemberAttribute { Alias = "UsePowershellCore" })
                .WithAttributeOverride(typeof(DefaultActionSettings), nameof(DefaultActionSettings.UsePowerShellProfile), new YamlMemberAttribute { Alias = "UsePowershellProfile" })
                .WithAttributeOverride(typeof(DefaultMonitorSettings), nameof(DefaultMonitorSettings.PowerShellExe), new YamlMemberAttribute { Alias = "PowershellExe" })
                .WithAttributeOverride(typeof(DefaultMonitorSettings), nameof(DefaultMonitorSettings.PowerShellExecutionPolicy), new YamlMemberAttribute { Alias = "PowershellExecutionPolicy" })
                .WithAttributeOverride(typeof(DefaultMonitorSettings), nameof(DefaultMonitorSettings.UsePowerShellCore), new YamlMemberAttribute { Alias = "UsePowershellCore" })
                .WithAttributeOverride(typeof(DefaultMonitorSettings), nameof(DefaultMonitorSettings.UsePowerShellProfile), new YamlMemberAttribute { Alias = "UsePowershellProfile" })
                .WithAttributeOverride(typeof(CommandActionBase), nameof(CommandActionBase.PowerShellExe), new YamlMemberAttribute { Alias = "PowershellExe" })
                .WithAttributeOverride(typeof(CommandActionBase), nameof(CommandActionBase.PowerShellExecutionPolicy), new YamlMemberAttribute { Alias = "PowershellExecutionPolicy" })
                .WithAttributeOverride(typeof(CommandActionBase), nameof(CommandActionBase.UsePowerShellCore), new YamlMemberAttribute { Alias = "UsePowershellCore" })
                .WithAttributeOverride(typeof(CommandActionBase), nameof(CommandActionBase.UsePowerShellProfile), new YamlMemberAttribute { Alias = "UsePowershellProfile" })
                .WithAttributeOverride(typeof(AutoActionSettings), nameof(AutoActionSettings.PowerShellExe), new YamlMemberAttribute { Alias = "PowershellExe" })
                .WithAttributeOverride(typeof(AutoActionSettings), nameof(AutoActionSettings.PowerShellExecutionPolicy), new YamlMemberAttribute { Alias = "PowershellExecutionPolicy" })
                .WithAttributeOverride(typeof(AutoActionSettings), nameof(AutoActionSettings.UsePowerShellCore), new YamlMemberAttribute { Alias = "UsePowershellCore" })
                .WithAttributeOverride(typeof(AutoActionSettings), nameof(AutoActionSettings.UsePowerShellProfile), new YamlMemberAttribute { Alias = "UsePowershellProfile" })
                .WithAttributeOverride(typeof(CommandMonitor), nameof(CommandMonitor.PowerShellExe), new YamlMemberAttribute { Alias = "PowershellExe" })
                .WithAttributeOverride(typeof(CommandMonitor), nameof(CommandMonitor.PowerShellExecutionPolicy), new YamlMemberAttribute { Alias = "PowershellExecutionPolicy" })
                .WithAttributeOverride(typeof(CommandMonitor), nameof(CommandMonitor.UsePowerShellCore), new YamlMemberAttribute { Alias = "UsePowershellCore" })
                .WithAttributeOverride(typeof(CommandMonitor), nameof(CommandMonitor.UsePowerShellProfile), new YamlMemberAttribute { Alias = "UsePowershellProfile" })
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

        private void AsureRelativeDirectory(string path)
        {
            if (path is null) return;
            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(Environment.CurrentDirectory, path);
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
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
            if (Project == null) return;

            var actionDefaults = Project.Defaults.ForActions;
            var monitorDefaults = Project.Defaults.ForMonitors;

            if (string.IsNullOrWhiteSpace(actionDefaults.Logs)) actionDefaults.Logs = "logs";
            var defaultActionLogDir = ExpandEnv(actionDefaults.Logs);
            AsureRelativeDirectory(defaultActionLogDir);
            if (string.IsNullOrWhiteSpace(monitorDefaults.Logs)) monitorDefaults.Logs = "logs";
            var defaultMonitorLogDir = ExpandEnv(monitorDefaults.Logs);
            AsureRelativeDirectory(defaultMonitorLogDir);

            if (string.IsNullOrWhiteSpace(actionDefaults.WorkingDirectory)) actionDefaults.WorkingDirectory = ".";
            if (string.IsNullOrWhiteSpace(monitorDefaults.WorkingDirectory)) monitorDefaults.WorkingDirectory = ".";

            void AddActionViews(IEnumerable<ActionView> actionViews)
            {
                foreach (var actionView in actionViews) ProjectView.ActionViews.Add(actionView);
            }
            if (Project.Actions != null) AddActionViews(Project.Actions.Select(ActionViewFromCommandAction));
            if (Project.ActionDiscovery != null) AddActionViews(Project.ActionDiscovery.SelectMany(DiscoverActions));
            if (Project.ActionPatterns != null) AddActionViews(Project.ActionPatterns.SelectMany(ExpandActionPattern));

            ProjectView.IsMonitoringPaused = Project.PauseMonitoring;
            var defaultMonitorInterval = new TimeSpan(0, 0, monitorDefaults.Interval);
            var defaultWebMonitorTimeout = new TimeSpan(0, 0, monitorDefaults.HttpTimeout);
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
                monitorView.Logs = BuildMonitorLogDirPath(monitorView.Logs, monitorView.NoLogs);
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

            var tagsPerspective = ProjectView.AddTagsPerspective();
            var facetPerspectives = new Dictionary<string, PerspectiveView>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var perspective in Project.Perspectives)
            {
                facetPerspectives[perspective.Facet]
                    = ProjectView.AddFacetPerspective(perspective.Facet, perspective.Caption);
            }

            CreateFacetViews();

            if (!string.IsNullOrEmpty(Project.StartupPerspective))
            {
                if (Project.StartupPerspective.Equals("Tags", StringComparison.InvariantCultureIgnoreCase))
                {
                    ProjectView.CurrentPerspective = tagsPerspective;
                }
                else
                {
                    ProjectView.CurrentPerspective =
                        facetPerspectives.TryGetValue(Project.StartupPerspective, out var perspective)
                            ? perspective : null;
                }
                if (ProjectView.CurrentPerspective is not null && 
                    !string.IsNullOrEmpty(Project.StartupSelection))
                {
                    ProjectView.CurrentPerspective.CurrentSubset =
                        ProjectView.CurrentPerspective.Subsets
                            .FirstOrDefault(ss => string.Equals(
                                Project.StartupSelection,
                                ss.Title,
                                StringComparison.InvariantCulture));
                }
            }
        }

        private string BuildLogDirPath(string logs, string defaultLogs, bool noLogs)
        {
            if (noLogs) return null;
            logs = logs ?? defaultLogs;
            if (logs != null && !Path.IsPathRooted(logs))
            {
                logs = Path.Combine(Environment.CurrentDirectory, logs);
            }
            return logs;
        }

        private string BuildActionLogDirPath(string logs, bool noLogs)
            => BuildLogDirPath(logs, Project.Defaults.ForActions.Logs, noLogs);

        private string BuildMonitorLogDirPath(string logs, bool noLogs)
            => BuildLogDirPath(logs, Project.Defaults.ForMonitors.Logs, noLogs);

        private IEnumerable<AutoActionSettings> AutoSettingsFor(MatchableAction action)
            => Project.AutoSettings.ForActions.Where(a => a.Match(action));

        private void CreateFacetViews()
        {
            foreach (var actionView in ProjectView.ActionViews)
            {
                actionView.FacetViews.Clear();
                foreach (var kvp in actionView.Facets)
                {
                    actionView.FacetViews.Add(new FacetView(
                        facet: kvp.Key,
                        title: ProjectView.Perspectives
                            .Where(p => p.Facet == kvp.Key)
                            .Select(p => p.Title)
                            .FirstOrDefault() ?? kvp.Key,
                        value: kvp.Value
                    ));
                }
            }
        }

        private ActionView ActionViewFromCommandAction(CommandAction action)
        {
            var matchable = action.CreateMatchable();
            var autoSettings = AutoSettingsFor(matchable).ToList();
            return action.CreateView(Project.Defaults.ForActions, autoSettings);
        }
        
        private IEnumerable<ActionView> DiscoverActions(CommandActionDiscovery actionDiscovery)
        {
            var basePath = BuildAbsolutePath(actionDiscovery.BasePath);
            if (!Directory.Exists(basePath)) yield break;
            var pathRegex = BuildPathPattern(actionDiscovery.PathPattern);
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
            var facets = actionPattern.Facets != null
                ? new Dictionary<string, string[]>(actionPattern.Facets)
                : [];

            return EnumerateVariations(facets)
                .Select(d => ActionViewFromPatternVariation(actionPattern, d));
        }

        private ActionView ActionViewFromDiscoveredMatch(
            CommandActionDiscovery actionDiscovery,
            string[] groupNames, Match m, string file)
        {
            var discoveryFacets = new Dictionary<string, string>();
            foreach (var groupName in groupNames)
            {
                if (groupName == "0") continue;
                var g = m.Groups[groupName];
                if (!g.Success) continue;
                discoveryFacets[groupName] = g.Value;
            }

            var matchable = actionDiscovery.CreateMatchable(discoveryFacets, file);
            var autoSettings = AutoSettingsFor(matchable).ToList();
            return actionDiscovery.CreateView(Project.Defaults.ForActions, autoSettings, discoveryFacets, file);
        }

        private ActionView ActionViewFromPatternVariation(
            CommandActionPattern actionPattern, Dictionary<string, string> facets)
        {
            var matchableAction = actionPattern.CreateMatchable(facets);
            var autoSettings = AutoSettingsFor(matchableAction).ToList();
            return actionPattern.CreateView(Project.Defaults.ForActions, autoSettings, facets);
        }

        private MonitorView MonitorViewFromCommandMonitor(CommandMonitor monitor)
        {
            var defaults = Project.Defaults.ForMonitors;
            return new CommandMonitorView
            {
                Title = monitor.Title,
                Logs = ExpandEnv(monitor.Logs ?? defaults.Logs),
                NoLogs = monitor.NoLogs ?? defaults.NoLogs,
                NoExecutionInfo = monitor.NoExecutionInfo ?? defaults.NoExecutionInfo,
                Deactivated = monitor.Deactivated ?? defaults.Deactivated,
                Interval = new TimeSpan(0, 0, monitor.Interval ?? defaults.Interval),
                UsePowerShellCore = monitor.UsePowerShellCore ?? defaults.UsePowerShellCore,
                PowerShellExe = !string.IsNullOrWhiteSpace(monitor.PowerShellExe)
                            ? ExpandEnv(monitor.PowerShellExe)
                            : ExpandEnv(defaults.PowerShellExe),
                UsePowerShellProfile = monitor.UsePowerShellProfile ?? defaults.UsePowerShellProfile,
                PowerShellExecutionPolicy = monitor.PowerShellExecutionPolicy ?? defaults.PowerShellExecutionPolicy,
                Command = ExpandEnv(monitor.Command),
                Arguments = FormatArguments(monitor.Arguments?.Select(ExpandEnv)),
                WorkingDirectory = BuildAbsolutePath(monitor.WorkingDirectory ?? defaults.WorkingDirectory),
                Environment = Merge(defaults.Environment, monitor.Environment),
                ExePaths = (monitor.ExePaths ?? defaults.ExePaths ?? [])
                            .Select(ExpandEnv)
                            .ToArray(),
                ExitCodes = monitor.ExitCodes ?? defaults.ExitCodes ?? [0],
                RequiredPatterns = BuildTextPatterns(monitor.RequiredPatterns ?? defaults.RequiredPatterns),
                ForbiddenPatterns = BuildTextPatterns(monitor.ForbiddenPatterns ?? defaults.ForbiddenPatterns),
            };
        }

        private IEnumerable<MonitorView> DiscoverMonitors(CommandMonitorDiscovery monitorDiscovery)
        {
            var basePath = BuildAbsolutePath(monitorDiscovery.BasePath);
            if (!Directory.Exists(basePath)) yield break;
            var pathRegex = BuildPathPattern(monitorDiscovery.PathPattern);
            if (pathRegex == null) yield break;

            var groupNames = pathRegex.GetGroupNames();
            foreach (var discovery in DiscoverFiles(basePath, pathRegex))
            {
                var file = discovery.Item1;
                var match = discovery.Item2;
                yield return MonitorViewFromDiscoveredMatch(monitorDiscovery, groupNames, match, file);
            }
        }

        private IEnumerable<MonitorView> ExpandCommandMonitorPattern(CommandMonitorPattern monitorPattern)
            => monitorPattern.Variables != null
                ? EnumerateVariations(monitorPattern.Variables)
                    .Select(d => MonitorViewFromPatternVariation(monitorPattern, d))
                : [];

        private MonitorView MonitorViewFromDiscoveredMatch(CommandMonitorDiscovery monitorDiscovery,
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

            string cmd;
            string cmdArgs;
            var fileVariable = new Dictionary<string, string> { { "File", file } };
            if (!string.IsNullOrWhiteSpace(monitorDiscovery.Interpreter))
            {
                // custom interpreter for discovered files

                cmd = ExpandEnv(ExpandTemplate(monitorDiscovery.Interpreter, variables));
                if (monitorDiscovery.Arguments is null)
                {
                    cmdArgs = FormatArguments([file]);
                }
                else
                {
                    cmdArgs = FormatArguments(monitorDiscovery.Arguments
                        // expand ${File} and ${file} into discovered filename 
                        .Select(a => ExpandTemplate(a, fileVariable))
                        // expand group names from Regex match
                        .Select(a => ExpandTemplate(a, variables))
                        // expand CMD-style environment variables
                        .Select(ExpandEnv));
                }
            }
            else
            {
                // discovered file is used as command itself

                cmd = file;
                cmdArgs = FormatArguments(monitorDiscovery.Arguments?
                    // expand group names from Regex match
                    .Select(a => ExpandTemplate(a, variables))
                    // expand CMD-style environment variables
                    .Select(ExpandEnv));
            }

            var defaults = Project.Defaults.ForMonitors;
            return new CommandMonitorView
            {
                Title = ExpandTemplate(monitorDiscovery.Title, variables),
                Logs = ExpandEnv(ExpandTemplate(monitorDiscovery.Logs ?? defaults.Logs, variables)),
                NoLogs = monitorDiscovery.NoLogs ?? defaults.NoLogs,
                NoExecutionInfo = monitorDiscovery.NoExecutionInfo ?? defaults.NoExecutionInfo,
                Deactivated = monitorDiscovery.Deactivated ?? defaults.Deactivated,
                Interval = new TimeSpan(0, 0, monitorDiscovery.Interval ?? defaults.Interval),
                UsePowerShellCore = monitorDiscovery.UsePowerShellCore ?? defaults.UsePowerShellCore,
                PowerShellExe = !string.IsNullOrWhiteSpace(monitorDiscovery.PowerShellExe)
                    ? ExpandEnv(ExpandTemplate(monitorDiscovery.PowerShellExe, variables))
                    : ExpandEnv(ExpandTemplate(defaults.PowerShellExe, variables)),
                UsePowerShellProfile = monitorDiscovery.UsePowerShellProfile ?? defaults.UsePowerShellProfile,
                PowerShellExecutionPolicy = !string.IsNullOrWhiteSpace(monitorDiscovery.PowerShellExecutionPolicy)
                    ? ExpandTemplate(monitorDiscovery.PowerShellExecutionPolicy, variables)
                    : ExpandTemplate(defaults.PowerShellExecutionPolicy, variables),
                Command = cmd,
                Arguments = cmdArgs,
                WorkingDirectory = BuildAbsolutePath(
ExpandEnv(ExpandTemplate(monitorDiscovery.WorkingDirectory, variables))),
                Environment = ExpandEnv(
ExpandDictionaryTemplate(
ExpandDictionaryTemplate(
Merge(defaults.Environment, monitorDiscovery.Environment ?? []),
                            fileVariable),
                        variables)),
                ExePaths = (monitorDiscovery.ExePaths ?? defaults.ExePaths ?? [])
                    .Select(p => ExpandTemplate(p, variables))
                    .Select(ExpandEnv)
                    .ToArray(),
                ExitCodes = monitorDiscovery.ExitCodes ?? defaults.ExitCodes ?? [0],
                RequiredPatterns = BuildTextPatterns(monitorDiscovery.RequiredPatterns ?? defaults.RequiredPatterns),
                ForbiddenPatterns = BuildTextPatterns(monitorDiscovery.ForbiddenPatterns ?? defaults.ForbiddenPatterns)
            };
        }

        private MonitorView MonitorViewFromPatternVariation(
            CommandMonitorPattern monitorPattern, Dictionary<string, string> variables)
        {
            var defaults = Project.Defaults.ForMonitors;
            return new CommandMonitorView
            {
                Title = ExpandTemplate(monitorPattern.Title, variables),
                Logs = ExpandEnv(ExpandTemplate(monitorPattern.Logs ?? defaults.Logs, variables)),
                NoLogs = monitorPattern.NoLogs ?? defaults.NoLogs,
                NoExecutionInfo = monitorPattern.NoExecutionInfo ?? defaults.NoExecutionInfo,
                Deactivated = monitorPattern.Deactivated ?? defaults.Deactivated,
                Interval = new TimeSpan(0, 0, monitorPattern.Interval ?? defaults.Interval),
                UsePowerShellCore = monitorPattern.UsePowerShellCore ?? defaults.UsePowerShellCore,
                PowerShellExe = !string.IsNullOrWhiteSpace(monitorPattern.PowerShellExe)
                            ? ExpandEnv(ExpandTemplate(monitorPattern.PowerShellExe, variables))
                            : ExpandEnv(ExpandTemplate(defaults.PowerShellExe, variables)),
                UsePowerShellProfile = monitorPattern.UsePowerShellProfile ?? defaults.UsePowerShellProfile,
                PowerShellExecutionPolicy = !string.IsNullOrWhiteSpace(monitorPattern.PowerShellExecutionPolicy)
                            ? ExpandTemplate(monitorPattern.PowerShellExecutionPolicy, variables)
                            : ExpandTemplate(defaults.PowerShellExecutionPolicy, variables),
                Command = ExpandEnv(ExpandTemplate(monitorPattern.Command, variables)),
                Arguments = FormatArguments(
                            monitorPattern.Arguments?.Select(a => ExpandEnv(ExpandTemplate(a, variables)))),
                WorkingDirectory = BuildAbsolutePath(
ExpandEnv(ExpandTemplate(monitorPattern.WorkingDirectory, variables))),
                Environment = ExpandEnv(
ExpandDictionaryTemplate(
Merge(defaults.Environment, monitorPattern.Environment ?? []),
                                variables)),
                ExePaths = (monitorPattern.ExePaths ?? defaults.ExePaths ?? [])
                            .Select(p => ExpandTemplate(p, variables))
                            .Select(ExpandEnv)
                            .ToArray(),
                ExitCodes = monitorPattern.ExitCodes ?? defaults.ExitCodes ?? [0],
                RequiredPatterns = BuildTextPatterns(monitorPattern.RequiredPatterns ?? defaults.RequiredPatterns),
                ForbiddenPatterns = BuildTextPatterns(monitorPattern.ForbiddenPatterns ?? defaults.ForbiddenPatterns)
            };
        }

        private MonitorView MonitorViewFromWebMonitor(WebMonitor monitor)
        {
            var defaults = Project.Defaults.ForMonitors;
            return new WebMonitorView
            {
                Title = monitor.Title,
                Logs = ExpandEnv(monitor.Logs ?? defaults.Logs),
                NoLogs = monitor.NoLogs ?? defaults.NoLogs,
                NoExecutionInfo = monitor.NoExecutionInfo ?? defaults.NoExecutionInfo,
                Deactivated = monitor.Deactivated ?? defaults.Deactivated,
                Interval = new TimeSpan(0, 0, monitor.Interval ?? defaults.Interval),
                Url = monitor.Url,
                Headers = new Dictionary<string, string>(monitor.Headers ?? []),
                Timeout = new TimeSpan(0, 0, monitor.HttpTimeout ?? defaults.HttpTimeout),
                ServerCertificateHash = monitor.ServerCertificateHash,
                NoTlsVerify = monitor.NoTlsVerify ?? defaults.NoTlsVerify,
                StatusCodes = monitor.StatusCodes ?? defaults.StatusCodes ?? [200, 201, 202, 203, 204],
                RequiredPatterns = BuildTextPatterns(monitor.RequiredPatterns ?? defaults.RequiredPatterns),
                ForbiddenPatterns = BuildTextPatterns(monitor.ForbiddenPatterns ?? defaults.ForbiddenPatterns),
            };
        }

        private IEnumerable<MonitorView> ExpandWebMonitorPattern(WebMonitorPattern monitorPattern)
            => monitorPattern.Variables != null
                ? EnumerateVariations(monitorPattern.Variables)
                    .Select(d => MonitorViewFromPatternVariation(monitorPattern, d))
                : [];

        private MonitorView MonitorViewFromPatternVariation(
            WebMonitorPattern monitorPattern, Dictionary<string, string> variables)
        {
            var defaults = Project.Defaults.ForMonitors;
            return new WebMonitorView
            {
                Title = ExpandTemplate(monitorPattern.Title, variables),
                Logs = ExpandEnv(ExpandTemplate(monitorPattern.Logs ?? defaults.Logs, variables)),
                NoLogs = monitorPattern.NoLogs ?? defaults.NoLogs,
                NoExecutionInfo = monitorPattern.NoExecutionInfo ?? defaults.NoExecutionInfo,
                Deactivated = monitorPattern.Deactivated ?? defaults.Deactivated,
                Interval = new TimeSpan(0, 0, monitorPattern.Interval ?? defaults.Interval),
                Url = ExpandTemplate(monitorPattern.Url, variables),
                Headers = ExpandDictionaryTemplate(monitorPattern.Headers, variables),
                Timeout = new TimeSpan(0, 0, monitorPattern.HttpTimeout ?? defaults.HttpTimeout),
                ServerCertificateHash = monitorPattern.ServerCertificateHash,
                NoTlsVerify = monitorPattern.NoTlsVerify ?? defaults.NoTlsVerify,
                StatusCodes = monitorPattern.StatusCodes ?? defaults.StatusCodes ?? [200, 201, 202, 203, 204],
                RequiredPatterns = BuildTextPatterns(monitorPattern.RequiredPatterns ?? defaults.RequiredPatterns),
                ForbiddenPatterns = BuildTextPatterns(monitorPattern.ForbiddenPatterns ?? defaults.ForbiddenPatterns),
            };
        }
    }
}
