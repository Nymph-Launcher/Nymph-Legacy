using Nymph.Model.Item;

namespace Nymph.Shared.ViewModel.ItemViewModel;

public class FunctionItemViewModel<TParam, TResult>(FunctionItem<TParam, TResult> functionItem) : ItemViewModel<FunctionItem<TParam, TResult>>(functionItem)
    where TParam : Item
    where TResult : Item;