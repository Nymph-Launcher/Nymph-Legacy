using LanguageExt;

namespace Nymph.Model.Item;

public record ListItem<TItem>(Seq<TItem> List) : Item where TItem : Item;