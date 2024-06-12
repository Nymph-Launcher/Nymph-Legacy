using System.Collections.ObjectModel;
using Nymph.Model.Item;

namespace Nymph.Shared.ViewModel.ItemViewModel;

public class RecordItemViewModel(RecordItem recordItem) : ItemViewModel<RecordItem>(recordItem)
{
    private readonly RecordItem _recordItem = recordItem;

    public ReadOnlyCollection<Tuple<string, ItemViewModel>> Properties => _recordItem
        .Properties
        .Select(p => new Tuple<string, ItemViewModel>(p.Item1, new ItemViewModelBuilder()
            .Build(p.Item2)))
        .ToList()
        .AsReadOnly();
}