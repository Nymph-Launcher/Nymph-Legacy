using LanguageExt;
using static LanguageExt.Prelude;
using Nymph.Model.Item;
using Nymph.Model.Group;
using Nymph.Model.Strategy;

namespace Nymph.Model.Strategy;

public class ApplyTextToConstraintStrategy : IStrategy
{
    public Seq<Group.Group> GetGroups(LayerState state)
    {
        return state.Item
            .Bind(item => !string.IsNullOrWhiteSpace(state.Text)
                          && item is FunctionItem
                ? Option<Item.Item>.Some(item)
                : Option<Item.Item>.None)
            .Bind(item =>
            {
                var genericTypes = item.GetType().GetGenericArguments();
                if (genericTypes[0] != typeof(AtomItem<string>)) return Option<Group.Group>.None;
                var unaryFuncType = typeof(DynamicUnaryFunctionGroup<>).MakeGenericType(genericTypes[1]);
                var group = (Group.Group?)Activator.CreateInstance(unaryFuncType, item);
                return group == null ? Option<Group.Group>.None : Option<Group.Group>.Some(group);
            })
            .Some(group => Seq<Group.Group>([group]))
            .None(Seq<Group.Group>());
    }
}