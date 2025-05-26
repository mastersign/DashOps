using Mastersign.DashOps.ViewModel;

namespace Mastersign.DashOps.DesignTime
{
    sealed class DesignTimeProjectView : ProjectView
    {
        public DesignTimeProjectView()
        {
            FormatVersion = "2.0";
            Title = "Design Time Project View";
            ShowMonitorPanel = true;
            AddFacetPerspective("host", "Hosts");
            AddFacetPerspective("service", "Services");
            AddFacetPerspective("verb", "Verbs");
            AddFacetPerspective("env", "Environments");
            AddFacetPerspective("role", "Roles");
            ActionViews.Add(new ActionView
            {
                Title = "Start X on A",
                Command = "start_x",
                Arguments ="--on a",
                Facets = new Dictionary<string, string>
                {
                    {"verb", "start"},
                    {"service", "x"},
                    {"host", "a"},
                }
            });
            ActionViews.Add(new ActionView
            {
                Title = "Backup X on A",
                Command = "backup_x",
                Arguments = "--on a",
                Facets = new Dictionary<string, string>
                {
                    {"verb", "backup"},
                    {"service", "x"},
                    {"host", "a"},
                }
            });
            ActionViews.Add(new ActionView
            {
                Title = "Restore X on A",
                Command = "restore_x",
                Arguments = "--on a",
                Facets = new Dictionary<string, string>
                {
                    {"verb", "restore"},
                    {"service", "x"},
                    {"host", "a"},
                }
            });
            ActionViews.Add(new ActionView
            {
                Title = "Pause X on A",
                Command = "pause_x",
                Arguments = "--on a",
                Facets = new Dictionary<string, string>
                {
                    {"verb", "pause"},
                    {"service", "x"},
                    {"host", "a"},
                }
            });
            ActionViews.Add(new ActionView
            {
                Title = "Restart X on A",
                Command = "restart_x",
                Arguments = "--on a",
                Facets = new Dictionary<string, string>
                {
                    {"verb", "restart"},
                    {"service", "x"},
                    {"host", "a"},
                }
            });
            ActionViews.Add(new ActionView
            {
                Title = "Stop X on A",
                Command = "stop_x",
                Arguments = "--on a",
                Facets = new Dictionary<string, string>
                {
                    {"verb", "stop"},
                    {"service", "x"},
                    {"host", "a"},
                }
            });
            ActionViews.Add(new ActionView
            {
                Title = "Start Y on A",
                Command = "start_y",
                Arguments = "--on a",
                Facets = new Dictionary<string, string>
                {
                    {"verb", "start"},
                    {"service", "y"},
                    {"host", "a"},
                }
            });
            ActionViews.Add(new ActionView
            {
                Title = "Stop Y on A",
                Command = "stop_y",
                Arguments = "--on a",
                Facets = new Dictionary<string, string>
                {
                    {"verb", "stop"},
                    {"service", "y"},
                    {"host", "a"},
                }
            });
            CurrentPerspective = Perspectives[0];
            CurrentPerspective.CurrentSubset = CurrentPerspective.Subsets[0];

            MonitorViews.Add(new CommandMonitorView
            {
                Title = "Monitor A",
                Command = "xcopy",
                Arguments = "/?",
                Interval = new TimeSpan(0, 0, 4),
                HasLastExecutionResult = true,
                LastExecutionResult = true,
                LastExecutionTime = DateTime.Now,
            });
            MonitorViews.Add(new CommandMonitorView
            {
                Title = "Monitor B",
                Command = "git",
                Arguments = "--version",
                Interval = new TimeSpan(0, 0, 4),
                HasLastExecutionResult = true,
                LastExecutionResult = false,
                HasExecutionResultChanged = true,
                LastExecutionTime = DateTime.Now - TimeSpan.FromSeconds(10),
            });
            MonitorViews.Add(new CommandMonitorView
            {
                Title = "Very long monitor name with a lots of characters",
                Command = "invalid",
                HasLastExecutionResult = false,
                LastExecutionResult = false,
            });
        }
    }
}
