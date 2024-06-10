using LanguageExt;
using static LanguageExt.Prelude;
using Nymph.Model.Item;
using Nymph.Model.Group;
using Nymph.Model.Strategy;

namespace Nymph.Model.Test;

public class ApplyTextToConstraintStrategyTests
{
    [Fact]
    public void GetGroups_ReturnDynamicUnaryFunctionGroup_WhenStateItemAndTextProper()
    {
        // arrange
        var funcItem = new FunctionItem<AtomItem<string>, AtomItem<string>>(async _ =>
        {
            await Task.Delay(100);
            return Seq<AtomItem<string>>([new AtomItem<string>("Hello, world!")]);
        });
        var state = new LayerState(Seq<Binding>(), Option<Item.Item>.Some(funcItem), "hello");
        var strategy = new ApplyTextToConstraintStrategy();
        // act
        var result = strategy.GetGroups(state);
        //assert
        Assert.NotEmpty(result);
        Assert.All(result, group => Assert.IsAssignableFrom<DynamicUnaryFunctionGroup<AtomItem<string>>>(group));

        var group = result[0] as DynamicUnaryFunctionGroup<AtomItem<string>>;
        Assert.NotNull(group);
        var groupResult = group.GetResult(new AtomItem<string>("")).Result;
        Assert.Equal("Hello, world!",  (groupResult[0] as AtomItem<string>).GetValue());
    }

    [Fact]
    public void GetGroups_ReturnEmpty_WhenStateItemNotProper()
    {
        var funcItem = new FunctionItem<AtomItem<int>, AtomItem<int>>(async _ =>
        {
            await Task.Delay(100);
            return Seq<AtomItem<int>>([new AtomItem<int>(10)]);
        });
        var state = new LayerState(Seq<Binding>(), Option<Item.Item>.Some(funcItem), "hello");
        var strategy = new ApplyTextToConstraintStrategy();

        var result = strategy.GetGroups(state);
        
        Assert.Empty(result);
    }

    [Fact]
    public void GetGroups_ReturnEmpty_WhenStateTextNotProper()
    {
        var funcItem = new FunctionItem<AtomItem<string>, AtomItem<string>>(async _ =>
        {
            await Task.Delay(100);
            return Seq<AtomItem<string>>([new AtomItem<string>("Hello, world!")]);
        });
        var state = new LayerState(Seq<Binding>(), Option<Item.Item>.Some(funcItem), " ");
        var strategy = new ApplyTextToConstraintStrategy();
        // act
        var result = strategy.GetGroups(state);
        Assert.Empty(result);
    }
}
