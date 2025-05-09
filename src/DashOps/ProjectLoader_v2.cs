using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Mastersign.DashOps.Model_v2;
using Mastersign.WpfUiTools;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using static Mastersign.DashOps.Model_v2.Helper;

namespace Mastersign.DashOps
{
    public class ProjectLoader_v2 : IProjectLoader
    {
        private static readonly string[] SUPPORTED_VERSIONS = ["2.0"];

        private const string DEFAULT_LOG_DIR = "logs";

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
            try
            {
                LoadProject();
            } 
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return;
            }
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
            catch (YamlException yamlExc)
            {
                var msg = string.Empty;
                msg += $"{yamlExc.Start} - {yamlExc.End}";
                msg += Environment.NewLine;
                msg += yamlExc.Message;

                Exception innerExc = yamlExc.InnerException;
                while (innerExc is not null)
                {
                    msg += Environment.NewLine + innerExc.Message;
                    innerExc = innerExc.InnerException;
                }
                throw new ProjectLoadException(version, msg);
            }
            catch (Exception exc2)
            {
                var msg = string.Empty;
                msg += exc2.Message;

                while (exc2.InnerException != null)
                {
                    exc2 = exc2.InnerException;
                    msg += Environment.NewLine + exc2.Message;
                }
                throw new ProjectLoadException(version, msg);
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

        private void TransferWindowSettings(WindowSettings target, Model_v2.WindowSettings source)
        {
            target.Mode = (WindowMode)(source?.Mode ?? Model_v2.WindowMode.Default);
            target.ScreenNo = source?.ScreenNo;
            target.Left = source?.Left;
            target.Top = source?.Top;
            target.Width = source?.Width;
            target.Height = source?.Height;
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

            ProjectView.MainWindow ??= new();
            TransferWindowSettings(ProjectView.MainWindow, Project.MainWindow);
            ProjectView.EditorWindow ??= new();
            TransferWindowSettings(ProjectView.EditorWindow, Project.EditorWindow);
            ProjectView.Color = Project.Color ?? ThemeAccentColor.Default;
            ProjectView.Theme = Project.Theme ?? Theme.Auto;

            var commonDefaults = Project.Defaults;
            var actionDefaults = Project.Defaults.ForActions;
            var monitorDefaults = Project.Defaults.ForMonitors;

            if (string.IsNullOrWhiteSpace(commonDefaults.Logs)) commonDefaults.Logs = DEFAULT_LOG_DIR;
            var defaultActionLogDir = ExpandEnv(CoalesceWhitespace([actionDefaults?.Logs, commonDefaults.Logs]));
            AsureRelativeDirectory(defaultActionLogDir);
            var defaultMonitorLogDir = ExpandEnv(CoalesceWhitespace([monitorDefaults?.Logs, commonDefaults.Logs]));
            AsureRelativeDirectory(defaultMonitorLogDir);

            if (string.IsNullOrWhiteSpace(commonDefaults.WorkingDirectory)) commonDefaults.WorkingDirectory = ".";

            void AddActionViews(IEnumerable<ActionView> actionViews)
            {
                foreach (var actionView in actionViews) ProjectView.ActionViews.Add(actionView);
            }
            if (Project.Actions != null) AddActionViews(Project.Actions.Select(ActionViewFromCommandAction));
            if (Project.ActionDiscovery != null) AddActionViews(Project.ActionDiscovery.SelectMany(DiscoverActions));
            if (Project.ActionPatterns != null) AddActionViews(Project.ActionPatterns.SelectMany(ExpandActionPattern));

            ProjectView.IsMonitoringPaused = Project.PauseMonitoring;

            void AddMonitorViews(IEnumerable<MonitorView> monitorViews)
            {
                foreach (var monitorView in monitorViews) ProjectView.MonitorViews.Add(monitorView);
            }
            if (Project.Monitors != null) AddMonitorViews(Project.Monitors.Select(MonitorViewFromCommandMonitor));
            if (Project.MonitorDiscovery != null) AddMonitorViews(Project.MonitorDiscovery.SelectMany(DiscoverMonitors));
            if (Project.MonitorPatterns != null) AddMonitorViews(Project.MonitorPatterns.SelectMany(ExpandCommandMonitorPattern));
            if (Project.WebMonitors != null) AddMonitorViews(Project.WebMonitors.Select(MonitorViewFromWebMonitor));
            if (Project.WebMonitorPatterns != null) AddMonitorViews(Project.WebMonitorPatterns.SelectMany(ExpandWebMonitorPattern));

            ProjectView.ShowMonitorPanel = Project.MonitorPanel ?? ProjectView.MonitorViews.Count > 0;

            var anyTags = ProjectView.ActionViews.SelectMany(a => a.Tags).Any();

            var tagsPerspective = anyTags ? ProjectView.AddTagsPerspective() : null;
            var facetPerspectives = new Dictionary<string, PerspectiveView>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var perspective in Project.Perspectives)
            {
                facetPerspectives[perspective.Facet]
                    = ProjectView.AddFacetPerspective(perspective.Facet, perspective.Caption);
            }

            CreateFacetViews();

            if (!string.IsNullOrEmpty(Project.StartupPerspective))
            {
                if (anyTags && Project.StartupPerspective.Equals("Tags", StringComparison.InvariantCultureIgnoreCase))
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

            ProjectView.NotifyProjectUpdated();
        }

        private IEnumerable<AutoActionSettings> AutoSettingsFor(MatchableAction matchable)
            => Project.AutoSettings.ForActions.Where(s => s.Match(matchable));

        private IEnumerable<AutoMonitorSettings> AutoSettingsFor(MatchableMonitor matchable)
            => Project.AutoSettings.ForMonitors.Where(s => s.Match(matchable));

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
            return action.CreateView(autoSettings, Project.Defaults.ForActions, Project.Defaults);
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

            var matchable = actionDiscovery.CreateMatchable(Project.Defaults.ForActions, Project.Defaults, discoveryFacets, file);
            var autoSettings = AutoSettingsFor(matchable).ToList();
            return actionDiscovery.CreateView(autoSettings, Project.Defaults.ForActions, Project.Defaults, discoveryFacets, file);
        }

