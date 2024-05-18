using LanguageExt;
using Nymph.Model.Group;
using Nymph.Model.Item;
using static LanguageExt.Prelude;

namespace Nymph.Model.Test;

public class BinaryFunctionGroupTests
{
    [Fact]
    public async Task GetFinalResult_ReturnsExpectedResult_WhenGivenValidParameters()
    {
        var binaryFunctionGroup = new BinaryFunctionGroup<AtomItem<string>, AtomItem<string>, AtomItem<string>>(
            new FunctionItem<AtomItem<string>, FunctionItem<AtomItem<string>, AtomItem<string>>>(a => 
                Task.FromResult(
                    Seq1(new FunctionItem<AtomItem<string>, AtomItem<string>>(b => 
                    Task.FromResult(
                        Seq1(
                        new AtomItem<string>($"{a.Value}{b.Value}")))))
                    )), 
            new AtomItem<string>("Hello"));
        var param2 = new AtomItem<string>("World");

        var result = await binaryFunctionGroup.GetFinalResult(param2).IfNoneAsync(Task.FromResult(LanguageExt.Seq<Item.Item>.Empty));

        Assert.Single(result.Result);
        Assert.Equal("HelloWorld", ((AtomItem<string>)result.Result.Head).Value);
    }
}