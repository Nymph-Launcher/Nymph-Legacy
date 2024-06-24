using LanguageExt;

namespace Nymph.Model.Item;

internal record RecordItem(Map<string, Item> Fields) : Item;