using System.Reactive;
using Nymph.Model.Helper;
using Nymph.Model.Item;
using ReactiveUI;

namespace Nymph.Shared.ViewModel.ItemViewModel;

public abstract class ItemViewModel : ReactiveObject
{
    public abstract Item GetItem { get; }
}

public abstract class ItemViewModel<T>(T item) : ItemViewModel
    where T : Item
{
    public override Item GetItem => Item;
    
    public T Item { get; } = item;
}