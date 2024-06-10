using System.Collections.ObjectModel;
using Nymph.Model.Group;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.ItemViewModel;

namespace Nymph.Shared.ViewModel.GroupViewModel;

public class DynamicUnaryFunctionGroupViewModel<TResult> : GroupViewModel<DynamicUnaryFunctionGroup<TResult>>
    where TResult : Item
{
    public override ReadOnlyObservableCollection<CandidateItemViewModel> Items { get; }
    public override IObservable<Item> ChosenItemViewModels { get; }

    public DynamicUnaryFunctionGroupViewModel(DynamicUnaryFunctionGroup<TResult> group) : base(group)
    {
        
    }
}