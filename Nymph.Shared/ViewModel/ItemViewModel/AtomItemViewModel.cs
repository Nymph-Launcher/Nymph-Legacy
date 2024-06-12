using Nymph.Model.Item;

namespace Nymph.Shared.ViewModel.ItemViewModel;

public class AtomItemViewModel<T>(AtomItem<T> atomItem) : ItemViewModel<AtomItem<T>>(atomItem), IAtomItemViewModel
{
    public T Value => Item.Value;

    public object GetValue => Value;
}