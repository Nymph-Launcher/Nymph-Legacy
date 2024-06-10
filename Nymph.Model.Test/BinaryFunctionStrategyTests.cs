using Nymph.Model.Group;
using Nymph.Model.Item;
using Nymph.Model.Strategy;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Nymph.Model.Test;

public class BinaryFunctionStrategyTests
{
    [Fact]
    public void GetGroups_ReturnBinaryFunctionGroup_WhenStateAndTextProper()
    {
        //arrange
        var funcItem = new FunctionItem<AtomItem<string>, FunctionItem<AtomItem<string>, AtomItem<string>>>(
            async param =>
            {
                await Task.Delay(100);
                return Seq([new FunctionItem<AtomItem<string>, AtomItem<string>>(
                    async _ =>
                    {
                        await Task.Delay(100);
                        return Seq([new AtomItem<string>("Hello, world!")]);
                    })]);
            });
        var state = new LayerState(
            Seq<Binding>([new Binding("hello", funcItem)]),
            Option<Item.Item>.Some(new AtomItem<string>("Hello, item")), 
            "hello");
        var strategy = new BinaryFunctionStrategy();
        
        //act
        var result = strategy.GetGroups(state);
        
        //assert
        Assert.NotEmpty(result);
        Assert.All(result, group => Assert.IsAssignableFrom<BinaryFunctionGroup<AtomItem<string>, AtomItem<string>>>(group));
    }

    [Fact]
    public void GetGroups_ReturnAtomItemString_WhenStateAndTextProper()
    {
        //arrange
        var funcItem = new FunctionItem<AtomItem<string>, FunctionItem<AtomItem<string>, AtomItem<string>>>(
            async param =>
            {
                await Task.Delay(100);
                return Seq([new FunctionItem<AtomItem<string>, AtomItem<string>>(
                    async _ =>
                    {
                        await Task.Delay(100);
                        return Seq([new AtomItem<string>("Hello, world!")]);
                    })]);
            });
        var state = new LayerState(
            Seq<Binding>([new Binding("hello", funcItem)]),
            Option<Item.Item>.Some(new AtomItem<string>("Hello, item")), 
            "hello");
        var strategy = new BinaryFunctionStrategy();
        
        //act
        var binaryGroup = strategy.GetGroups(state)[0] as BinaryFunctionGroup;
        Assert.NotNull(binaryGroup);
        var result = binaryGroup.GetFinalResult(new AtomItem<string>("hello"))
            .Some(task =>
            {
                var item = task.Result[0] as AtomItem<string>;
                return (item ?? new AtomItem<string>("")).GetValue();
            })
            .None("");
        
        //assert
        Assert.Equal(result, "Hello, world!");

    }

    [Fact]
    public void GetGroups_ReturnsEmpty_WhenStateNoProperBinding()
    {
        var funcItem = new FunctionItem<AtomItem<string>, AtomItem<int>>(
            async param =>
            {
                await Task.Delay(100);
                return Seq([new AtomItem<int>(42)]);
            });
        var state = new LayerState(
            Seq<Binding>([new Binding("hello", funcItem)]),
            Option<Item.Item>.Some(new AtomItem<string>("Hello, item")), 
            "hello");
        var strategy = new BinaryFunctionStrategy();
        var result = strategy.GetGroups(state);
        Assert.Empty(result);
    }
    
    [Fact]
    public void GetGroups_ReturnEmpty_WhenStateTextNotExists()
    {
        var funcItem = new FunctionItem<AtomItem<string>, FunctionItem<AtomItem<string>, AtomItem<string>>>(
            async param =>
            {
                await Task.Delay(100);
                return Seq([new FunctionItem<AtomItem<string>, AtomItem<string>>(
                    async _ =>
                    {
                        await Task.Delay(100);
                        return Seq([new AtomItem<string>("Hello, world!")]);
                    })]);
            });
        var state = new LayerState(
            Seq<Binding>([new Binding("hello", funcItem)]),
            Option<Item.Item>.Some(funcItem), " ");
        var strategy = new BinaryFunctionStrategy();
        var result = strategy.GetGroups(state);
        Assert.Empty(result);
    }
}


