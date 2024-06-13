using System.Collections.ObjectModel;
using LanguageExt;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.ItemViewModel;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Nymph.Shared.ViewModel.GroupViewModel;

public interface IStaticUnaryFunctionGroupViewModel
{
    ReadOnlyObservableCollection<CandidateItemViewModel> Items { get; }
    ReactiveCommand<Unit, Unit> ExecuteFunc { get; }
}