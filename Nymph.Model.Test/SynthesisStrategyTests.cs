using LanguageExt;
using static LanguageExt.Prelude;
using Nymph.Model.Group;
using Nymph.Model.Item;
using Nymph.Model.Strategy;


namespace Nymph.Model.Test;


public class SynthesisStrategyTests : IDisposable
{
    private AtomItem<string> atomItemString = new("hello");
    private AtomItem<int> atomItemInt = new(42);

    private FunctionItem<AtomItem<string>, AtomItem<string>> funcStringString
        = new(async _ =>
        {
            await Task.Delay(100);
            return Seq([new AtomItem<string>("test")]);
        });
    private FunctionItem<AtomItem<string>, FunctionItem<AtomItem<string>, AtomItem<string>>> funcStringFunc
        = new(async param =>
        {
            await Task.Delay(100);
            return Seq([new FunctionItem<AtomItem<string>, AtomItem<string>>(
                async _ =>
                {
                    await Task.Delay(100);
                    return Seq([new AtomItem<string>("Hello, world!")]);
                })]);
        });
    
    private FunctionItem<AtomItem<int>, AtomItem<int>> funcIntInt 
        = new(async _ =>
        {
            await Task.Delay(100);
            return Seq([new AtomItem<int>(12)]);
        });
    private FunctionItem<AtomItem<int>, FunctionItem<AtomItem<string>, AtomItem<string>>> funcIntFunc
        = new(async param =>
        {
            await Task.Delay(100);
            return Seq([new FunctionItem<AtomItem<string>, AtomItem<string>>(
                async _ =>
                {
                    await Task.Delay(100);
                    return Seq([new AtomItem<string>("Hello, world!")]);
                })]);
        });

    private IStrategy strategy = new SynthesisStrategy();

    private Seq<Binding> allBinding;

    public SynthesisStrategyTests()
    {
        allBinding = Seq([
            new Binding("atomItemString", atomItemString),
            new Binding("atomItemInt", atomItemInt),
            new Binding("funcStringString", funcStringString),
            new Binding("funcStringFunc", funcStringFunc),
            new Binding("funcIntInt", funcIntInt),
            new Binding("funcIntFunc", funcIntFunc)
        ]);
    }

    public void Dispose()
    {
    }

    [Fact]
    public void GetGroups_ReturnGroups_WhenStateTextProperAndItemIsString()
    {
        var state = new LayerState(allBinding, Option<Item.Item>.Some(atomItemString), "hello");
        var result = strategy.GetGroups(state);
        Assert.NotEmpty(result);
        Assert.Equal(6, result.Length);
        Assert.IsAssignableFrom<BinaryFunctionGroup>(result[0]); // binaryFunction
        Assert.IsAssignableFrom<ItemGroup>(result[1]); // itemPreview
        Assert.IsAssignableFrom<StaticUnaryFunctionGroup>(result[2]); // staticUnary
        Assert.IsAssignableFrom<StaticUnaryFunctionGroup>(result[3]); // staticUnary
        Assert.IsAssignableFrom<DynamicUnaryFunctionGroup>(result[4]); // dynamicUnary
        Assert.IsAssignableFrom<DynamicUnaryFunctionGroup>(result[5]); // dynamicUnary
    }

    [Fact]
    public void GetGroups_ReturnGroups_WhenStateTextProperAndItemIsFunc()
    {
        var state = new LayerState(allBinding, Option<Item.Item>.Some(funcStringString), "hello");
        var result = strategy.GetGroups(state);
        Assert.NotEmpty(result);
        Assert.Equal(5, result.Length);
        Assert.IsAssignableFrom<DynamicUnaryFunctionGroup>(result[0]); // applyTextToConstraint
        Assert.IsAssignableFrom<ItemGroup>(result[1]); // itemPreview
        Assert.IsAssignableFrom<ItemGroup>(result[2]); // findParameter
        Assert.IsAssignableFrom<DynamicUnaryFunctionGroup>(result[3]); // dynamicUnary
        Assert.IsAssignableFrom<DynamicUnaryFunctionGroup>(result[4]); // dynamicUnary
    }

    [Fact]
    public void GetGroups_ReturnGroups_WhenStateTextMissingAndItemIsFunc()
    {
        var state = new LayerState(allBinding, Option<Item.Item>.Some(funcStringString), " ");
        var result = strategy.GetGroups(state);
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Length);
        Assert.IsAssignableFrom<ItemGroup>(result[0]); // itemPreview
        Assert.IsAssignableFrom<ItemGroup>(result[1]); // findParameter
    }
    
    [Fact]
    public void GetGroups_ReturnGroups_WhenStateTextMissingAndItemIsString()
    {
        var state = new LayerState(allBinding, Option<Item.Item>.Some(atomItemString), " ");
        var result = strategy.GetGroups(state);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Length);
        Assert.IsAssignableFrom<ItemGroup>(result[0]); // itemPreview
        Assert.IsAssignableFrom<StaticUnaryFunctionGroup>(result[1]); // staticUnary
        Assert.IsAssignableFrom<StaticUnaryFunctionGroup>(result[2]); // staticUnary
    }

    [Fact]
    public void GetGroups_ReturnGroups_WhenStateTextProperAndItemEmpty()
    {
        var state = new LayerState(allBinding, Option<Item.Item>.None, "hello");
        var result = strategy.GetGroups(state);
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Length);
        Assert.IsAssignableFrom<DynamicUnaryFunctionGroup>(result[0]); // dynamicUnary
        Assert.IsAssignableFrom<DynamicUnaryFunctionGroup>(result[1]); // dynamicUnary
    }
    
    [Fact]
    public void GetGroups_ReturnGroups_WhenStateTextMissingAndItemEmpty()
    {
        var state = new LayerState(allBinding, Option<Item.Item>.None, " ");
        var result = strategy.GetGroups(state);
        Assert.NotEmpty(result);
        Assert.Equal(6, result.Length);
    }
}