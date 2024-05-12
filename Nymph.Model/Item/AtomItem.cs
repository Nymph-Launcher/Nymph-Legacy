namespace Nymph.Model.Item;

public record AtomItem<TValue>(TValue Value) : Item;