using LanguageExt;

namespace Nymph.Model.Item;

public record RecordItem(Seq<Tuple<string, Item>> Properties) : Item;