using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Mime;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using DynamicData;
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
public class DynamicUnaryFunctionGroupViewModelTests
{
    [Test]
    public void DynamicUnaryFunctionGroupViewModel_ShouldEmitChosenCandidateItemViewModel_WhenChosen()
    {
        // Arrange
        var dynamicUnaryFunctionGroup = new DynamicUnaryFunctionGroup<Str>(
            new FunctionItem<Str, Str>(
                async param =>
                {
                    await Task.Delay(420);
                    return Seq(Enumerable.Repeat(param, 10));
                }
            ));
        SourceList<Str> _text= new();
        var text = _text
            .Connect()
            .Cast<Str>();
        var dynamicUnaryFunctionGroupViewModel =
            new DynamicUnaryFunctionGroupViewModel<Str>(dynamicUnaryFunctionGroup, text);
        Item chosen = null;
        dynamicUnaryFunctionGroupViewModel.ChosenItemViewModels.Subscribe(x => chosen = x);
        var toBeChosen = dynamicUnaryFunctionGroupViewModel.Items.First();
        
        // Act
        toBeChosen.Choose.Execute().Subscribe();
        
        // Assert
        Assert.That(toBeChosen.ItemViewModel.GetItem, Is.EqualTo(chosen));
    }
    
    [Test]
    public void DynamicUnaryFunctionGroupViewModel_ShouldBeFunctionItem_WhenFirstIn()
    {
        // Arrange
        var dynamicUnaryFunctionGroup = new DynamicUnaryFunctionGroup<Str>(
            new FunctionItem<Str, Str>(
                async param =>
                {
                    await Task.Delay(420);
                    return Seq(Enumerable.Repeat(param, 10));
                }
            ));
        SourceList<Str> _text= new();
        var text = _text
            .Connect()
            .Cast<Str>();
        var dynamicUnaryFunctionGroupViewModel =
            new DynamicUnaryFunctionGroupViewModel<Str>(dynamicUnaryFunctionGroup, text);
        var vm = dynamicUnaryFunctionGroupViewModel.Items.First();
        
        // Act
        
        // Assert
        Assert.That(vm.ItemViewModel.GetItem, Is.AssignableFrom(typeof(FunctionItem<Str, Str>)));
    }
    
    [Test]
    public void StaticUnaryFunctionGroupViewModel_ShouldGetCorrectResults_WhenNextInput()
    {
        // Arrange
        var dynamicUnaryFunctionGroup = new DynamicUnaryFunctionGroup<Str>(
            new FunctionItem<Str, Str>(
                async param =>
                {
                    await Task.Delay(420);
                    return Seq(Enumerable.Repeat(param, 10));
                }
            ));

        var subject = new Subject<Str>();
        IObservable<Str> text = subject;
        
        var dynamicUnaryFunctionGroupViewModel =
            new DynamicUnaryFunctionGroupViewModel<Str>(dynamicUnaryFunctionGroup, text);
        var vm = dynamicUnaryFunctionGroupViewModel.Items.First();
        // set Auto Execute the Function
        dynamicUnaryFunctionGroupViewModel.IsAutoExecute = true;
        var expectedResults = Enumerable.Repeat(new Str("NEXT TYPE"), 10).ToSeq();
        var expectedResults2 = Enumerable.Repeat(new Str("THIRD TYPE"), 10).ToSeq();
        
        // Act
        subject.OnNext(new Str("NEXT TYPE"));
        var results = dynamicUnaryFunctionGroupViewModel.Items
            .Select(vm => vm.ItemViewModel.GetItem)
            .Cast<Str>()
            .ToSeq();

        Task.Delay(5000).Wait();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(results[0], Is.Not.AssignableFrom(typeof(FunctionItem<Str, Str>)));
            Assert.That(results, Has.Count.EqualTo(10));
            Assert.That(results.SequenceEqual(expectedResults), Is.True);
        });
        
        // Act
        subject
            .Delay(TimeSpan.FromSeconds(2));
        subject
            .OnNext(new Str("THIRD TYPE"));

        Task.Delay(5000).Wait();
        
        results = dynamicUnaryFunctionGroupViewModel.Items
            .Select(vm => vm.ItemViewModel.GetItem)
            .Cast<Str>()
            .ToSeq();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(results[0], Is.Not.AssignableFrom(typeof(FunctionItem<Str, Str>)));
            Assert.That(results, Has.Count.EqualTo(10));
            Assert.That(results.SequenceEqual(expectedResults2), Is.True);
        });
    }
}