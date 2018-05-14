using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastersign.DashOps
{
    partial class PerspectiveView : IDisposable
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

        public void Dispose()
        {
            this.SourceActions.CollectionChanged -= SourceActionsCollectionChangedHandler;
        }

        private void UpdateSubsets()
        {
            var classes = this.SourceActions.Where(Filter).SelectMany(Classifier).Distinct().OrderBy(c => c);
            this.Subsets.Clear();
            foreach (var cls in classes)
            {
                var actions = new ObservableCollection<ActionView>(
                    SourceActions.Where(Filter)
                        .Where(a => Classifier(a).Contains(cls))
                        .OrderBy(a => a.Title));
                var subset = new ActionSubset(cls) { Actions = actions };
                this.Subsets.Add(subset);
            }
        }

        public override string ToString() => Title;
    }
}
