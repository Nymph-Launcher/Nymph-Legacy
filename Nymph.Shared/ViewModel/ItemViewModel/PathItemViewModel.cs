using Nymph.Model.Item;

namespace Nymph.Shared.ViewModel.ItemViewModel;

public class PathItemViewModel<TDecorator, TItem>(PathItem<TDecorator, TItem> pathItem)
    : ItemViewModel<PathItem<TDecorator, TItem>>(pathItem)
    where TDecorator : Item
    where TItem : Item
{
    private readonly PathItem<TDecorator, TItem> _pathItem = pathItem;
    
    public ItemViewModel<TItem> DecoratedItem => new ItemViewModelBuilder<TItem>(_pathItem.Item).Build();
    
    public ItemViewModel<TDecorator> Decorator => new ItemViewModelBuilder<TDecorator>(_pathItem.Decorator).Build();
}