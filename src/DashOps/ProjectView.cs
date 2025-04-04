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

        public void AddFacettePerspective(string facetteName, string caption = null)
        {
            Perspectives.Add(
                new PerspectiveView(
                    title: caption ?? facetteName + "s",
                    facette: facetteName,
                    sourceActions: ActionViews,
                    filter: a => a.HasFacette(facetteName),
                    classifier: a => [a.GetFacetteValue(facetteName)]));
        }

        public void AddTagsPerspective()
        {
            Perspectives.Add(
                new PerspectiveView(
                    title: "Tags",
                    facette: null,
                    sourceActions: ActionViews,
                    filter: a => (a.Tags?.Length ?? 0) > 0,
                    classifier: a => a.Tags));
        }

        public string WindowTitle => Properties.Resources.Common.WindowTitle + " - " + Title;

        public bool HasMonitors => MonitorViews.Count > 0;
    }
}
