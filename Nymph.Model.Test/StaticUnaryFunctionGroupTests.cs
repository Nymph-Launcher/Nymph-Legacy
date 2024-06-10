using LanguageExt;
using Nymph.Model.Group;
using Nymph.Model.Item;

namespace Nymph.Model.Test;

public class StaticUnaryFunctionGroupTests
{
    [Fact]
    public async Task GetFinalResult_ReturnsExpectedResult_WhenGivenValidParameters()
    {
        var staticUnaryFunctionGroup =
            new StaticUnaryFunctionGroup<AtomItem<string>, AtomItem<string>>(
                new FunctionItem<AtomItem<string>, AtomItem<string>>(input => Task.FromResult(new Seq<AtomItem<string>>([new AtomItem<string>(input.Value)]))),
                new AtomItem<string>("Hello from input"));

        var result = await staticUnaryFunctionGroup.GetSpecificResult();

        Assert.Single(result);
        Assert.Equal("Hello from input", result.Head.Value);
    }
}