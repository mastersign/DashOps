using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastersign.DashOps
{
    sealed class DesignTimeProjectView : ProjectView
    {
        public DesignTimeProjectView()
        {
            FormatVersion = "2.0";
            Title = "Design Time Project View";
            AddFacettePerspectives(
                "Host",
                "Service",
                "Verb",
                "Environment",
                "Role");
            ActionViews.Add(new ActionView
            {
                Title = "Start X on A",
                Command = "start_x",
                Arguments ="--on a",
                Facettes = new Dictionary<string, string>
                {
                    {"Verb", "start"},
                    {"Service", "x"},
                    {"Host", "a"},
                }
            });
            ActionViews.Add(new ActionView
            {
                Title = "Stop X on A",
                Command = "stop_x",
                Arguments = "--on a",
                Facettes = new Dictionary<string, string>
                {
                    {"Verb", "stop"},
                    {"Service", "x"},
                    {"Host", "a"},
                }
            });
            ActionViews.Add(new ActionView
            {
                Title = "Start Y on A",
                Command = "start_y",
                Arguments = "--on a",
                Facettes = new Dictionary<string, string>
                {
                    {"Verb", "start"},
                    {"Service", "y"},
                    {"Host", "a"},
                }
            });
            ActionViews.Add(new ActionView
            {
                Title = "Stop Y on A",
                Command = "stop_y",
                Arguments = "--on a",
                Facettes = new Dictionary<string, string>
                {
                    {"Verb", "stop"},
                    {"Service", "y"},
                    {"Host", "a"},
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
