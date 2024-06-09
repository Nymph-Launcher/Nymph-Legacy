using Nymph.Model.Item;
using Nymph.Model.Group;
using Nymph.Model.Strategy;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Nymph.Model.Test;

public class UnaryProcessStrategyTest
{
    [Fact]
    public void GetGroups_ReturnsUnaryFunctionGroup_WhenStateItemProper()
    {
        var funcItem = new FunctionItem<AtomItem<string>, AtomItem<string>>(
            async param =>
            {
                await Task.Delay(100);
                return Seq([new AtomItem<string>("Hello, world!")]);
            });
        var state = new LayerState(
             Seq<Binding>([new Binding("hello", funcItem)]),
                Option<Item.Item>.Some(new AtomItem<string>("Hello, world")), " ");
        var strategy = new UnaryProcessStrategy();
        var result = strategy.GetGroups(state);
        Assert.NotEmpty(result);
        Assert.All(result, group => Assert.IsAssignableFrom<UnaryFunctionGroup<AtomItem<string>>>(group));
    }

    [Fact]
    public void GetGroups_ReturnUnaryFunctionGroupResult_WhenStateItemProper()
    {
        var funcItem = new FunctionItem<AtomItem<string>, AtomItem<string>>(
            async param =>
            {
                await Task.Delay(100);
                return Seq([new AtomItem<string>("Hello, world!")]);
            });
        var state = new LayerState(
            Seq<Binding>([new Binding("hello", funcItem)]),
            Option<Item.Item>.Some(new AtomItem<string>("Hello, world")), " ");
        var strategy = new UnaryProcessStrategy();
        var result = strategy.GetGroups(state);
        Assert.All(result, async group =>
        {
            var funcGroup = group as UnaryFunctionGroup;
            if (funcGroup == null) return;
            var atom = await funcGroup.GetResult();
            var str = atom[0] as AtomItem<string>;
            Assert.Equal((str ?? new AtomItem<string>("")).GetValue(), "Hello, world!");
        });
    }

    [Fact]
    public void GetGroups_ReturnsUnaryFunctionGroup_WhenStateItemImproper()
    {
        var funcItem = new FunctionItem<AtomItem<string>, AtomItem<string>>(
            async param =>
            {
                await Task.Delay(100);
                return Seq([new AtomItem<string>("Hello, World!")]);
            });
        var state = new LayerState(Seq<Binding>(
                [new Binding("hello", funcItem)]),
            Option<Item.Item>.Some(funcItem), " ");
        var strategy = new UnaryProcessStrategy();
        var result = strategy.GetGroups(state);
        Assert.Empty(result);
    }
    
    
    [Fact]
    public void GetGroups_ReturnsUnaryFunctionGroup_WhenNoProperBinding()
    {
        var funcItem = new FunctionItem<ListItem, AtomItem<string>>(
            async param =>
            {
                await Task.Delay(100);
                return Seq([new AtomItem<string>("Hello, World!")]);
            });
        var state = new LayerState(Seq<Binding>(
                [new Binding("hello", funcItem)]),
            Option<Item.Item>.Some(new AtomItem<string>("Hello, world")), " ");
        var strategy = new UnaryProcessStrategy();
        var result = strategy.GetGroups(state);
        Assert.Empty(result);
    }
    
}