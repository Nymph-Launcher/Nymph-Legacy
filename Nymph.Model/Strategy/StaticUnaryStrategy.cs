using System.Reflection;
using LanguageExt;
using static LanguageExt.Prelude;
using Nymph.Model.Item;
using Nymph.Model.Group;

namespace Nymph.Model.Strategy;

public class StaticUnaryStrategy : IStrategy
{
    public Seq<Group.Group> GetGroups(LayerState state)
    {
        return state.Item
            .Some(stateItem => state.Bindings
                .Map(binding => binding.Item)
                .Filter(item => item is FunctionItem)
                .Filter(item => stateItem.GetType().IsAssignableFrom(item.GetType().GetGenericArguments()[0]))
                .Map(item =>
                {
                    var genericTypes = item.GetType().GetGenericArguments();
                    var unaryFuncType = typeof(StaticUnaryFunctionGroup<,>).MakeGenericType(genericTypes);
                    var unaryFuncs = (Group.Group?)Activator.CreateInstance(unaryFuncType, [item, stateItem]);
                    return unaryFuncs == null ? Option<Group.Group>.None : Option<Group.Group>.Some(unaryFuncs);
                })
                .Sequence()
                .Some(groups => groups)
                .None(Seq<Group.Group>()))
            .None(Seq<Group.Group>());
    }
}