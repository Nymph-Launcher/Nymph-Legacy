using LanguageExt;
using Nymph.Model.Item;

namespace Nymph.Model.Group;

public abstract record UnaryFunctionGroup : Group
{
    public abstract Task<Seq<Item.Item>> GetResult();
}

public abstract record UnaryFunctionGroup<TResult> : UnaryFunctionGroup
    where TResult : Item.Item
{
    public abstract Task<Seq<TResult>> GetSpecificResult();
}

public record UnaryFunctionGroup<TParam, TResult>(
    FunctionItem<TParam, TResult> UnaryFunction,
    TParam Param) : UnaryFunctionGroup<TResult>
    where TParam : Item.Item
    where TResult : Item.Item
{
    public override Task<Seq<TResult>> GetSpecificResult()
    {
        return UnaryFunction.Func(Param);
    }

    public override Task<Seq<Item.Item>> GetResult()
    {
        return GetSpecificResult().Map(seq => seq.Map(item => item as Item.Item));
    }
}