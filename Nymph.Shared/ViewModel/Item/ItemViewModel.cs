using ReactiveUI;

namespace Nymph.Shared.ViewModel.Item;

public abstract class ItemViewModel
{
    public abstract ObservableAsPropertyHelper<Model.Item.Item> Item { get; }
}