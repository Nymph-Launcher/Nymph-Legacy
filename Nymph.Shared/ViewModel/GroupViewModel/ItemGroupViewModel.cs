using System.Collections.ObjectModel;
using Nymph.Model.Group;
using Nymph.Shared.ViewModel.ItemViewModel;

namespace Nymph.Shared.ViewModel.GroupViewModel;

using ItemViewModel = ItemViewModel.ItemViewModel;

public class ItemGroupViewModel<T>(ItemGroup<T> group) : GroupViewModel<ItemGroup<T>>(group)
    where T : Model.Item.Item
{
    public override ReadOnlyObservableCollection<CandidateItemViewModel> Items => new([new CandidateItemViewModel(new ItemViewModel<T>(Group.Item))]);

    public ItemPreviewViewModel<T> Preview { get; } = new(group.Item);
}