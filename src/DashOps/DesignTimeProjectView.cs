using Mastersign.DashOps.Model;
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
            Title = "Design Time Project View";
            AddFacettePerspectives(
                nameof(CommandAction.Host),
                nameof(CommandAction.Service),
                nameof(CommandAction.Verb),
                "Environment",
                "Role");
            ActionViews.Add(new ActionView
            {
                Description = "Start X on A",
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
                Description = "Stop X on A",
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
                Description = "Start Y on A",
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
                Description = "Stop Y on A",
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
        }
    }
}
