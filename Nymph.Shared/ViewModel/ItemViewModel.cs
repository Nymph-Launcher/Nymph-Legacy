using System.Collections.ObjectModel;
using System.Reactive;
using Nymph.Model.Item;
using ReactiveUI;

namespace Nymph.Shared.ViewModel;

public abstract class ItemViewModel : ReactiveObject
{
    public abstract Item GetItem { get; }
}

public class ItemViewModel<T>(T item) : ItemViewModel
    where T : Item
{
    public override Item GetItem => Item;
    
    public T Item { get; } = item;

    public ReactiveCommand<Unit, Unit> Choose { get; } = ReactiveCommand.Create(() => { });
}

public class AtomItemViewModel<T>(AtomItem<T> atomItem) : ItemViewModel<AtomItem<T>>(atomItem)
{
    public T Value => Item.Value;
}

public class ListItemViewModel<T>(ListItem<T> listItem) : ItemViewModel<ListItem<T>>(listItem)
    where T : Item
{
    public ReadOnlyObservableCollection<ItemViewModel<T>> Items => new(
        new ObservableCollection<ItemViewModel<T>>(
            Item.List.Select(item => new ItemViewModel<T>(item))
        ));
}

public class FunctionItemViewModel<TParam, TResult>(FunctionItem<TParam, TResult> functionItem) : ItemViewModel<FunctionItem<TParam, TResult>>(functionItem)
    where TParam : Item
    where TResult : Item;
    
public class RecordItemViewModel<T>(T recordItem) : ItemViewModel<T>(recordItem)
    where T : RecordItem;

public class PathItemViewModel<TDecorator, TItem>(PathItem<TDecorator, TItem> pathItem)
    : ItemViewModel<PathItem<TDecorator, TItem>>(pathItem)
    where TDecorator : Item
    where TItem : Item
{
    public ItemViewModel<TItem> DecoratedItem => new(pathItem.Item);
}