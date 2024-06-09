using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using Nymph.Model.Group;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.ItemViewModel;

namespace Nymph.Shared.ViewModel.GroupViewModel;

using ItemViewModel = ItemViewModel.ItemViewModel;

public class ItemGroupViewModel<T> : GroupViewModel<ItemGroup<T>>
    where T : Model.Item.Item
{
    private readonly SourceList<CandidateItemViewModel> _candidates = new();
    
    public ItemGroupViewModel(ItemGroup<T> group) : base(group)
    {
        _candidates.Add(new CandidateItemViewModel(new ItemViewModelBuilder<T>(Group.Item).Build()));

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
        
        Preview = new(group.Item);
    }

    public override ReadOnlyObservableCollection<CandidateItemViewModel> Items { get; }
    
    public override IObservable<Item> ChosenItemViewModels { get; }

    public ItemPreviewViewModel<T> Preview { get; }
}