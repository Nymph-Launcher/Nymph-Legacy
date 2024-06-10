using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using LanguageExt;
using Nymph.Model.Group;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.ItemViewModel;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Nymph.Shared.ViewModel.GroupViewModel;

public class StaticUnaryFunctionGroupViewModel<TParam, TResult> : GroupViewModel<StaticUnaryFunctionGroup<TParam, TResult>> where TParam : Item
    where TResult : Item
{
    private readonly SourceList<CandidateItemViewModel> _candidates = new();
    public override ReadOnlyObservableCollection<CandidateItemViewModel> Items { get; }
    public override IObservable<Item> ChosenItemViewModels { get; }
    public ReactiveCommand<Unit, Seq<TResult>> ExecuteFunc { get;  }

    public StaticUnaryFunctionGroupViewModel(StaticUnaryFunctionGroup<TParam, TResult> group) : base(group)
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
            .MergeMany(candidate =>
                candidate.Choose
                    .Select(_ => candidate.ItemViewModel))
            .Select(itemvm => itemvm.GetItem)
            .Publish()
            .RefCount();
        
        ExecuteFunc = ReactiveCommand.CreateFromTask(async () => await group.GetSpecificResult());
        ExecuteFunc
            .Throttle(TimeSpan.FromMilliseconds(300)) 
            .DistinctUntilChanged()
            .Do(seq =>
            {
                _candidates.Clear();
                _candidates.AddRange(seq.Select(item =>
                    new CandidateItemViewModel(new ItemViewModelBuilder().Build(item))));
            });
        // might used as ExecuteFunc.Execute(Unit.Default).Subscribe()

    }
}