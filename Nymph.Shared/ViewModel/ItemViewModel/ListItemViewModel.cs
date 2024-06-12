using System.Collections.ObjectModel;
using Nymph.Model.Item;

namespace Nymph.Shared.ViewModel.ItemViewModel;

public class ListItemViewModel<T>(ListItem<T> listItem) : ItemViewModel<ListItem<T>>(listItem), IListItemViewModel
    where T : Item
{
    public ReadOnlyCollection<ItemViewModel<T>> Items => new(
        Item.List.Select(item => new ItemViewModelBuilder().Build(item)).ToList());

    public ReadOnlyCollection<ItemViewModel> GetItems => Items.Cast<ItemViewModel>().ToList().AsReadOnly();
}