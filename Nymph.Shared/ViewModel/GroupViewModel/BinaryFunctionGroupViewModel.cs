using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using DynamicData;
using LanguageExt;
using LanguageExt.Pipes;
using Nymph.Model.Group;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.ItemViewModel;
using ReactiveUI;
using static LanguageExt.Prelude;


namespace Nymph.Shared.ViewModel.GroupViewModel;

public class BinaryFunctionGroupViewModel<TParam1,TParam2,TResult>: GroupViewModel<BinaryFunctionGroup<TParam1,TParam2,TResult>>, IBinaryFunctionGroupViewModel where TParam1: Item
    where TParam2: AtomItem<string>
    where TResult: Item
{
    private bool _autoOperation;
    public bool AutoOperation
    {
        get => _autoOperation;
        set => this.RaiseAndSetIfChanged(ref _autoOperation,value);
    }

    private readonly SourceList<CandidateItemViewModel> _candidate = new ();

    public override ReadOnlyObservableCollection<CandidateItemViewModel> Items { get; }
    
    public override IObservable<Item> ChosenItemViewModels { get; }

    public IObservable<AtomItem<string>> finalText;

    public BinaryFunctionGroupViewModel(BinaryFunctionGroup<TParam1,TParam2,TResult> group,IObservable<AtomItem<string>> text) : base(group)
    {
        var autoOperation = this
            .WhenAnyValue(x => x.AutoOperation)
            .Throttle(TimeSpan.FromMilliseconds(100))
            .DistinctUntilChanged();
        
        finalText = autoOperation
            .CombineLatest(text,(auto,text)=>(auto,text))
            .Where(item => item.auto)
            .Select(item => item.text);
        
        finalText
            .SelectMany(GetResults)
            .Subscribe(results => _candidate.Edit(inner =>
            {
                inner.Clear();
                inner.AddRange(results.Select(result => new CandidateItemViewModel(new ItemViewModelBuilder().Build(result))));
            }));
        
        _candidate.Add(new CandidateItemViewModel(new ItemViewModelBuilder().Build(group.BinaryFunction)));
        
        _candidate
            .Connect()
            .Bind(out var candidates)
            .Subscribe();
        
        Items = candidates;
        
        ChosenItemViewModels = _candidate
            .Connect()
            .AutoRefresh()
            .MergeMany(candidate => candidate.Choose
                .Select(_ => candidate.ItemViewModel))
            .Select(itemvm => itemvm.GetItem)
            .Publish()
            .RefCount();
    }
    private async Task<Seq<TResult>> GetResults(AtomItem<string> item)
    {
        return await Group.GetSpecificFinalResult(item)
            .Some(results => results)
            .None(Task.FromResult(Seq<TResult>()));
    }
}
