using System.Collections.ObjectModel;
using Nymph.Model.Group;
using Nymph.Shared.ViewModel.ItemViewModel;

namespace Nymph.Shared.ViewModel.GroupViewModel;

using ItemViewModel = ItemViewModel.ItemViewModel;

public class ListGroupViewModel<T>(ListGroup<T> group) : GroupViewModel<ListGroup<T>>(group)
    where T : Model.Item.Item
{
    public override ObservableCollection<CandidateItemViewModel> Items => new(
        new ObservableCollection<CandidateItemViewModel>(Group.List.List.Select(item => new CandidateItemViewModel(new ItemViewModelBuilder<T>(item).Build())))
    );
}