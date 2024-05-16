namespace Nymph.Model.Group;

public abstract record ItemGroup : Group
{
    public abstract Item.Item GetItem();
}

public record ItemGroup<T>(T Item) : ItemGroup where T : Item.Item
{
    public override Item.Item GetItem() => Item;
}