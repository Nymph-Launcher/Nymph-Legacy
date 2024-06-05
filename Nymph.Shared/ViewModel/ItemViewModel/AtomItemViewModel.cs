using Nymph.Model.Item;

namespace Nymph.Shared.ViewModel.ItemViewModel;

public class AtomItemViewModel<T>(AtomItem<T> atomItem) : ItemViewModel<AtomItem<T>>(atomItem)
{
    public T Value => Item.Value;
}