using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastersign.DashOps
{
    partial class ProjectView
    {
        private void Initialize()
        {
            this.CurrentPerspectiveChanged += CurrentPerspectiveChangedHandler;
        }

        private void CurrentPerspectiveChangedHandler(object sender, EventArgs e)
        {
            foreach (var p in Perspectives)
            {
                p.IsSelected = p == CurrentPerspective;
            }
        }

        private void AddFacettePerspective(string facetteName)
        {
            Perspectives.Add(
                new PerspectiveView(
                    title: facetteName + "s",
                    sourceActions: ActionViews,
                    filter: a => a.HasFacette(facetteName),
                    classifier: a => new[] { a.GetFacetteValue(facetteName) }));
        }

        public void AddFacettePerspectives(params string[] facetteNames)
        {
            foreach (var facetteName in facetteNames)
            {
                AddFacettePerspective(facetteName);
            }
        }

        public void AddTagsPerspective()
        {
            Perspectives.Add(
                new PerspectiveView(
                    title: "Tags",
                    sourceActions: ActionViews,
                    filter: a => (a.Tags?.Length ?? 0) > 0,
                    classifier: a => a.Tags));
        }

        public string WindowTitle => "DashOps - " + Title;

        public bool HasMonitors => MonitorViews.Count > 0;
    }
}
