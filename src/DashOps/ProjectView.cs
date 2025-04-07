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

        public void AddFacetPerspective(string facetName, string caption = null)
        {
            Perspectives.Add(
                new PerspectiveView(
                    title: caption ?? facetName + "s",
                    facet: facetName,
                    sourceActions: ActionViews,
                    filter: a => a.HasFacet(facetName),
                    classifier: a => [a.GetFacetValue(facetName)]));
        }

        public void AddTagsPerspective()
        {
            Perspectives.Add(
                new PerspectiveView(
                    title: "Tags",
                    facet: null,
                    sourceActions: ActionViews,
                    filter: a => (a.Tags?.Length ?? 0) > 0,
                    classifier: a => a.Tags));
        }

        public string WindowTitle => Properties.Resources.Common.WindowTitle + " - " + Title;

        public bool HasMonitors => MonitorViews.Count > 0;
    }
}
