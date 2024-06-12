using Nymph.Model.Item;

namespace Nymph.Shared.ViewModel.GroupViewModel;

public interface IItemPreviewViewModel
{
    Item GetItem { get; }
}