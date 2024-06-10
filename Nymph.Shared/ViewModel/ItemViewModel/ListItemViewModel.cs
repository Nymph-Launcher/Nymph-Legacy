using System.Collections.ObjectModel;
using Nymph.Model.Item;

namespace Nymph.Shared.ViewModel.ItemViewModel;

public class ListItemViewModel<T>(ListItem<T> listItem) : ItemViewModel<ListItem<T>>(listItem)
    where T : Item
{
    public ReadOnlyObservableCollection<ItemViewModel<T>> Items => new(
        new ObservableCollection<ItemViewModel<T>>(
            Item.List.Select(item => new ItemViewModelBuilder().Build(item))
        ));
}