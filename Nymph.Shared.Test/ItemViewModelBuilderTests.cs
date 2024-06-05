using LanguageExt;
using NUnit.Framework;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.ItemViewModel;

namespace Nymph.Shared.Test;

[TestFixture]
public class ItemViewModelBuilderTests
{
    [Test]
    public void Build_ShouldReturnAtomItemViewModel_WhenItemIsAtomItem()
    {
        // Arrange
        var item = new AtomItem<string>("Hello World");
        var builder = new ItemViewModelBuilder<AtomItem<string>>(item);

        // Act
        var result = builder.Build();

        // Assert
        Assert.IsInstanceOf<AtomItemViewModel<string>>(result);
    }

    [Test]
    public void Build_ShouldReturnListItemViewModel_WhenItemIsListItem()
    {
        // Arrange
        var item = new ListItem<AtomItem<bool>>(new Seq<AtomItem<bool>>(new []{new AtomItem<bool>(false)}));
        var builder = new ItemViewModelBuilder<ListItem<AtomItem<bool>>>(item);

        // Act
        var result = builder.Build();

        // Assert
        Assert.IsInstanceOf<ListItemViewModel<AtomItem<bool>>>(result);
    }

    [Test]
    public void Build_ShouldReturnFunctionItemViewModel_WhenItemIsFunctionItem()
    {
        // Arrange
        var item = new FunctionItem<AtomItem<string>, AtomItem<string>>(atomItem => Task.FromResult(new Seq<AtomItem<string>>(new []{new AtomItem<string>(atomItem.Value+"?")})));
        var builder = new ItemViewModelBuilder<FunctionItem<AtomItem<string>, AtomItem<string>>>(item);

        // Act
        var result = builder.Build();

        // Assert
        Assert.IsInstanceOf<FunctionItemViewModel<AtomItem<string>,AtomItem<string>>>(result);
    }

    [Test]
    public void Build_ShouldReturnRecordItemViewModel_WhenItemIsRecordItem()
    {
        // Arrange
        var item = new RecordItem(new Seq<Tuple<string, Item>>(new []
        {
            new Tuple<string, Item>("word1", new AtomItem<string>("cons"))
        }));
        var builder = new ItemViewModelBuilder<RecordItem>(item);

        // Act
        var result = builder.Build();

        // Assert
        Assert.IsInstanceOf<RecordItemViewModel>(result);
    }

    [Test]
    public void Build_ShouldReturnPathItemViewModel_WhenItemIsPathItem()
    {
        // Arrange
        var item = new PathItem<AtomItem<string>, AtomItem<string>>(new AtomItem<string>("123"), new AtomItem<string>("456"));
        var builder = new ItemViewModelBuilder<PathItem<AtomItem<string>,AtomItem<string>>>(item);

        // Act
        var result = builder.Build();

        // Assert
        Assert.IsInstanceOf<PathItemViewModel<AtomItem<string>,AtomItem<string>>>(result);
    }
    
    [Test]
    public void Build_FromPathItemViewModel_ShouldReturnAtomItemViewModel_WhenItemIsAtomItem()
    {
        // Arrange
        var pathItem = new PathItem<AtomItem<string>, AtomItem<int>>(new AtomItem<string>("2"), new AtomItem<int>(2));
        var pathItemViewModel = new ItemViewModelBuilder<PathItem<AtomItem<string>, AtomItem<int>>>(pathItem).Build();
        
        // Act
        var result = (pathItemViewModel as PathItemViewModel<AtomItem<string>, AtomItem<int>>)?.Decorator;
        
        // Assert
        Assert.IsInstanceOf<AtomItemViewModel<string>>(result);
    }
}