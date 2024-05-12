using Nymph.Model.Item;

namespace Nymph.Model.Group;

public record UnaryFunctionGroup<TParam, TResult>(
    FunctionItem<TParam, TResult> UnaryFunction) : Group
    where TParam : Item.Item
    where TResult : Item.Item;