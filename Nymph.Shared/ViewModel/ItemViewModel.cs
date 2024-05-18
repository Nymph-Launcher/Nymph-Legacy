using System.Reactive;
using ReactiveUI;

namespace Nymph.Shared.ViewModel;

public abstract class ItemViewModel : ReactiveObject
{
    public abstract Model.Item.Item GetItem { get; }
}

public class ItemViewModel<T>(T item) : ItemViewModel
    where T : Model.Item.Item
{
    public override Model.Item.Item GetItem => Item;
    
    public T Item { get; } = item;

    public ReactiveCommand<Unit, Unit> Choose { get; } = ReactiveCommand.Create(() => { });
}