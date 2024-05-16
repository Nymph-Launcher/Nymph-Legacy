using System.Collections.ObjectModel;
using Nymph.Shared.ViewModel.Item;

namespace Nymph.Shared.ViewModel.Group;

public abstract class GroupViewModel
{
    public abstract ReadOnlyObservableCollection<ItemViewModel> Items { get; }
}