using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using LanguageExt;
using Nymph.Model.Group;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.ItemViewModel;
using ReactiveUI;

namespace Nymph.Shared.ViewModel.GroupViewModel;

public class DynamicUnaryFunctionGroupViewModel<TResult> : GroupViewModel<DynamicUnaryFunctionGroup<TResult>>
    where TResult : Item
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

        text
            .Throttle(TimeSpan.FromMilliseconds(300))
            .DistinctUntilChanged()
            .Where(_ => IsAutoExecute)
            .SelectMany(async t => await group.GetSpecificResult(t))
            .Select(seq => seq.Select(res => new CandidateItemViewModel(new ItemViewModelBuilder().Build(res))))
            .Subscribe(seq =>
            {
                _candidates.Clear();
                _candidates.AddRange(seq);
            });
    }
}