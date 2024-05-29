using Nymph.Model.Item;
using ReactiveUI;

namespace Nymph.Shared.ViewModel.GroupViewModel;

public class ItemPreviewViewModel<T>(T item) : ReactiveObject
    where T : Item
{
    public T Item { get; } = item;
}