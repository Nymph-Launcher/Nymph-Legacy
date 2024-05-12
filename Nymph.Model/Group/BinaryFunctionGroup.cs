using Nymph.Model.Item;

namespace Nymph.Model.Group;

public record BinaryFunctionGroup<TParam1, TParam2, TResult>(
    FunctionItem<TParam1, FunctionItem<TParam2, TResult>> BinaryFunction) : Group
    where TParam1 : Item.Item
    where TParam2 : AtomItem<string>
    where TResult : Item.Item;