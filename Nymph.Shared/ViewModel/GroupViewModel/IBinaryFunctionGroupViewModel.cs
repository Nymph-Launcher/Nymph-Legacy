using System.Collections.ObjectModel;
using Nymph.Shared.ViewModel.ItemViewModel;

namespace Nymph.Shared.ViewModel.GroupViewModel;

public interface IBinaryFunctionGroupViewModel
{
    bool AutoOperation { get; set; }
    ReadOnlyObservableCollection<CandidateItemViewModel> Items { get; }
}