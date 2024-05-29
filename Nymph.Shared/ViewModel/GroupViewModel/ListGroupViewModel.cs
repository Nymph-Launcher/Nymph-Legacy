using System.Collections.ObjectModel;
using Nymph.Model.Group;
using Nymph.Shared.ViewModel.ItemViewModel;

namespace Nymph.Shared.ViewModel.GroupViewModel;

using ItemViewModel = ItemViewModel.ItemViewModel;

public class ListGroupViewModel<T>(ListGroup<T> group) : GroupViewModel<ListGroup<T>>(group)
    where T : Model.Item.Item
{
    public override ReadOnlyObservableCollection<ItemViewModel> Items => new(
        new ObservableCollection<ItemViewModel>(Group.List.List.Select(item => new ItemViewModel<T>(item) as ItemViewModel))
    );
}