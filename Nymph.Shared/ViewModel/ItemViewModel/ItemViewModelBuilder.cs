using Nymph.Model.Item;

namespace Nymph.Shared.ViewModel.ItemViewModel;

public class ItemViewModelBuilder()
{
    public ItemViewModel Build(Item item)
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
    
    public ItemViewModel<T> Build<T>(T item) where T : Item
    {
        var vm = item switch
        {
            AtomItem atomItem => CreateAtomItemViewModel(atomItem),
            ListItem listItem => CreateListItemViewModel(listItem),
            FunctionItem functionItem => CreateFunctionItemViewModel(functionItem),
            RecordItem recordItem => CreateRecordItemViewModel(recordItem),
            PathItem pathItem => CreatePathItemViewModel(pathItem),
            _ => throw new ArgumentException("Unknown item type.")
        } as ItemViewModel<T>;
        
        if (vm == null)
        {
            throw new ArgumentException("Failed to create item view model.");
        }

        return vm;
    }
    
    private ItemViewModel? CreateAtomItemViewModel(AtomItem atomItem)
    {
        return (ItemViewModel?)Activator.CreateInstance(
            typeof(AtomItemViewModel<>).MakeGenericType(atomItem.GetType().GetGenericArguments()[0]), atomItem);
    }
    
    private ItemViewModel? CreateFunctionItemViewModel(FunctionItem functionItem)
    {
        var typeParameters = functionItem.GetType().GetGenericArguments();
        return (ItemViewModel?)Activator.CreateInstance(
            typeof(FunctionItemViewModel<,>).MakeGenericType(typeParameters[0], typeParameters[1]), functionItem);
    }
    
    private ItemViewModel? CreatePathItemViewModel(PathItem pathItem)
    {
        var typeParameters = pathItem.GetType().GetGenericArguments();
        return (ItemViewModel?)Activator.CreateInstance(
            typeof(PathItemViewModel<,>).MakeGenericType(typeParameters[0], typeParameters[1]), pathItem);
    }
    
    private ItemViewModel? CreateRecordItemViewModel(RecordItem recordItem)
    {
        return new RecordItemViewModel(recordItem) as ItemViewModel;
    }
    
    private ItemViewModel? CreateListItemViewModel(ListItem listItem)
    {
        var typeParameter = listItem.GetType().GetGenericArguments()[0];
        return (ItemViewModel?)Activator.CreateInstance(
            typeof(ListItemViewModel<>).MakeGenericType(typeParameter), listItem);
    }
}