using System.Collections.ObjectModel;
using Nymph.Shared.ViewModel.ItemViewModel;

namespace Nymph.Shared.ViewModel.GroupViewModel;

public interface IItemGroupViewModel
{
    ReadOnlyObservableCollection<CandidateItemViewModel> Items { get; }
    
    IItemPreviewViewModel Preview { get; }
}