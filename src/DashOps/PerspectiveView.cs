using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastersign.DashOps
{
    partial class PerspectiveView
    {
        private void Initialize()
        {
            this.SourceActions.CollectionChanged += SourceActionsCollectionChangedHandler;
            UpdateSubsets();
        }

        private void SourceActionsCollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateSubsets();
        }

        private void UpdateSubsets()
        {
            var classes = this.SourceActions.Where(Filter).SelectMany(Classifier).Distinct().OrderBy(c => c);
            this.Subsets.Clear();
            foreach (var cls in classes)
            {
                var subset = new ActionSubset(cls);
                foreach (var a in this.SourceActions.Where(Filter).Where(a => Classifier(a).Contains(cls)))
                {
                    subset.Actions.Add(a);
                }
                this.Subsets.Add(subset);
            }
        }

        public override string ToString() => Title;
    }
}
