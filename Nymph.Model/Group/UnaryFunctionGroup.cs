using LanguageExt;
using Nymph.Model.Item;

namespace Nymph.Model.Group;

public abstract record UnaryFunctionGroup : Group
{
    public abstract Option<Task<Seq<Item.Item>>> GetResult(Item.Item item);
}

public abstract record UnaryFunctionGroup<TResult> : UnaryFunctionGroup
    where TResult : Item.Item
{
    public abstract Option<Task<Seq<TResult>>> GetSpecificResult(Item.Item item);
}

public record UnaryFunctionGroup<TParam, TResult>(
    FunctionItem<TParam, TResult> UnaryFunction) : UnaryFunctionGroup<TResult>
    where TParam : Item.Item
    where TResult : Item.Item
{
    public override Option<Task<Seq<TResult>>> GetSpecificResult(Item.Item item)
    {
        return (item is TParam specificParam ? Option<TParam>.Some(specificParam) : Option<TParam>.None)
            .Map(param => UnaryFunction.Func(param));
    }

    public override Option<Task<Seq<Item.Item>>> GetResult(Item.Item item)
    {
        return GetSpecificResult(item)
            .Map<Task<Seq<Item.Item>>>(task => task.Map(seq => seq.Map(item => item as Item.Item)));
    }
}