using LanguageExt;
using static LanguageExt.Prelude;
using Nymph.Model.Item;
using Nymph.Model.Group;
using Nymph.Model.Strategy;

namespace Nymph.Model.Test;

public class ItemPreviewStrategyTests
{
    [Fact]
    public void GetGroups_ReturnItemGroup_WhenStateItemProper()
    {
        var item = new AtomItem<string>("hello");
        var state = new LayerState(Seq<Binding>(), Option<Item.Item>.Some(item), " ");
        var strategy = new ItemPreviewStrategy();

        var result = strategy.GetGroups(state);
        
        Assert.NotEmpty(result);
        Assert.All(result, group => Assert.Equal("hello", (group as ItemGroup<AtomItem<string>>).Item.GetValue()));
    }

    [Fact]
    public void GetGroups_ReturnEmpty_WhenStateItemNone()
    {
        var state = new LayerState(Seq<Binding>(), Option<Item.Item>.None, " ");
        var strategy = new ItemPreviewStrategy();

        var result = strategy.GetGroups(state);
        
        Assert.Empty(result);
    }
}