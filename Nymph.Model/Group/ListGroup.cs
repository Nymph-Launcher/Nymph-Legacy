using Nymph.Model.Item;

namespace Nymph.Model.Group;

public abstract record ListGroup : Group
{
    public abstract ListItem GetList();
}

public record ListGroup<T>(ListItem<T> List) : ListGroup where T : Item.Item
{
    public override ListItem GetList() => List;
}