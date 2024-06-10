using System.Collections.ObjectModel;
using Nymph.Model.Group;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.ItemViewModel;

namespace Nymph.Shared.ViewModel.GroupViewModel;

public class
    BinaryFunctionGroupViewModel<TParam1, TParam2, TResult> : GroupViewModel<
    BinaryFunctionGroup<TParam1, TParam2, TResult>> where TParam1 : Item
    where TParam2 : AtomItem<string>
    where TResult : Item
{
    public override ReadOnlyObservableCollection<CandidateItemViewModel> Items { get; }
    public override IObservable<Item> ChosenItemViewModels { get; }

    public BinaryFunctionGroupViewModel(BinaryFunctionGroup<TParam1, TParam2, TResult> group) : base(group)
    {
        
    }
}