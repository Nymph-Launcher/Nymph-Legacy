namespace Nymph.Model.Group;

public record ItemGroup<T>(T Item) : Group where T : Item.Item;