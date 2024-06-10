using LanguageExt;
using static LanguageExt.Prelude;
using Nymph.Model.Item;
using Nymph.Model.Group;
using Nymph.Model.Strategy;

namespace Nymph.Model.Test;

public class DefaultStrategyTests
{
    [Fact]
    public void GetGroups_ReturnItemGroups_WhenStateItemAndTextEmpty()
    {
        // arrange
        var item = new AtomItem<string>("hello");
        var state = new LayerState(Seq<Binding>([
            new Binding("hello", item),
            new Binding("world", item)
        ]), Option<Item.Item>.None, " ");
        var strategy = new DefaultStrategy();
        //act 
        var result = strategy.GetGroups(state);
        
        //assert
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Length);
        Assert.All(result, group =>
        {
            var itemGroup = (group as ItemGroup<AtomItem<string>>) ?? new ItemGroup<AtomItem<string>>(new AtomItem<string>(""));
            Assert.Equivalent((itemGroup.GetItem() as AtomItem<string>).GetValue(), "hello");
        });
    }

    [Fact]
    public void GetGroups_ReturnEmpty_WhenStateItemNotEmpty()
    {
        var item = new AtomItem<string>("hello");
        var state = new LayerState(Seq<Binding>([
            new Binding("hello", item),
            new Binding("world", item)
        ]), Option<Item.Item>.Some(new AtomItem<string>("")), " ");
        var strategy = new DefaultStrategy();

        var result = strategy.GetGroups(state);
        
        Assert.Empty(result);
    }

    [Fact]
    public void GetGroups_ReturnEmpty_WhenStateTextEmpty()
    {
        var item = new AtomItem<string>("hello");
        var state = new LayerState(Seq<Binding>([
            new Binding("hello", item),
            new Binding("world", item)
        ]), Option<Item.Item>.None, "world");
        var strategy = new DefaultStrategy();

        var result = strategy.GetGroups(state);
        
        Assert.Empty(result);
    }
}