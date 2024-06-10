using System.Collections.ObjectModel;
using Nymph.Model.Group;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.ItemViewModel;

namespace Nymph.Shared.ViewModel.GroupViewModel;

public class StaticUnaryFunctionGroupViewModel<TParam, TResult> : GroupViewModel<StaticUnaryFunctionGroup<TParam, TResult>> where TParam : Item
    where TResult : Item
{
    public override ReadOnlyObservableCollection<CandidateItemViewModel> Items { get; }
    public override IObservable<Item> ChosenItemViewModels { get; }

    public StaticUnaryFunctionGroupViewModel(StaticUnaryFunctionGroup<TParam, TResult> group) : base(group)
    {
        
    }
}