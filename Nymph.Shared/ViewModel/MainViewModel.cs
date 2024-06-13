using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using DynamicData;
using Nymph.Model.Strategy;
using Nymph.Shared.ViewModel.ItemViewModel;
using ReactiveUI;
using LanguageExt;
using Nymph.Model;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.GroupViewModel;
using static LanguageExt.Prelude;

namespace Nymph.Shared.ViewModel;

using GroupVM = GroupViewModel.GroupViewModel;

public class MainViewModel : ReactiveObject
{
    private IStrategy _strategy;
    
    private readonly ObservableAsPropertyHelper<Option<ConstraintItemViewModel>> _constraintItemViewModel;
    public Option<ConstraintItemViewModel> ConstraintItemViewModel => _constraintItemViewModel.Value;

    private string _searchText;
    public string SearchText
    {
        get => _searchText;
        set => this.RaiseAndSetIfChanged(ref _searchText, value);
    }

    private SourceList<GroupVM> _groupViewModels;
    public ReadOnlyObservableCollection<GroupVM> GroupViewModels { get; }

    private readonly SourceList<Binding> _bindings;

    public MainViewModel(IEnumerable<Binding> bindings, IStrategy strategy)
    {
        _groupViewModels = new SourceList<GroupVM>();
        _bindings = new SourceList<Binding>();
        _strategy = strategy;
        
        _bindings.AddRange(bindings);
        
        _groupViewModels
            .Connect()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out var groupViewModels)
            .Subscribe();
        GroupViewModels = groupViewModels;
        
        var searchTextObservable = this.WhenAnyValue(x => x.SearchText)
            .Select(str => new AtomItem<string>(str))
            .Publish()
            .RefCount();
        
        var groupViewModelBuilder = new GroupViewModelBuilder(searchTextObservable);
        
        var constraintItemObservable = _groupViewModels
            .Connect()
            .AutoRefresh()
            .MergeMany(groupVm => groupVm.ChosenItemViewModels)
            .Select(item => new ConstraintItemViewModel(new ItemViewModelBuilder().Build(item)))
            .Select(Some)
            .StartWith(Option<ConstraintItemViewModel>.None);
        
        _constraintItemViewModel = constraintItemObservable
            .ToProperty(this, x => x.ConstraintItemViewModel);
        
        var layerStateObservable = constraintItemObservable
            .Select(constraintItemViewModel => constraintItemViewModel.Map(vm => vm.ItemViewModel.GetItem))
            .CombineLatest(
                searchTextObservable.DistinctUntilChanged(atomItemString => string.IsNullOrWhiteSpace(atomItemString.Value)),
                _bindings.Connect().ToCollection(),
                (constraintItem, searchText, bds) =>
                    new LayerState(new Seq<Binding>(bds), constraintItem, searchText.Value));

        layerStateObservable
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(state =>
        {
            _groupViewModels.Edit(groupVMs =>
            {
                groupVMs.Clear();
                groupVMs.AddRange(_strategy.GetGroups(state).Map(group => groupViewModelBuilder.Build(group)));
            });
        });
    }
}