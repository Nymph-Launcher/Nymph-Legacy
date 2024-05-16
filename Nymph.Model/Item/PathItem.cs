namespace Nymph.Model.Item;

public abstract record PathItem : Item
{
    public abstract Item GetDecorator();

    public abstract Item GetItem();
}

public record PathItem<TDecorator, TItem>(TDecorator Decorator, TItem Item)
    : PathItem
    where TDecorator : Item
    where TItem : Item
{
    public override Item GetDecorator() => Decorator;

    public override Item GetItem() => Item;
}