using LanguageExt;
using Nymph.Model.Item;

namespace Nymph.Model.Group;

public abstract record DynamicUnaryFunctionGroup : Group
{
    public abstract Task<Seq<Item.Item>> GetResult(AtomItem<string> param);
}

public record DynamicUnaryFunctionGroup<TResult>(
    FunctionItem<AtomItem<string>, TResult> UnaryFunction) : DynamicUnaryFunctionGroup
    where TResult : Item.Item
{
    public Task<Seq<TResult>> GetSpecificResult(AtomItem<string> param)
    {
        return UnaryFunction.Func(param);
    }

    public override Task<Seq<Item.Item>> GetResult(AtomItem<string> param)
    {
        return GetSpecificResult(param).Map(seq => seq.Map(item => item as Item.Item));
    }
}