using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Mastersign.DashOps.Model_v2;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

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

            var defaultActionLogDir = ExpandEnv(actionDefaults.Logs);
            AsureRelativeDirectory(defaultActionLogDir);
            var defaultMonitorLogDir = ExpandEnv(monitorDefaults.Logs);
            AsureRelativeDirectory(defaultMonitorLogDir);

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
                actionView.Logs = BuildActionLogDirPath(actionView.Logs, actionView.NoLogs);
                if (actionDefaults.KeepOpen) actionView.KeepOpen = true;
                if (actionDefaults.AlwaysClose) actionView.AlwaysClose = true;
                var logInfo = LogFileManager.GetLastLogFileInfo(actionView);
                if (logInfo != null)
                {
                    actionView.Status = logInfo.Success ? ActionStatus.Success : ActionStatus.Failed;
                }
            }

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

        private void ApplyAutoAnnotations()
        {
            if (Project.AutoSettings?.ForActions is null) return;
            foreach (var actionView in ProjectView.ActionViews)
            {
                foreach (var autoSettings in Project.AutoSettings.ForActions.Where(a => a.Match(actionView)))
                {
                    ApplyAutoSettings(actionView, autoSettings);
                }
            }
        }

        private void ApplyAutoSettings(ActionView action, AutoActionSettings autoSettings)
        {
            if (autoSettings.Tags != null)
            {
                action.Tags = [.. action.Tags.Union(autoSettings.Tags)];
            }

            if (autoSettings.Facets != null)
            {
                foreach (var facetName in autoSettings.Facets.Keys)
                {
                    action.Facets[facetName] = autoSettings.Facets[facetName];
                }
            }

            action.Reassure = autoSettings.Reassure ?? action.Reassure;
            action.Logs = autoSettings.Logs ?? action.Logs;
            action.NoLogs = autoSettings.NoLogs ?? action.NoLogs;
            action.NoExecutionInfo = autoSettings.NoExecutionInfo ?? action.NoExecutionInfo;
            action.KeepOpen = autoSettings.KeepOpen ?? action.KeepOpen;
            action.AlwaysClose = autoSettings.AlwaysClose ?? action.AlwaysClose;
            action.Visible = !(autoSettings.Background ?? !action.Visible);

            if (autoSettings.Environment != null)
            {
                foreach (var kvp in autoSettings.Environment)
                {
                    action.Environment[kvp.Key] = kvp.Value;
                }
            }
            if (autoSettings.ExePaths != null)
            {
                action.ExePaths = autoSettings.ExePaths
                    // expand facets
                    .Select(p => ExpandTemplate(p, action.Facets))
                    // expand CMD-style environment variables
                    .Select(ExpandEnv)
                    .ToArray();
            }

            action.ExitCodes = autoSettings.ExitCodes ?? action.ExitCodes;

            action.UsePowerShellCore = autoSettings.UsePowerShellCore ?? action.UsePowerShellCore;
            if (autoSettings.PowerShellExe != null)
            {
                action.PowerShellExe = ExpandEnv(ExpandTemplate(autoSettings.PowerShellExe, action.Facets));
            }
            action.UsePowerShellProfile = autoSettings.UsePowerShellProfile ?? action.UsePowerShellProfile;
            action.PowerShellExecutionPolicy = autoSettings.PowerShellExecutionPolicy ?? action.PowerShellExecutionPolicy;

            action.UseWindowsTerminal = autoSettings.UseWindowsTerminal ?? action.UseWindowsTerminal;
            if (autoSettings.WindowsTerminalArgs != null)
            {
                action.WindowsTerminalArguments = FormatArguments(autoSettings.WindowsTerminalArgs
                    // expand facets
                    .Select(a => ExpandTemplate(a, action.Facets))
                    // expand CMD-style environment variables
                    .Select(ExpandEnv));
            }
        }

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
            var defaults = Project.Defaults.ForActions;
            var facets = new Dictionary<string, string>(action.Facets ?? []);
            var actionView = new ActionView
            {
                UsePowerShellCore = action.UsePowerShellCore ?? defaults.UsePowerShellCore,
                PowerShellExe = !string.IsNullOrWhiteSpace(action.PowerShellExe)
                    ? ExpandEnv(ExpandTemplate(action.PowerShellExe, facets))
                    : ExpandEnv(ExpandTemplate(defaults.PowerShellExe, facets)),
                UsePowerShellProfile = action.UsePowerShellProfile ?? defaults.UsePowerShellProfile,
                PowerShellExecutionPolicy = !string.IsNullOrWhiteSpace(action.PowerShellExecutionPolicy)
                    ? action.PowerShellExecutionPolicy
                    : defaults.PowerShellExecutionPolicy,
                Command = ExpandEnv(ExpandTemplate(action.Command, facets)),
                Arguments = FormatArguments(
                    action.Arguments?.Select(a => ExpandEnv(ExpandTemplate(a, facets)))),
                WorkingDirectory = BuildAbsolutePath(action.WorkingDirectory),
                Environment = Merge(defaults.Environment, action.Environment),
                ExePaths = (action.ExePaths ?? defaults.ExePaths ?? [])
                    .Select(p => ExpandTemplate(p, facets))
                    .Select(ExpandEnv)
                    .ToArray(),
                UseWindowsTerminal = action.UseWindowsTerminal ?? defaults.UseWindowsTerminal,
                WindowsTerminalArguments = FormatArguments(
                    (action.WindowsTerminalArgs ?? defaults.WindowsTerminalArgs)
                        .Select(a => ExpandEnv(ExpandTemplate(a, facets)))),
                ExitCodes = action.ExitCodes ?? defaults.ExitCodes ?? [0],
                Title = action.Description,
                Reassure = action.Reassure ?? defaults.Reassure,
                Visible = !(action.Background ?? defaults.Background),
                Logs = ExpandEnv(action.Logs ?? defaults.Logs),
                NoLogs = action.NoLogs ?? defaults.NoLogs,
                NoExecutionInfo = action.NoExecutionInfo ?? defaults.NoExecutionInfo,
                KeepOpen = action.KeepOpen ?? defaults.KeepOpen,
                AlwaysClose = action.AlwaysClose ?? defaults.AlwaysClose,
                Tags = action.Tags ?? [],
                Facets = facets,
            };
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
            var defaults = Project.Defaults.ForActions;
            var facets = new Dictionary<string, string>(actionDiscovery.Facets ?? []);

            // capture named Regex groups into facets
            foreach (var groupName in groupNames)
            {
                if (groupName == "0") continue;
                var g = m.Groups[groupName];
                if (!g.Success) continue;
                facets[groupName] = g.Value;
            }

            string cmd;
            string cmdArgs;
            var fileVariable = new Dictionary<string, string> { { "File", file } };
            if (!string.IsNullOrWhiteSpace(actionDiscovery.Interpreter))
            {
                // custom interpreter for discovered files

                cmd = ExpandEnv(ExpandTemplate(actionDiscovery.Interpreter, facets));
                if (actionDiscovery.Arguments is null)
                {
                    cmdArgs = FormatArguments([file]);
                }
                else
                {
                    cmdArgs = FormatArguments(actionDiscovery.Arguments
                        // expand ${File} and ${file} into discovered filename 
                        .Select(a => ExpandTemplate(a, fileVariable))
                        // expand facets
                        .Select(a => ExpandTemplate(a, facets))
                        // expand CMD-style environment variables
                        .Select(ExpandEnv));
                }
            }
            else
            {
                // discovered file is used as command itself

                cmd = file;
                cmdArgs = FormatArguments(actionDiscovery.Arguments?
                    // expand facets
                    .Select(a => ExpandTemplate(a, facets))
                    // expand CMD-style environment variables
                    .Select(ExpandEnv));
            }

            return new ActionView()
            {
                Title = ExpandTemplate(actionDiscovery.Description, facets),
                Reassure = actionDiscovery.Reassure ?? defaults.Reassure,
                Visible = !(actionDiscovery.Background ?? defaults.Background),
                Logs = ExpandEnv(ExpandTemplate(actionDiscovery.Logs ?? defaults.Logs, facets)),
                NoLogs = actionDiscovery.NoLogs ?? defaults.NoLogs,
                NoExecutionInfo = actionDiscovery.NoExecutionInfo ?? defaults.NoExecutionInfo,
                KeepOpen = actionDiscovery.KeepOpen ?? defaults.KeepOpen,
                AlwaysClose = actionDiscovery.AlwaysClose ?? defaults.AlwaysClose,
                UsePowerShellCore = actionDiscovery.UsePowerShellCore ?? defaults.UsePowerShellCore,
                PowerShellExe = !string.IsNullOrWhiteSpace(actionDiscovery.PowerShellExe)
                    ? ExpandEnv(ExpandTemplate(actionDiscovery.PowerShellExe, facets))
                    : ExpandEnv(ExpandTemplate(defaults.PowerShellExe, facets)),
                UsePowerShellProfile = actionDiscovery.UsePowerShellProfile ?? defaults.UsePowerShellProfile,
                PowerShellExecutionPolicy = !string.IsNullOrWhiteSpace(actionDiscovery.PowerShellExecutionPolicy)
                    ? ExpandTemplate(actionDiscovery.PowerShellExecutionPolicy, facets)
                    : ExpandTemplate(defaults.PowerShellExecutionPolicy, facets),
                Command = cmd,
                Arguments = cmdArgs,
                WorkingDirectory = BuildAbsolutePath(
                            ExpandEnv(ExpandTemplate(actionDiscovery.WorkingDirectory, facets))),
                Environment = ExpandEnv(
                    ExpandDictionaryTemplate(
                        ExpandDictionaryTemplate(
                            Merge(defaults.Environment, actionDiscovery.Environment ?? []),
                            fileVariable),
                        facets)),
                ExePaths = (actionDiscovery.ExePaths ?? defaults.ExePaths ?? [])
                    .Select(p => ExpandTemplate(p, facets))
                    .Select(ExpandEnv)
                    .ToArray(),
                UseWindowsTerminal = actionDiscovery.UseWindowsTerminal ?? defaults.UseWindowsTerminal,
                WindowsTerminalArguments = FormatArguments(
                    (actionDiscovery.WindowsTerminalArgs ?? defaults.WindowsTerminalArgs)
                        .Select(a => ExpandTemplate(a, fileVariable))
                        .Select(a => ExpandTemplate(a, facets))
                        .Select(ExpandEnv)),
                ExitCodes = actionDiscovery.ExitCodes ?? defaults.ExitCodes ?? [0],
                Facets = ExpandDictionaryTemplate(facets, facets),
                Tags = actionDiscovery.Tags ?? [],
            };
        }

        private ActionView ActionViewFromPatternVariation(
            CommandActionPattern actionPattern, Dictionary<string, string> facets)
        {
            var defaults = Project.Defaults.ForActions;
            return new()
            {
                Title = ExpandTemplate(actionPattern.Description, facets),
                Reassure = actionPattern.Reassure ?? defaults.Reassure,
                Visible = !(actionPattern.Background ?? defaults.Background),
                Logs = ExpandEnv(ExpandTemplate(actionPattern.Logs ?? defaults.Logs, facets)),
                NoLogs = actionPattern.NoLogs ?? defaults.NoLogs,
                NoExecutionInfo = actionPattern.NoExecutionInfo ?? defaults.NoExecutionInfo,
                KeepOpen = actionPattern.KeepOpen ?? defaults.AlwaysClose,
                AlwaysClose = actionPattern.AlwaysClose ?? defaults.AlwaysClose,
                UsePowerShellCore = actionPattern.UsePowerShellCore ?? defaults.UsePowerShellCore,
                PowerShellExe = !string.IsNullOrWhiteSpace(actionPattern.PowerShellExe)
                            ? ExpandEnv(ExpandTemplate(actionPattern.PowerShellExe, facets))
                            : ExpandEnv(ExpandTemplate(defaults.PowerShellExe, facets)),
                UsePowerShellProfile = actionPattern.UsePowerShellProfile ?? defaults.UsePowerShellProfile,
                PowerShellExecutionPolicy = !string.IsNullOrWhiteSpace(actionPattern.PowerShellExecutionPolicy)
                            ? ExpandTemplate(actionPattern.PowerShellExecutionPolicy, facets)
                            : ExpandTemplate(defaults.PowerShellExecutionPolicy, facets),
                Command = ExpandEnv(ExpandTemplate(actionPattern.Command, facets)),
                Arguments = FormatArguments(actionPattern.Arguments?
                            .Select(a => ExpandEnv(ExpandTemplate(a, facets)))),
                WorkingDirectory = BuildAbsolutePath(
                            ExpandEnv(ExpandTemplate(actionPattern.WorkingDirectory, facets))),
                Environment = ExpandEnv(
                            ExpandDictionaryTemplate(
                                Merge(defaults.Environment, actionPattern.Environment ?? []),
                                facets)),
                ExePaths = (actionPattern.ExePaths ?? defaults.ExePaths ?? [])
                            .Select(p => ExpandTemplate(p, facets))
                            .Select(ExpandEnv)
                            .ToArray(),
                UseWindowsTerminal = actionPattern.UseWindowsTerminal ?? defaults.UseWindowsTerminal,
                WindowsTerminalArguments = FormatArguments(
                            (actionPattern.WindowsTerminalArgs ?? defaults.WindowsTerminalArgs)
                                .Select(a => ExpandTemplate(a, facets))
                                .Select(ExpandEnv)),
                ExitCodes = actionPattern.ExitCodes ?? defaults.ExitCodes ?? [0],
                Facets = ExpandDictionaryTemplate(facets, facets),
                Tags = actionPattern.Tags ?? [],
            };
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
                RequiredPatterns = BuildPatterns(monitor.RequiredPatterns ?? defaults.RequiredPatterns),
                ForbiddenPatterns = BuildPatterns(monitor.ForbiddenPatterns ?? defaults.ForbiddenPatterns),
            };
        }

        private IEnumerable<MonitorView> DiscoverMonitors(CommandMonitorDiscovery monitorDiscovery)
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
                RequiredPatterns = BuildPatterns(monitorDiscovery.RequiredPatterns ?? defaults.RequiredPatterns),
                ForbiddenPatterns = BuildPatterns(monitorDiscovery.ForbiddenPatterns ?? defaults.ForbiddenPatterns)
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
                RequiredPatterns = BuildPatterns(monitorPattern.RequiredPatterns ?? defaults.RequiredPatterns),
                ForbiddenPatterns = BuildPatterns(monitorPattern.ForbiddenPatterns ?? defaults.ForbiddenPatterns)
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
                RequiredPatterns = BuildPatterns(monitor.RequiredPatterns ?? defaults.RequiredPatterns),
                ForbiddenPatterns = BuildPatterns(monitor.ForbiddenPatterns ?? defaults.ForbiddenPatterns),
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
                RequiredPatterns = BuildPatterns(monitorPattern.RequiredPatterns ?? defaults.RequiredPatterns),
                ForbiddenPatterns = BuildPatterns(monitorPattern.ForbiddenPatterns ?? defaults.ForbiddenPatterns),
            };
        }

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

        private static Dictionary<string, string> ExpandEnv(Dictionary<string, string> dict)
            => MapValues(dict, ExpandEnv);

        private static string FormatArguments(IEnumerable<string> arguments)
            => arguments != null
                ? CommandLine.FormatArgumentList([.. arguments.Select(ExpandEnv)])
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

        private static Dictionary<TKey, TValue> Merge<TKey, TValue>(params IDictionary<TKey, TValue>[] dicts)
        {
            var result = new Dictionary<TKey, TValue>();
            foreach (var d in dicts)
            {
                if (d is null) continue;
                foreach (var kvp in d)
                {
                    result[kvp.Key] = kvp.Value;
                }
            }
            return result;
        }
    }
}
