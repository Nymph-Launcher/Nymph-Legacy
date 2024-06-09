using System.Collections.ObjectModel;
using LanguageExt;
using Nymph.Model.Group;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.GroupViewModel;
using Nymph.Shared.ViewModel.ItemViewModel;

namespace Nymph.Shared.Test;

public class GroupViewModelObservableTests
{
    [Test]
    public void ListGroupViewModel_ShouldEmitChosenCandidateItemViewModel_WhenChosen()
    {
        // Arrage
        var listGroup = new ListGroup<AtomItem<string>>(
            new ListItem<AtomItem<string>>(new Seq<AtomItem<string>>([new AtomItem<string>("123")])));

        var listGroupViewModel = new ListGroupViewModel<AtomItem<string>>(listGroup);

        Item chosenItem = null;
        listGroupViewModel.ChosenItemViewModels.Subscribe(item => chosenItem = item);
        var toBeChosenItem = listGroupViewModel.Items.First();
        
        // Act
        toBeChosenItem.Choose.Execute().Subscribe();
        
        // Assert
        Assert.That(toBeChosenItem.ItemViewModel.GetItem, Is.EqualTo(chosenItem));
    }
    
    [Test]
    public void ItemGroupViewModel_ShouldEmitChosenCandidateItemViewModel_WhenChosen()
    {
        // Arrange
        var itemGroup = new ItemGroup<AtomItem<string>>(new AtomItem<string>("123"));

        var itemGroupViewModel = new ItemGroupViewModel<AtomItem<string>>(itemGroup);

        Item chosenItem = null;
        itemGroupViewModel.ChosenItemViewModels.Subscribe(item => chosenItem = item);
        var toBeChosenItem = itemGroupViewModel.Items.First();
        
        // Act
        toBeChosenItem.Choose.Execute().Subscribe();
        
        // Assert
        Assert.That(toBeChosenItem.ItemViewModel.GetItem, Is.EqualTo(chosenItem));
    }
}