using System.Reactive.Linq;
using System.Reactive.Subjects;
using LanguageExt;
using Microsoft.Reactive.Testing;
using static LanguageExt.Prelude;
using Nymph.Model.Group;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.GroupViewModel;
using Seq = LanguageExt.Seq;
using ReactiveUI.Testing;

namespace Nymph.Shared.Test;

using Str = AtomItem<string>;

public class BinaryFunctionGroupViewModelTests
{
    private IObservable<Str> CombineLatest_test(IObservable<Str> text,IObservable<bool> autoOperation)
    {
        return autoOperation
            .Select(flag => flag ? text : Observable.Empty<Str>())
            .Switch();;
    }
    
    [Test]
    public void Switch_test()
    {
        //arrange
        var text = new Subject<Str>();
        var autoOperation = new Subject<bool>();
        var finalText = CombineLatest_test(text, autoOperation);

        string[] expressed = ["h","he","hel","hell","hello","world","456"];
        var result = List<string>();
        
        //act
        finalText.Subscribe(output =>
        {
            result = result.Add(output.Value);
            Console.WriteLine(output.Value);
        });
        
        autoOperation.OnNext(true);
        text.OnNext(new Str("h"));
        text.OnNext(new Str("he"));
        text.OnNext(new Str("hel"));
        autoOperation.OnNext(true);
        text.OnNext(new Str("hell"));
        text.OnNext(new Str("hello"));
        text.OnNext(new Str("world"));
        autoOperation.OnNext(false);
        text.OnNext(new Str("123"));
        autoOperation.OnNext(true);
        text.OnNext(new Str("456"));
        
        //assert
        Console.WriteLine(result.Count);
        for (int i = 0; i < expressed.Length-1; i++)
        {
            Assert.That(result[i], Is.EqualTo(expressed[i]));
        }
        
    }

    [Test]
    public void BinaryFunctionGroupViewModel_ShouldEmitChosenCandidateItemViewModel_WhenChosen()
    {
        //arrange
        var text = new Subject<Str>();
        var autoOperation = new Subject<bool>();
        
        
         var innerFunctionItem = new FunctionItem<Str, Str>(
             async param =>
             {
                 await Task.Delay(100);
                 return Seq(Enumerable.Repeat(param, 10));
             });
        
         var binaryFunction = new FunctionItem<Str, FunctionItem<Str, Str>>(
             async _ =>
             {
                 await Task.Delay(100);
                 return Seq(Enumerable.Repeat(innerFunctionItem,10));
             });
        
         var binaryFunctionGroup = new BinaryFunctionGroup<Str, Str, Str>(
             binaryFunction, new Str("hello")
         );

         var binaryFunctionGroupVm = new BinaryFunctionGroupViewModel<Str,Str,Str>(binaryFunctionGroup, text);

         Item chosen = null;
         binaryFunctionGroupVm.ChosenItemViewModels.Subscribe(x => chosen = x);
         var toBeChosen = binaryFunctionGroupVm.Items.First();

         //act
         toBeChosen.Choose.Execute().Subscribe();
         
         //Assert
         Assert.That(toBeChosen.ItemViewModel.GetItem,Is.EqualTo(chosen));
    }

    [Test]
    public void BinaryFunctionGroupViewModel_ShouldBeFunctionItem_WhenFirstIn()
    {
        //arrange
        var text = new Subject<Str>();
        var autoOperation = new Subject<bool>();
        
        var innerFunctionItem = new FunctionItem<Str, Str>(
            async param =>
            {
                await Task.Delay(100);
                return Seq(Enumerable.Repeat(param, 10));
            });
        
        var binaryFunction = new FunctionItem<Str, FunctionItem<Str, Str>>(
            async _ =>
            {
                await Task.Delay(100);
                return Seq(Enumerable.Repeat(innerFunctionItem,10));
            });
        
        var binaryFunctionGroup = new BinaryFunctionGroup<Str, Str, Str>(
            binaryFunction, new Str("hello")
        );

        var binaryFunctionGroupVm = new BinaryFunctionGroupViewModel<Str,Str,Str>(binaryFunctionGroup, text);

        var firstGroup = binaryFunctionGroupVm.Items.First();
        
        //act
        
        //assert
        Assert.That(firstGroup.ItemViewModel.GetItem,Is.AssignableFrom(typeof(FunctionItem<Str,FunctionItem<Str,Str>>)));
    }

