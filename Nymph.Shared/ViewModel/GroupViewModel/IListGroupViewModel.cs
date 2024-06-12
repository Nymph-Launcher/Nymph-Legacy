using System.Collections.ObjectModel;
using Nymph.Shared.ViewModel.ItemViewModel;

namespace Nymph.Shared.ViewModel.GroupViewModel;

public interface IListGroupViewModel
{
    ReadOnlyObservableCollection<CandidateItemViewModel> Items { get; }
}