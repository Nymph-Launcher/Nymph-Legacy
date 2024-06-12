using Nymph.Model.Item;

namespace Nymph.Shared.ViewModel.ItemViewModel;

public class PathItemViewModel<TDecorator, TItem>(PathItem<TDecorator, TItem> pathItem)
    : ItemViewModel<PathItem<TDecorator, TItem>>(pathItem), IPathItemViewModel
    where TDecorator : Item
    where TItem : Item
{
    private readonly PathItem<TDecorator, TItem> _pathItem = pathItem;
    
    public ItemViewModel<TItem> DecoratedItem => new ItemViewModelBuilder().Build(_pathItem.Item);
    
    public ItemViewModel<TDecorator> Decorator => new ItemViewModelBuilder().Build(_pathItem.Decorator);
    
    public ItemViewModel GetDecoratedItem => DecoratedItem;

    public ItemViewModel GetDecorator => Decorator;
}