using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastersign.DashOps
{
    partial class ProjectView
    {
        private void AddFacettePerspective(string facetteName)
        {
            Perspectives.Add(
                new Perspective(
                    title: facetteName + "s",
                    sourceActions: ActionViews,
                    filter: a => a.HasFacette(facetteName),
                    classifier: a => a.GetFacetteValue(facetteName)));
        }

        public void InitializeFacettePerspectives(params string[] facetteNames)
        {
            foreach (var facetteName in facetteNames)
            {
                AddFacettePerspective(facetteName);
            }
        }
    }
}
