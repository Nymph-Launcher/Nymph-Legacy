namespace Nymph.Shared.ViewModel.ItemViewModel;

public interface IPathItemViewModel
{
    ItemViewModel GetDecoratedItem { get; }
    
    ItemViewModel GetDecorator { get; }
}