    //Test With Throttle()
    [Test]
    public void BinaryFunctionGroupViewModel_ShouldGetCorrectResult_WhenExecuteFunc_WhenTextChange()
    {
        //arrange
        var text = new Subject<Str>();
        var autoOperation = new Subject<bool>();
        
        
        var innerFunctionItem = new FunctionItem<Str, Str>(
            async param =>
            {
                await Task.Delay(100);
                return Seq(Enumerable.Repeat(param, 10));
            });
        
        var binaryFunction = new FunctionItem<Str, FunctionItem<Str, Str>>(
            async _ =>
            {
                await Task.Delay(1);
                return Seq(Enumerable.Repeat(innerFunctionItem,1));
            });
        
        var binaryFunctionGroup = new BinaryFunctionGroup<Str, Str, Str>(
            binaryFunction, new Str("hello")
        );

        var binaryFunctionGroupVm = new BinaryFunctionGroupViewModel<Str,Str,Str>(binaryFunctionGroup, text);
        var groups = binaryFunctionGroupVm.Items;
        var results = groups
            .Select(vm => vm.ItemViewModel.GetItem)
            .Cast<Str>()
            .ToSeq();
        var expectedResult = Enumerable.Repeat(new Str("2333"), 10).ToSeq();
        
        //act
        
        // binaryFunctionGroupVm.finalText.Subscribe(output =>
        // {
        //     Console.WriteLine(output.Value);
        // });
        
        text.OnNext(new Str("1223"));
        Task.Delay(TimeSpan.FromMilliseconds(500)).Wait();
        text.OnNext(new Str("1233"));
        autoOperation.OnNext(false);
        Task.Delay(TimeSpan.FromMilliseconds(500)).Wait();
        text.OnNext(new Str("2333"));
        
        //assert
        Task.Delay(TimeSpan.FromMilliseconds(500)).Wait();
        // Console.WriteLine(results);
        Assert.That(results[0],Is.Not.AssignableFrom(typeof(FunctionItem<Str,FunctionItem<Str,Str>>)));
        Assert.That(results,Has.Count.EqualTo(10));
        Assert.That(results.SequenceEqual(expectedResult),Is.True);
    }
    
    //Test Without Throttle()
    [Test]
    public void BinaryFunctionGroupViewModel_ShouldGetCorrectResult_WhenExecuteFunc_WhenAutoOperationChange()
    {
        new TestScheduler().With(scheduler =>
        {
        //arrange
        var text = new Subject<Str>();
        var autoOperation = new Subject<bool>();
        
        var innerFunctionItem = new FunctionItem<Str, Str>(
            async param =>
            {
                //await Task.Delay(100);
                return Seq(Enumerable.Repeat(param, 10));
            });
        
        var binaryFunction = new FunctionItem<Str, FunctionItem<Str, Str>>(
            async _ =>
            {
                //await Task.Delay(1);
                return Seq(Enumerable.Repeat(innerFunctionItem,1));
            });
        
        var binaryFunctionGroup = new BinaryFunctionGroup<Str, Str, Str>(
            binaryFunction, new Str("hello")
        );

        var binaryFunctionGroupVm = new BinaryFunctionGroupViewModel<Str,Str,Str>(binaryFunctionGroup, text);
        var groups = binaryFunctionGroupVm.Items;
        var results = groups
            .Select(vm => vm.ItemViewModel.GetItem)
            .Cast<Str>()
            .ToSeq();
        var expectedResult = new List<string>(["1223","1233","1231231","2333","100"]).ToSeq();
        var testings = new List<string>();
        
        //act
        
        binaryFunctionGroupVm.finalText.Subscribe(output =>
        {
            testings.Add(output.Value);
        });

        //text.OnNext(new Str(""));
        //Task.Delay(TimeSpan.FromMilliseconds(500)).Wait();
        scheduler.AdvanceByMs(500);
        binaryFunctionGroupVm.AutoOperation = true;
        text.OnNext(new Str("1223"));
        // Task.Delay(TimeSpan.FromMilliseconds(500)).Wait();
        scheduler.AdvanceByMs(500);
        text.OnNext(new Str("1233"));
        text.OnNext(new Str("1231231"));
        //Task.Delay(TimeSpan.FromMilliseconds(500)).Wait();
        scheduler.AdvanceByMs(500);
        binaryFunctionGroupVm.AutoOperation = false;
        text.OnNext(new Str("2333"));
        //Task.Delay(TimeSpan.FromMilliseconds(500)).Wait();
        scheduler.AdvanceByMs(500);
        binaryFunctionGroupVm.AutoOperation = true;
        text.OnNext(new Str("100"));
        
        //assert
        //Task.Delay(TimeSpan.FromMilliseconds(500)).Wait();
        scheduler.AdvanceByMs(500);
        //Console.WriteLine(results);
        // Assert.That(results[0],Is.Not.AssignableFrom(typeof(FunctionItem<Str,FunctionItem<Str,Str>>)));
        // Assert.That(results,Has.Count.EqualTo(10));
        // Assert.That(results.SequenceEqual(expectedResult),Is.True);
        
        //When Without Throttle,Is Correct
        Assert.That(testings.SequenceEqual(expectedResult),Is.True);
        Assert.That(testings, Has.Count.EqualTo(5));
        });
    }
}