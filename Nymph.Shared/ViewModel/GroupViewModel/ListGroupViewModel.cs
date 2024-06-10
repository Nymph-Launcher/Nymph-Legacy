using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using Nymph.Model.Group;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.ItemViewModel;

namespace Nymph.Shared.ViewModel.GroupViewModel;

public class ListGroupViewModel<T> : GroupViewModel<ListGroup<T>>
    where T : Model.Item.Item
{
    public override IObservable<Item> ChosenItemViewModels { get; }
    
    private readonly SourceList<CandidateItemViewModel> _candidates = new();
    
    public override ReadOnlyObservableCollection<CandidateItemViewModel> Items { get; }
    
    public ListGroupViewModel(ListGroup<T> group) : base(group)
    {
        _candidates.AddRange(group.List.List.Select(item => new CandidateItemViewModel(new ItemViewModelBuilder().Build(item))));

        _candidates
            .Connect()
            .Bind(out var candidates)
            .Subscribe();
        Items = candidates;

        ChosenItemViewModels = _candidates
            .Connect()
            .AutoRefresh()
            .MergeMany(candidate =>
                candidate.Choose
                    .Select(_ => candidate.ItemViewModel))
            .Select(itemvm => itemvm.GetItem)
            .Publish()
            .RefCount();
    }
}