using System.Reactive.Linq;
using System.Reactive.Subjects;
using DynamicData;
using LanguageExt;
using static LanguageExt.Prelude;
using Microsoft.Reactive.Testing;
using Nymph.Model;
using Nymph.Model.Group;
using Nymph.Model.Item;
using Nymph.Model.Strategy;
using Nymph.Shared.ViewModel;
using Nymph.Shared.ViewModel.GroupViewModel;
using ReactiveUI;
using ReactiveUI.Testing;
using Unit = System.Reactive.Unit;
using System.Reactive.Subjects;

namespace Nymph.Shared.Test;

public class DefaultAndStaticUnaryStrategy : IStrategy
{
    public Seq<Group> GetGroups(LayerState state)
    {
        var defaultStrategy = new DefaultStrategy();
        var staticUnaryStrategy = new StaticUnaryStrategy();
        return defaultStrategy.GetGroups(state).Concat(staticUnaryStrategy.GetGroups(state));
    }
}

public class DefaultAndDynamicStrategy : IStrategy
{
    public Seq<Group> GetGroups(LayerState state)
    {
        var defaultStrategy = new DefaultStrategy();
        var dynamicStrategy = new DynamicUnaryStrategy();
        return defaultStrategy.GetGroups(state).Concat(dynamicStrategy.GetGroups(state));
    }
}

public class DefaultAndListPreviewStrategy : IStrategy
{
    public Seq<Group> GetGroups(LayerState state)
    {
        var defaultStrategy = new DefaultStrategy();
        var listPreviewStrategy = new ListPreviewStrategy();
        return defaultStrategy.GetGroups(state).Concat(listPreviewStrategy.GetGroups(state));
    }
}

[TestFixture]
public class MainViewModelTests
{
    [Test]
    public void Init_WithNoAction_GroupViewModelsMatcheAllBindings()
    {
            // Arrange
            var initBindings = new[]
            {
                new Binding("HelloWorld", new AtomItem<string>("hello world")),
                new Binding("Change Int Into String", new FunctionItem<AtomItem<int>,AtomItem<string>>(intAtom => 
                    Task.FromResult<Seq<AtomItem<string>>>(
                    [
                        new AtomItem<string>($"{intAtom.Value}")
                    ])
                )),
            };

            var strategy = new DefaultStrategy();
            
            var mainViewModel = new MainViewModel(initBindings, strategy);
            
            // Act
            
            // Assert
            Assert.Multiple(() => 
            {
                Assert.That(mainViewModel.GroupViewModels, Has.Count.EqualTo(2));
                Assert.That((mainViewModel.GroupViewModels[0].Items[0].ItemViewModel.GetItem as AtomItem<string>)?.Value, Is.EqualTo("hello world"));
            });

    }

    [Test]
    public void ConstraintBeAtomItemString_AfterChoosingAtomItemStringItem()
    {

            // Arrange
            var initBindings = new[]
            {
                new Binding("HelloWorld", new AtomItem<string>("hello world")),
                new Binding("Change Int Into String", new FunctionItem<AtomItem<int>, AtomItem<string>>(intAtom =>
                    Task.FromResult<Seq<AtomItem<string>>>(
                    [
                        new AtomItem<string>($"{intAtom.Value}")
                    ])
                )),
            };

            var strategy = new DefaultStrategy();

            var mainViewModel = new MainViewModel(initBindings, strategy);

            // Act
            mainViewModel.GroupViewModels[0].Items[0].Choose.Execute().Subscribe();

            // Assert
            Assert.That(mainViewModel.ConstraintItemViewModel.Map(vm => vm.ItemViewModel.GetItem),
                Is.EqualTo(Some(new AtomItem<string>("hello world"))));
    }
    
