namespace Nymph.Model.Item;

internal record PathItem<TDecorator, TValue>(TDecorator Decorator, TValue Value)
    : Item where TDecorator : Item where TValue : Item;