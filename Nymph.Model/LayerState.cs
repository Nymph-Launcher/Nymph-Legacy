using LanguageExt;

namespace Nymph.Model;

public record LayerState(Seq<Binding> Bindings, Option<Item.Item> Item, string Text);