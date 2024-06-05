using System.Reactive;
using ReactiveUI;

namespace Nymph.Shared.ViewModel.ItemViewModel;



public class CandidateItemViewModel(ItemViewModel itemViewModel)
{
    public ItemViewModel ItemViewModel { get; } = itemViewModel;

    public ReactiveCommand<Unit, Unit> Choose { get; } = ReactiveCommand.Create(() => { });
    
    
}