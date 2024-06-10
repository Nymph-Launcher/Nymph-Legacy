using Nymph.Model.Item;
using LanguageExt;
using Nymph.Model.Group;
using Nymph.Model.Strategy;
using static LanguageExt.Prelude;

namespace Nymph.Model.Test;

public class FindParameterStrategyTests
{
    [Fact]
    public void GetGroups_ReturnFindParameter()
    {
        //arrange
        var funcItem = new FunctionItem<AtomItem<string>, AtomItem<string>>(
            async param =>
            {
                await Task.Delay(100);
                return Seq([new AtomItem<string>("Hello world")]);
            });
        var state = new LayerState(
            Seq<Binding>([new Binding("hello", funcItem), new Binding("world", new AtomItem<string>("123"))]),
            Option<Item.Item>.Some(funcItem),
            "text");
        var strategy = new FindParameterStrategy();
        
        //act
        var result = strategy.GetGroups(state);
        
        //assert
        Assert.NotEmpty(result);
        var group = result[0] as ItemGroup<AtomItem<string>>;
        Assert.NotNull(group);
        Assert.IsAssignableFrom<AtomItem<string>>(group.Item);
        Assert.Equal("123",group.Item.Value);
    }

    [Fact]
    public void GetGroups_ReturnFindParameter_2()
    {
        //arrange
        var funcItem = new FunctionItem<AtomItem<int>, AtomItem<string>>(
            async param =>
            {
                await Task.Delay(100);
                return Seq([new AtomItem<string>("hello world")]);
            });
        var state = new LayerState(
            Seq<Binding>([new Binding("hello", funcItem), new Binding("this",new AtomItem<int>(1)), new Binding("world", new ListItem<AtomItem<string>>([new AtomItem<string>("123"),new AtomItem<string>("456")]))]),
            Option<Item.Item>.Some(funcItem),
            "text");
        var strategy = new FindParameterStrategy();
        
        //act
        var result = strategy.GetGroups(state);
        
        //assert
        Assert.NotEmpty(result);
        var group = result[0] as ItemGroup<AtomItem<int>>;
        Assert.NotNull(group);
        Assert.IsAssignableFrom<AtomItem<int>>(group.Item);
        Assert.Equal(1,group.Item.Value);
    }
    
    
    [Fact]
    public void GetGroups_ReturnFindParameter_Fail()
    {
        //arrange
        var funcItem = new FunctionItem<AtomItem<string>, AtomItem<string>>(
            async param =>
            {
                await Task.Delay(100);
                return Seq([new AtomItem<string>("Hello world")]);
            });
        var state = new LayerState(
            Seq<Binding>([new Binding("hello", funcItem), new Binding("world", new AtomItem<string>("123"))]),
            Option<Item.Item>.Some(new AtomItem<string>("10086")),
            "text");
        var strategy = new FindParameterStrategy();
        
        //act
        var result = strategy.GetGroups(state);
        
        //assert
        Assert.Empty(result);
        // var group = result[0] as ItemGroup<AtomItem<string>>;
        // Assert.NotNull(group);
        // Assert.IsAssignableFrom<AtomItem<string>>(group.Item);
        // Assert.Equal("123",group.Item.Value);
        
    }
    
}