    [Test]
    public void GroupViewModelsContainListGroupViewModel_AfterChoosingListItem()
    {

        // Arrange
        var initBindings = new[]
        {
            new Binding("HelloWorld", new AtomItem<string>("hello world")),
            new Binding("Change Int Into String", new ListItem<AtomItem<string>>(Seq([new AtomItem<string>("123")]))),
        };

        var strategy = new DefaultAndListPreviewStrategy();

        var mainViewModel = new MainViewModel(initBindings, strategy);

        // Act
        mainViewModel.GroupViewModels[1].Items[0].Choose.Execute().Subscribe();

        // Assert
        // assert that the group view models contain a view model whose item is AtomItem<string>("123")
        Assert.That(mainViewModel.GroupViewModels, Has.Some.InstanceOf<ListGroupViewModel<AtomItem<string>>>());
    }

    [Ignore("This test is not working")]
    [Test]
    public void GroupViewModelShouldBeUpdated_AfterChoosingItem()
    {
            // Arrange
            var initBindings = new[]
            {
                new Binding("HelloWorld", new AtomItem<string>("hello world")),
                new Binding("1", new AtomItem<int>(1)),
                new Binding("justify text", new FunctionItem<AtomItem<string>, AtomItem<string>>(str =>
                    Task.FromResult(Seq<AtomItem<string>>([
                            new AtomItem<string>($"{str.Value}justified")
                        ])
                    )
                )),
            };

            var strategy = new DefaultAndStaticUnaryStrategy();

            var mainViewModel = new MainViewModel(initBindings, strategy);

            // Act
            mainViewModel.GroupViewModels[1].Items[0].Choose.Execute().Subscribe();

            // Assert
            Assert.That(mainViewModel.GroupViewModels, Has.Some.InstanceOf<StaticUnaryFunctionGroupViewModel<AtomItem<int>, AtomItem<string>>>());

    }
    
    [Test]
    public void GroupViewModelShouldBeUpdated_AfterTexting()
    {
            // Arrange
            var initBindings = new[]
            {
                new Binding("HelloWorld", new AtomItem<string>("hello world")),
                new Binding("1", new AtomItem<int>(1)),
                new Binding("justify text", new FunctionItem<AtomItem<string>, AtomItem<string>>(str =>
                    Task.FromResult(Seq<AtomItem<string>>([
                            new AtomItem<string>($"{str.Value}justified")
                        ])
                    )
                )),
            };

            var strategy = new DefaultAndDynamicStrategy();

            var mainViewModel = new MainViewModel(initBindings, strategy);

            // Act
            mainViewModel.SearchText = "Some text";
            
            // Assert
            Assert.That(mainViewModel.GroupViewModels, Has.Some.InstanceOf<DynamicUnaryFunctionGroupViewModel<AtomItem<string>>>());
    
    }
    
    [Ignore("This test is not working")]
    [Test]
    public void GroupViewModelShouldHaveItems_AfterExecuting()
    {
        // Arrange
        var initBindings = new[]
        {
            new Binding("HelloWorld", new AtomItem<string>("hello world")),
            new Binding("1", new AtomItem<int>(1)),
            new Binding("justify text", new FunctionItem<AtomItem<string>, AtomItem<string>>(str =>
                Task.FromResult(Seq<AtomItem<string>>([
                        new AtomItem<string>($"{str.Value}justified")
                    ])
                )
            )),
        };

        var strategy = new DefaultAndDynamicStrategy();

        var mainViewModel = new MainViewModel(initBindings, strategy);

        // Act
        mainViewModel.SearchText = "Some text";
        var groupViewModel = mainViewModel.GroupViewModels[0] as DynamicUnaryFunctionGroupViewModel<AtomItem<string>>;
        groupViewModel.IsAutoExecute = true;
        mainViewModel.SearchText = "Some other text";
        mainViewModel.SearchText = "helloworld";
            
        // Assert
        Assert.That(groupViewModel.Items[0].ItemViewModel.GetItem, Is.EqualTo(new AtomItem<string>("helloworldjustified")));
    }
}