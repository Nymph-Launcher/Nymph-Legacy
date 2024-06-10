using LanguageExt;
using static LanguageExt.Prelude;
using Nymph.Model.Group;
using Nymph.Model.Item;

namespace Nymph.Model.Strategy;

public class SynthesisStrategy : IStrategy
{
    private DefaultStrategy _defaultStrategy = new DefaultStrategy();
    private BinaryFunctionStrategy _binaryFunctionStrategy = new BinaryFunctionStrategy();
    private ApplyTextToConstraintStrategy _applyTextToConstraintStrategy = new ApplyTextToConstraintStrategy();
    private DynamicUnaryStrategy _dynamicUnaryStrategy = new DynamicUnaryStrategy();
    private FindParameterStrategy _findParameterStrategy = new FindParameterStrategy();
    private ItemPreviewStrategy _itemPreviewStrategy = new ItemPreviewStrategy();
    private ListPreviewStrategy _listPreviewStrategy = new ListPreviewStrategy();
    private StaticUnaryStrategy _staticUnaryStrategy = new StaticUnaryStrategy();
    
    public Seq<Group.Group> GetGroups(LayerState state)
    {
        var finalGroupSeq = Seq<Group.Group>();
        var isTextExists = !string.IsNullOrWhiteSpace(state.Text);
        var isItemExists = state.Item.Some(_ => true).None(false);

        if (isTextExists && isItemExists)
        {
            finalGroupSeq += _applyTextToConstraintStrategy.GetGroups(state);
            finalGroupSeq += _binaryFunctionStrategy.GetGroups(state);
        }
        if (isItemExists)
        {
            finalGroupSeq += _itemPreviewStrategy.GetGroups(state);
            finalGroupSeq += _listPreviewStrategy.GetGroups(state);
            finalGroupSeq += _findParameterStrategy.GetGroups(state);
            finalGroupSeq += _staticUnaryStrategy.GetGroups(state);
        }
        if (isTextExists)
        {
            finalGroupSeq += _dynamicUnaryStrategy.GetGroups(state);
        }

        if (!isTextExists && !isItemExists)
            finalGroupSeq += _defaultStrategy.GetGroups(state);

        return finalGroupSeq;
    }
}