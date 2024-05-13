using LanguageExt;
using static LanguageExt.Prelude;
using Nymph.Model.Group;
using Nymph.Model.Item;
using Nymph.Model.Strategy;

namespace Nymph.Model.Test;

public class ListPreviewStrategyTests
{
    [Fact]
    public void GetGroups_ReturnsListGroup_WhenStateItemIsListItem()
    {
        var listItem = new ListItem<AtomItem<string>>(Seq<AtomItem<string>>([new AtomItem<string>("String Atom 1"), new AtomItem<string>("String Atom 2")])); // Replace SomeType with the actual type
        var state = new LayerState(Seq<Binding>(), Option<Item.Item>.Some(listItem), "");
        var strategy = new ListPreviewStrategy();

        var result = strategy.GetGroups(state);

        Assert.NotEmpty(result);
        Assert.All(result, group => Assert.IsType<ListGroup<AtomItem<string>>>(group)); // Replace SomeType with the actual type
    }

    [Fact]
    public void GetGroups_ReturnsEmpty_WhenStateItemIsNotListItem()
    {
        var item = new AtomItem<string>("String Atom Item"); // Replace this with a non-ListItem type
        var state = new LayerState(Seq<Binding>(), Option<Item.Item>.Some(item), "");
        var strategy = new ListPreviewStrategy();

        var result = strategy.GetGroups(state);

        Assert.Empty(result);
    }

    [Fact]
    public void GetGroups_ReturnsEmpty_WhenStateItemIsNone()
    {
        var state = new LayerState(Seq<Binding>(), Option<Item.Item>.None, "");
        var strategy = new ListPreviewStrategy();

        var result = strategy.GetGroups(state);

        Assert.Empty(result);
    }
}