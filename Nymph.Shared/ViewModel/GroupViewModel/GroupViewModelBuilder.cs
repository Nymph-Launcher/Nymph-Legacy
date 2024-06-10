using Nymph.Model.Group;

namespace Nymph.Shared.ViewModel.GroupViewModel;

public class GroupViewModelBuilder
{
    public GroupViewModel Build(Group group)
    {
        var vm = group switch
        {
            ItemGroup itemGroup => CreateItemGroupViewModel(itemGroup),
            ListGroup listGroup => CreateListGroupViewModel(listGroup),
            StaticUnaryFunctionGroup staticUnaryFunctionGroup => CreateStaticUnaryFunctionGroupViewModel(staticUnaryFunctionGroup),
            DynamicUnaryFunctionGroup dynamicUnaryFunctionGroup => CreateDynamicUnaryFunctionGroupViewModel(dynamicUnaryFunctionGroup),
            BinaryFunctionGroup binaryFunctionGroup => CreateBinaryFunctionGroupViewModel(binaryFunctionGroup),
            _ => throw new ArgumentException("Unknown group type.")
        } as GroupViewModel;

        if (vm == null)
        {
            throw new ArgumentException("Failed to create group view model.");
        }

        return vm;
    }
    
    public GroupViewModel<T> Build<T>(T group) where T : Group
    {
        var vm = group switch
        {
            ItemGroup itemGroup => CreateItemGroupViewModel(itemGroup),
            ListGroup listGroup => CreateListGroupViewModel(listGroup),
            StaticUnaryFunctionGroup staticUnaryFunctionGroup => CreateStaticUnaryFunctionGroupViewModel(staticUnaryFunctionGroup),
            DynamicUnaryFunctionGroup dynamicUnaryFunctionGroup => CreateDynamicUnaryFunctionGroupViewModel(dynamicUnaryFunctionGroup),
            BinaryFunctionGroup binaryFunctionGroup => CreateBinaryFunctionGroupViewModel(binaryFunctionGroup),
            _ => throw new ArgumentException("Unknown group type.")
        } as GroupViewModel<T>;

        if (vm == null)
        {
            throw new ArgumentException("Failed to create group view model.");
        }

        return vm;
    }

    private GroupViewModel? CreateItemGroupViewModel(ItemGroup itemGroup)
    {
        return (GroupViewModel?)Activator.CreateInstance(
            typeof(ItemGroupViewModel<>).MakeGenericType(itemGroup.GetType().GetGenericArguments()[0]), itemGroup);
    }

    private GroupViewModel? CreateListGroupViewModel(ListGroup listGroup)
    {
        return (GroupViewModel?)Activator.CreateInstance(
            typeof(ListGroupViewModel<>).MakeGenericType(listGroup.GetType().GetGenericArguments()[0]), listGroup);
    }

    private GroupViewModel? CreateStaticUnaryFunctionGroupViewModel(StaticUnaryFunctionGroup staticUnaryFunctionGroup)
    {
        var typeParameters = staticUnaryFunctionGroup.GetType().GetGenericArguments();
        return (GroupViewModel?)Activator.CreateInstance(
            typeof(StaticUnaryFunctionGroupViewModel<,>).MakeGenericType(typeParameters[0], typeParameters[1]),
            staticUnaryFunctionGroup);
    }

    private GroupViewModel? CreateDynamicUnaryFunctionGroupViewModel(
        DynamicUnaryFunctionGroup dynamicUnaryFunctionGroup)
    {
        return (GroupViewModel?)Activator.CreateInstance(
            typeof(DynamicUnaryFunctionGroupViewModel<>).MakeGenericType(dynamicUnaryFunctionGroup.GetType()
                .GetGenericArguments()[0]), dynamicUnaryFunctionGroup);
    }

    private GroupViewModel? CreateBinaryFunctionGroupViewModel(BinaryFunctionGroup binaryFunctionGroup)
    {
        var typeParameters = binaryFunctionGroup.GetType().GetGenericArguments();
        return (GroupViewModel?)Activator.CreateInstance(
            typeof(BinaryFunctionGroupViewModel<,,>).MakeGenericType(typeParameters[0], typeParameters[1],
                typeParameters[2]), binaryFunctionGroup);
    }
}