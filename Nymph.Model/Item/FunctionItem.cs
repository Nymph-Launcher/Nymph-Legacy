using LanguageExt;

namespace Nymph.Model.Item;

public record FunctionItem<TParam, TResult>(Func<TParam, Task<Seq<TResult>>> Func) : Item where TParam : Item where TResult : Item;