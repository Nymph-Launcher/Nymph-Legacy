using System.Collections.ObjectModel;
using Nymph.Model.Item;

namespace Nymph.Shared.ViewModel.ItemViewModel;

public interface IListItemViewModel
{
    ReadOnlyCollection<ItemViewModel> GetItems { get; }
}