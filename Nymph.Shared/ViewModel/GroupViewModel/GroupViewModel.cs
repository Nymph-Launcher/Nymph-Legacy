using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Nymph.Model.Group;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.ItemViewModel;
using ReactiveUI;

namespace Nymph.Shared.ViewModel.GroupViewModel;

public abstract class GroupViewModel : ReactiveObject
{
    public abstract ReadOnlyObservableCollection<CandidateItemViewModel> Items { get; }
    
    public abstract IObservable<Item> ChosenItemViewModels { get; }
}

public abstract class GroupViewModel<T>(T group) : GroupViewModel
    where T : Group
{
    public T Group { get; } = group;
}