        private ActionView ActionViewFromPatternVariation(
            CommandActionPattern actionPattern, Dictionary<string, string> facets)
        {
            var matchableAction = actionPattern.CreateMatchable(facets);
            var autoSettings = AutoSettingsFor(matchableAction).ToList();
            return actionPattern.CreateView(autoSettings, Project.Defaults.ForActions, Project.Defaults, facets);
        }

        private MonitorView MonitorViewFromCommandMonitor(CommandMonitor monitor)
        {
            var matchableMonitor = monitor.CreateMatchable();
            var autoSettings = AutoSettingsFor(matchableMonitor).ToList();
            return monitor.CreateView(autoSettings, Project.Defaults.ForMonitors, Project.Defaults);
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

            var matchableMonitor = monitorDiscovery.CreateMatchable(Project.Defaults.ForMonitors, Project.Defaults, variables, file);
            var autoSettings = AutoSettingsFor(matchableMonitor).ToList();
            return monitorDiscovery.CreateView(autoSettings, Project.Defaults.ForMonitors, Project.Defaults, variables, file);
        }

        private MonitorView MonitorViewFromPatternVariation(
            CommandMonitorPattern monitorPattern, Dictionary<string, string> variables)
        {
            var matchableMonitor = monitorPattern.CreateMatchable(variables);
            var autoSettings = AutoSettingsFor(matchableMonitor).ToList();
            return monitorPattern.CreateView(autoSettings, Project.Defaults.ForMonitors, Project.Defaults, variables);
        }

        private MonitorView MonitorViewFromWebMonitor(WebMonitor monitor)
        {
            var matchable = monitor.CreateMatchable();
            var autoSettings = AutoSettingsFor(matchable).ToList();
            return monitor.CreateView(autoSettings, Project.Defaults.ForMonitors, Project.Defaults);
        }

        private IEnumerable<MonitorView> ExpandWebMonitorPattern(WebMonitorPattern monitorPattern)
            => monitorPattern.Variables != null
                ? EnumerateVariations(monitorPattern.Variables)
                    .Select(d => MonitorViewFromPatternVariation(monitorPattern, d))
                : [];

        private MonitorView MonitorViewFromPatternVariation(
            WebMonitorPattern monitorPattern, Dictionary<string, string> variables)
        {
            var matchable = monitorPattern.CreateMatchable(variables);
            var autoSettings = AutoSettingsFor(matchable).ToList();
            return monitorPattern.CreateView(autoSettings, Project.Defaults.ForMonitors, Project.Defaults, variables);
        }
    }
}
