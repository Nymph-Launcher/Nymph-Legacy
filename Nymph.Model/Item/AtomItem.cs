namespace Nymph.Model.Item;

public abstract record AtomItem : Item
{
    public abstract object GetValue();
}

public record AtomItem<TValue>(TValue Value) : AtomItem
{
    public override object GetValue() => Value;
}