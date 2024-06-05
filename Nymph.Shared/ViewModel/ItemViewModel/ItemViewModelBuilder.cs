using Nymph.Model.Item;

namespace Nymph.Shared.ViewModel.ItemViewModel;

public class ItemViewModelBuilder<T>(T item)
    where T : Item
{
    public ItemViewModel<T> Build()
    {
        var vm = item switch
        {
            AtomItem atomItem => CreateAtomItemViewModel(atomItem),
            ListItem listItem => CreateListItemViewModel(listItem),
            FunctionItem functionItem => CreateFunctionItemViewModel(functionItem),
            RecordItem recordItem => CreateRecordItemViewModel(recordItem),
            PathItem pathItem => CreatePathItemViewModel(pathItem),
            _ => throw new ArgumentException("Unknown item type.")
        };
        
        if (vm == null)
        {
            throw new ArgumentException("Failed to create item view model.");
        }

        return vm;
    }
    
    private ItemViewModel<T>? CreateAtomItemViewModel(AtomItem atomItem)
    {
        return (ItemViewModel<T>?)Activator.CreateInstance(
            typeof(AtomItemViewModel<>).MakeGenericType(atomItem.GetType().GetGenericArguments()[0]), atomItem);
    }
    
    private ItemViewModel<T>? CreateFunctionItemViewModel(FunctionItem functionItem)
    {
        var typeParameters = functionItem.GetType().GetGenericArguments();
        return (ItemViewModel<T>?)Activator.CreateInstance(
            typeof(FunctionItemViewModel<,>).MakeGenericType(typeParameters[0], typeParameters[1]), functionItem);
    }
    
    private ItemViewModel<T>? CreatePathItemViewModel(PathItem pathItem)
    {
        var typeParameters = pathItem.GetType().GetGenericArguments();
        return (ItemViewModel<T>?)Activator.CreateInstance(
            typeof(PathItemViewModel<,>).MakeGenericType(typeParameters[0], typeParameters[1]), pathItem);
    }
    
    private ItemViewModel<T>? CreateRecordItemViewModel(RecordItem recordItem)
    {
        return new RecordItemViewModel(recordItem) as ItemViewModel<T>;
    }
    
    private ItemViewModel<T>? CreateListItemViewModel(ListItem listItem)
    {
        var typeParameter = listItem.GetType().GetGenericArguments()[0];
        return (ItemViewModel<T>?)Activator.CreateInstance(
            typeof(ListItemViewModel<>).MakeGenericType(typeParameter), listItem);
    }
}