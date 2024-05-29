using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Nymph.Model.Group;
using ReactiveUI;

namespace Nymph.Shared.ViewModel.GroupViewModel;

using ItemViewModel = ItemViewModel.ItemViewModel;

public abstract class GroupViewModel : ReactiveObject
{
    public abstract ReadOnlyObservableCollection<ItemViewModel> Items { get; }
}

public abstract class GroupViewModel<T>(T group) : GroupViewModel
    where T : Group
{
    public T Group { get; } = group;
}