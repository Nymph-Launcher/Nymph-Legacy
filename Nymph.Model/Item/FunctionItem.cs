using LanguageExt;

namespace Nymph.Model.Item;

public abstract record FunctionItem(string Description) : Item
{
    public abstract Func<Item, Option<Task<Seq<Item>>>> GetFunc();
}

public record FunctionItem<TParam, TResult>(Func<TParam, Task<Seq<TResult>>> Func, string Description = "Unnamed Function")
    : FunctionItem(Description)
    where TParam : Item
    where TResult : Item
{
    public override Func<Item, Option<Task<Seq<Item>>>> GetFunc() => item =>
    {
        if (item is TParam param)
        {
            return Option<Task<Seq<Item>>>.Some(Func(param).Map(seq => seq.Map(i => i as Item)));
        }
        return Option<Task<Seq<Item>>>.None;
    };
}