using System.Reflection;
using LanguageExt;
using static LanguageExt.Prelude;
using Nymph.Model.Item;
using Nymph.Model.Group;

namespace Nymph.Model.Strategy;

public class UnaryProcessStrategy : IStrategy
{
    public Seq<Group.Group> GetGroups(LayerState state)
    {
        return state.Item
            .Some(stateItem => state.Bindings
                .Map(binding => binding.Item)
                .Filter(item => item is FunctionItem)
                .Filter(item => item.GetType().GetGenericArguments()[0].IsAssignableFrom(stateItem.GetType()))
                .Map(item =>
                {
                    var genericTypes = item.GetType().GetGenericArguments();
                    var unaryFuncType = typeof(UnaryFunctionGroup<,>).MakeGenericType(genericTypes);
                    var unaryFuncs = (Group.Group?)Activator.CreateInstance(unaryFuncType, [item, stateItem]);
                    return unaryFuncs == null ? Option<Group.Group>.None : Option<Group.Group>.Some(unaryFuncs);
                })
                .Sequence()
                .Some(groups => groups)
                .None(Seq<Group.Group>()))
            .None(Seq<Group.Group>());
    }
}