using Nymph.Model.Item;

namespace Nymph.Model.Group;

public abstract record UnaryFunctionGroup : Group;

public record UnaryFunctionGroup<TParam, TResult>(
    FunctionItem<TParam, TResult> UnaryFunction) : UnaryFunctionGroup
    where TParam : Item.Item
    where TResult : Item.Item;