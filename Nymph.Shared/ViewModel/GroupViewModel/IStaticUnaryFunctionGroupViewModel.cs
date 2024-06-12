using System.Collections.ObjectModel;
using LanguageExt;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.ItemViewModel;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Nymph.Shared.ViewModel.GroupViewModel;

public interface IStaticUnaryFunctionGroupViewModel<TResult> where TResult : Item
{
    ReadOnlyObservableCollection<CandidateItemViewModel> Items { get; }
    ReactiveCommand<Unit, Seq<TResult>> ExecuteFunc { get; }
}