namespace Nymph.Model.Item;

public record PathItem<TDecorator, TItem>(TDecorator Decorator, TItem Item) : Item where TDecorator : Item where TItem : Item;