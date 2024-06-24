namespace Nymph.Model.Item;

internal record FunctionItem<TParam, TResult>(Func<TParam, TResult> Func)
    : Item where TParam : Item where TResult : Item;