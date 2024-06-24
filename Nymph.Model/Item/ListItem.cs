using LanguageExt;

namespace Nymph.Model.Item;

internal record ListItem<TItem>(Seq<TItem> Items) : Item where TItem : Item;