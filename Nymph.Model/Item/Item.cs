using LanguageExt;
using static LanguageExt.Prelude;

namespace Nymph.Model.Item;

public record Item
{
    public static Item Atom<TValue>(TValue value) => new AtomItem<TValue>(value);

    public static Item List<TItem>(params TItem[] items) where TItem : Item => new ListItem<TItem>(Seq(items));
    
    public static Item List<TItem>(Seq<TItem> items) where TItem : Item => new ListItem<TItem>(items);
    
    public static Item Unit() => new UnitItem();

    public static Item Path<TDecorator, TValue>(TDecorator decorator, TValue value)
        where TDecorator : Item where TValue : Item => new PathItem<TDecorator, TValue>(decorator, value);

    public static Item Function<TParam, TResult>(Func<TParam, TResult> function)
        where TParam : Item where TResult : Item => new FunctionItem<TParam, TResult>(function);
    
    public static Item Record(Map<string, Item> fields) => new RecordItem(fields);
}