using LanguageExt;
using static LanguageExt.Prelude;
using Nymph.Model.Group;
using Nymph.Model.Item;
using Nymph.Model.Strategy;

namespace Nymph.Model.Test;

public class TextProcessStrategyTest
{
    [Fact]
    public void GetGroups_ReturnsUnaryFunctionGroup_WhenStateTextExists()
    {
        var funcItem = new FunctionItem<AtomItem<string>, AtomItem<string>>(
            async param =>
            {
                await Task.Delay(100);
                return Seq([new AtomItem<string>("Hello, World!")]);
            });
        var state = new LayerState(Seq<Binding>(
                [new Binding("hello", funcItem)]), 
            Option<Item.Item>.Some(funcItem), "hello");
        var strategy = new TextProcessStrategy();
        var result = strategy.GetGroups(state);

        Assert.NotEmpty(result);
        Assert.All(result, group => Assert.IsAssignableFrom<UnaryFunctionGroup<AtomItem<string>>>(group));
    }

    [Fact]
    public void GetGroups_ReturnUnaryFunctionGroup_WhenStateTextNotExists()
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
        var strategy = new TextProcessStrategy();
        var result = strategy.GetGroups(state);

        Assert.Empty(result);
    }

    [Fact]
    public void GetGroups_ReturnsEmpty_WhenStateNoProperBinding()
    {
        var state = new LayerState(Seq<Binding>(), 
            Option<Item.Item>.None, " ");
        var strategy = new TextProcessStrategy();
        var result = strategy.GetGroups(state);

        Assert.Empty(result);
    }
}