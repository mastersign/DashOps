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

        public PerspectiveView AddFacetPerspective(string facetName, string caption = null)
        {
            var perspective = new PerspectiveView(
                    title: caption ?? facetName + "s",
                    facet: facetName,
                    sourceActions: ActionViews,
                    filter: a => a.HasFacet(facetName),
                    classifier: a => [a.GetFacetValue(facetName)]);
            Perspectives.Add(perspective);
            return perspective;
        }

        public PerspectiveView AddTagsPerspective()
        {
            var perspective = new PerspectiveView(
                    title: "Tags",
                    facet: null,
                    sourceActions: ActionViews,
                    filter: a => (a.Tags?.Length ?? 0) > 0,
                    classifier: a => a.Tags);
            Perspectives.Add(perspective);
            return perspective;
        }

        public string WindowTitle => Title + " - " + Properties.Resources.Common.WindowTitle;

        public event EventHandler ProjectUpdated;

        public void NotifyProjectUpdated() => ProjectUpdated?.Invoke(this, EventArgs.Empty);
    }
}
