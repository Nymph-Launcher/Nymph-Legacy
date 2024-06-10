using LanguageExt;
using Nymph.Model.Group;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.GroupViewModel;

namespace Nymph.Shared.Test;

[TestFixture]
public class GroupViewModelBuilderTests
{
    private GroupViewModelBuilder _builder;

    [SetUp]
    public void SetUp()
    {
        _builder = new GroupViewModelBuilder();
    }

    [Test]
    public void Build_WithItemGroup_ReturnsGroupViewModel()
    {
        var itemGroup = new ItemGroup<AtomItem<string>>(new AtomItem<string>("123"));

        var result = _builder.Build(itemGroup);
        
        Assert.IsInstanceOf<ItemGroupViewModel<AtomItem<string>>>(result);
    }
    
    [Test]
    public void Build_WithListGroup_ReturnsListGroupViewModel()
    {
        var listGroup =
            new ListGroup<AtomItem<string>>(
                new ListItem<AtomItem<string>>(new Seq<AtomItem<string>>([new AtomItem<string>("123")])));

        var result = _builder.Build(listGroup);

        Assert.IsInstanceOf<ListGroupViewModel<AtomItem<string>>>(result);
    }

    [Test]
    public void Build_WithStaticUnaryFunctionGroup_ReturnsStaticUnaryFunctionGroupViewModel()
    {
        var staticUnaryFunctionGroup =
            new StaticUnaryFunctionGroup<AtomItem<string>, AtomItem<string>>(
                new FunctionItem<AtomItem<string>, AtomItem<string>>(input => Task.FromResult(new Seq<AtomItem<string>>([new AtomItem<string>(input.Value)]))),
                new AtomItem<string>("Hello from input"));

        var result = _builder.Build(staticUnaryFunctionGroup);

        Assert.IsInstanceOf<StaticUnaryFunctionGroupViewModel<AtomItem<string>,AtomItem<string>>>(result);
    }

    [Test]
    public void Build_WithDynamicUnaryFunctionGroup_ReturnsDynamicUnaryFunctionGroupViewModel()
    {
        var dynamicUnaryFunctionGroup = new DynamicUnaryFunctionGroup<AtomItem<int>>(
            new FunctionItem<AtomItem<string>, AtomItem<int>>(input =>
                Task.FromResult(new Seq<AtomItem<int>>([new AtomItem<int>(input.Value.Length)]))));

        var result = _builder.Build(dynamicUnaryFunctionGroup);

        Assert.IsInstanceOf<DynamicUnaryFunctionGroupViewModel<AtomItem<int>>>(result);
    }

    [Test]
    public void Build_WithBinaryFunctionGroup_ReturnsBinaryFunctionGroupViewModel()
    {
        var binaryFunctionGroup = new BinaryFunctionGroup<AtomItem<int>, AtomItem<string>, AtomItem<string>>(
            new FunctionItem<AtomItem<int>, FunctionItem<AtomItem<string>, AtomItem<string>>>(param1 =>
                Task.FromResult(new Seq<FunctionItem<AtomItem<string>, AtomItem<string>>>([
                    new FunctionItem<AtomItem<string>, AtomItem<string>>(param2 =>
                        Task.FromResult(new Seq<AtomItem<string>>([
                            new AtomItem<string>($"{param1.Value}{param2.Value}")
                        ])))
                ]))),
            new AtomItem<int>(132));

        var result = _builder.Build(binaryFunctionGroup);

        Assert.IsInstanceOf<BinaryFunctionGroupViewModel<AtomItem<int>, AtomItem<string>, AtomItem<string>>>(result);
    }

    [Test]
    public void Build_WithNullGroup_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentException>(() => _builder.Build(null));
    }
}