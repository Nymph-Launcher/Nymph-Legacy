using Nymph.Model.Item;

namespace Nymph.Model.Group;

public record ListGroup<T>(ListItem<T> List) : Group where T : Item.Item;