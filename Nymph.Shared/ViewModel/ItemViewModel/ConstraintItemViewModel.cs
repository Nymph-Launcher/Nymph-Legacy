using ReactiveUI;

namespace Nymph.Shared.ViewModel.ItemViewModel;

public class ConstraintItemViewModel(ItemViewModel itemViewModel) : ReactiveObject
{
    public ItemViewModel ItemViewModel { get; } = itemViewModel;
}