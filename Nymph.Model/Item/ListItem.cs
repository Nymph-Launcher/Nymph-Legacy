using LanguageExt;

namespace Nymph.Model.Item;

public abstract record ListItem : Item
{
    public abstract Seq<Item> GetList();
}

public record ListItem<TItem>(Seq<TItem> List) : ListItem where TItem : Item
{
    public override Seq<Item> GetList() => List.Map(item => item as Item);
}