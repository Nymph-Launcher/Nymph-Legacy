using LanguageExt;
using static LanguageExt.Prelude;
using Nymph.Model.Item;

namespace Nymph.Model.Group;

public abstract record BinaryFunctionGroup : Group
{
    public abstract Option<Task<Seq<Item.Item>>> GetFinalResult(AtomItem<string> param2);
}

public abstract record BinaryFunctionGroup<TResult> : BinaryFunctionGroup
    where TResult : Item.Item
{
    public abstract Option<Task<Seq<TResult>>> GetSpecificFinalResult(AtomItem<string> param2);
}

public abstract record BinaryFunctionGroup<TParam2, TResult> : BinaryFunctionGroup<TResult>
    where TParam2 : AtomItem<string>
    where TResult : Item.Item
{
    public abstract Task<Seq<FunctionItem<TParam2, TResult>>> GetUnaryFunctionItem();
}

public record BinaryFunctionGroup<TParam1, TParam2, TResult>(
    FunctionItem<TParam1, FunctionItem<TParam2, TResult>> BinaryFunction,
    TParam1 Param1) : BinaryFunctionGroup<TParam2, TResult>
    where TParam1 : Item.Item
    where TParam2 : AtomItem<string>
    where TResult : Item.Item
{
    public override Option<Task<Seq<Item.Item>>> GetFinalResult(AtomItem<string> atomItem)
    {
        return GetSpecificFinalResult(atomItem)
            .Map<Task<Seq<Item.Item>>>(task => task
                .Map(seq => seq
                    .Map(item => item as Item.Item)));
    }

    public override Option<Task<Seq<TResult>>> GetSpecificFinalResult(AtomItem<string> param2)
    {
        return (param2 is TParam2 specificParam2 ? Some(specificParam2) : None)
            .Map(param2 => GetUnaryFunctionItem()
                .Map(seq => seq
                    .Map(funcItem => funcItem.Func))
                .MapAsync(async functions =>
                {
                    var results = new Seq<TResult>();
                    foreach (var function1 in functions)
                    {
                        var resultSeq = await function1(param2);
                        results += resultSeq;
                    }
                    return results;
                }));
    }
    public override Task<Seq<FunctionItem<TParam2, TResult>>> GetUnaryFunctionItem()
    {
        return BinaryFunction.Func(Param1);
    }
}