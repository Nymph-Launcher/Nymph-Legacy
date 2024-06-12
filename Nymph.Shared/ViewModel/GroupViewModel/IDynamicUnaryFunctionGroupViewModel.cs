using System.Collections.ObjectModel;
using Nymph.Shared.ViewModel.ItemViewModel;

namespace Nymph.Shared.ViewModel.GroupViewModel;

public interface IDynamicUnaryFunctionGroupViewModel
{
    ReadOnlyObservableCollection<CandidateItemViewModel> Items { get; }
    bool IsAutoExecute { get; set; }
}