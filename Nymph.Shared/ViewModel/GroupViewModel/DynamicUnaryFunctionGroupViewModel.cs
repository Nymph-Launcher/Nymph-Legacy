using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using LanguageExt;
using Nymph.Model.Group;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.ItemViewModel;
using ReactiveUI;

namespace Nymph.Shared.ViewModel.GroupViewModel;

public class DynamicUnaryFunctionGroupViewModel<TResult> : GroupViewModel<DynamicUnaryFunctionGroup<TResult>>, IDynamicUnaryFunctionGroupViewModel where TResult : Item
{
    public readonly SourceList<CandidateItemViewModel> _candidates = new();
    public override ReadOnlyObservableCollection<CandidateItemViewModel> Items { get; }
    public override IObservable<Item> ChosenItemViewModels { get; }
    private bool _isAutoExecute;

    public bool IsAutoExecute
    {
        get => _isAutoExecute;
        set => this.RaiseAndSetIfChanged(ref _isAutoExecute, value);
    }

    public string Description => Group.UnaryFunction.Description;

    public DynamicUnaryFunctionGroupViewModel(DynamicUnaryFunctionGroup<TResult> group, IObservable<AtomItem<string>> text) : base(group)
    {
        _candidates.Add(new CandidateItemViewModel(new ItemViewModelBuilder().Build(group.UnaryFunction)));

        _candidates
            .Connect()
            .Bind(out var candidates)
            .Subscribe();
        Items = candidates;

        ChosenItemViewModels = _candidates
            .Connect()
            .AutoRefresh()
            .MergeMany(candidate => candidate.Choose.Select(_ => candidate.ItemViewModel))
            .Select(vm => vm.GetItem)
            .Publish()
            .RefCount();

        var autoExecuteObservable = this.WhenAnyValue(x => x.IsAutoExecute)
            .DistinctUntilChanged();
        
        var acceptedText = autoExecuteObservable
            .CombineLatest(text, (auto, text) => (auto, text))
            .Where(t => t.auto)
            .Select(t => t.text);
        
        acceptedText
            .Throttle(TimeSpan.FromMicroseconds(300))
            .DistinctUntilChanged()
            .SelectMany(async t => await group.GetSpecificResult(t))
            .Select(seq => seq.Select(res => new CandidateItemViewModel(new ItemViewModelBuilder().Build(res))))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(seq =>
            {
                _candidates.Edit(inner =>
                {
                    inner.Clear();
                    inner.AddRange(seq);
                });
            });
    }
}