using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Mime;
using System.Reactive.Linq;
using LanguageExt;
using static LanguageExt.Prelude;
using Nymph.Model.Group;
using Nymph.Model.Helper;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel;
using Nymph.Shared.ViewModel.GroupViewModel;
using Nymph.Shared.ViewModel.ItemViewModel;
using Unit = System.Reactive.Unit;

namespace Nymph.Shared.Test;

using Str = AtomItem<string>;

[TestFixture]
public class StaticUnaryFunctionGroupViewModelTests
{
    [Test]
    public void StaticUnaryFunctionGroupViewModel_ShouldEmitChosenCandidateItemViewModel_WhenChosen()
    {
        // Arrange
        var staticUnaryFunctionGroup = new StaticUnaryFunctionGroup<Str, Str>(
            new FunctionItem<Str, Str>(
                async param =>
                {
                    await Task.Delay(420);
                    return Seq(Enumerable.Repeat(param, 10));
                }
            ), new Str("Rx.NET"));
        var staticUnaryFunctionGroupViewModel =
            new StaticUnaryFunctionGroupViewModel<Str, Str>(staticUnaryFunctionGroup);
        Item chosen = null;
        staticUnaryFunctionGroupViewModel.ChosenItemViewModels.Subscribe(x => chosen = x);
        var toBeChosen = staticUnaryFunctionGroupViewModel.Items.First();
        
        // Act
        toBeChosen.Choose.Execute().Subscribe();
        
        // Assert
        Assert.That(toBeChosen.ItemViewModel.GetItem, Is.EqualTo(chosen));
    }
    
    [Test]
    public void StaticUnaryFunctionGroupViewModel_ShouldBeFunctionItem_WhenFirstIn()
    {
        // Arrange
        var staticUnaryFunctionGroup = new StaticUnaryFunctionGroup<Str, Str>(
            new FunctionItem<Str, Str>(
                async param =>
                {
                    await Task.Delay(420);
                    return Seq(Enumerable.Repeat(param, 10));
                }
            ), new Str("Rx.NET"));
        var staticUnaryFunctionGroupViewModel =
            new StaticUnaryFunctionGroupViewModel<Str, Str>(staticUnaryFunctionGroup);
        var vm = staticUnaryFunctionGroupViewModel.Items.First();
        
        // Act
        
        // Assert
        Assert.That(vm.ItemViewModel.GetItem, Is.AssignableFrom(typeof(FunctionItem<Str, Str>)));
    }
    
    [Test]
    public void StaticUnaryFunctionGroupViewModel_ShouldGetCorrectResults_WhenExecuteFunc()
    {
        // Arrange
        var staticUnaryFunctionGroup = new StaticUnaryFunctionGroup<Str, Str>(
            new FunctionItem<Str, Str>(
                async param =>
                {
                    await Task.Delay(420);
                    return Seq(Enumerable.Repeat(param, 10));
                }
            ), new Str("Rx.NET"));
        var staticUnaryFunctionGroupViewModel =
            new StaticUnaryFunctionGroupViewModel<Str, Str>(staticUnaryFunctionGroup);
        var expectedResults = Enumerable.Repeat(new Str("Rx.NET"), 10).ToSeq();
        
        // Act
        // ReactiveCommand, when after Execute, a <TOutput> flow created. Use Subscribe->onNext to observe it
        staticUnaryFunctionGroupViewModel.ExecuteFunc
            .Execute(Unit.Default)
            .Subscribe();
        
        var results = staticUnaryFunctionGroupViewModel.Items
            .Select(vm => vm.ItemViewModel.GetItem)
            .Cast<Str>()
            .ToSeq();
        
        // Delay 5000ms
        Task.Delay(5000).Wait();
        
        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(results[0], Is.Not.AssignableFrom(typeof(FunctionItem<Str, Str>)));
            Assert.That(results, Has.Count.EqualTo(10));
            Assert.That(results.SequenceEqual(expectedResults), Is.True);
        });
    }
}