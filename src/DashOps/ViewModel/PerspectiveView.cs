using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Mastersign.DashOps.ViewModel;

partial class PerspectiveView : IDisposable
{
    private void Initialize()
    {
        SourceActions.CollectionChanged += SourceActionsCollectionChangedHandler;
        UpdateSubsets();
    }

    private void SourceActionsCollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateSubsets();
    }

    public void Dispose()
    {
        SourceActions.CollectionChanged -= SourceActionsCollectionChangedHandler;
    }

    private void UpdateSubsets()
    {
        var classes = SourceActions.Where(Filter).SelectMany(Classifier).Distinct().OrderBy(c => c);
        Subsets.Clear();
        foreach (var cls in classes)
        {
            var actions = new ObservableCollection<ActionView>(
                SourceActions.Where(Filter)
                    .Where(a => Classifier(a).Contains(cls))
                    .OrderBy(a => a.Title));
            var subset = new ActionSubset(cls) { Actions = actions };
            Subsets.Add(subset);
        }
    }

    public override string ToString() => Title;
}
