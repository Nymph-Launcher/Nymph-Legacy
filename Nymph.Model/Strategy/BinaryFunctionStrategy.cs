using LanguageExt;
using static LanguageExt.Prelude;
using Nymph.Model.Item;
using Nymph.Model.Group;

namespace Nymph.Model.Strategy;

public class BinaryFunctionStrategy : IStrategy
{
    public Seq<Group.Group> GetGroups(LayerState state)
    {
        return state.Item
            .Some(stateItem => state.Bindings
                .Map(binding => binding.Item)
                .Filter(item => item is FunctionItem)
                .Filter(item =>
                {
                    var generricTypes = item.GetType().GetGenericArguments();
                    if (generricTypes.Length != 2) return false;
                    if (!typeof(FunctionItem).IsAssignableFrom(generricTypes[1])) return false;
                    var tresultArgTypes = generricTypes[1].GetGenericArguments();
                    return generricTypes[0].IsAssignableFrom(stateItem.GetType())
                           && tresultArgTypes[0] == typeof(AtomItem<string>);
                })
                .Map(func =>
                {
                    var generricTypes = func.GetType().GetGenericArguments();
                    var tresultArgTypes = generricTypes[1].GetGenericArguments();
                    var binaryGroupType =
                        typeof(BinaryFunctionGroup<,,>).MakeGenericType([generricTypes[0], ..tresultArgTypes]);
                    var binaryGroup = (Group.Group?)Activator.CreateInstance(binaryGroupType, [func, stateItem]);
                    return binaryGroup == null ? Option<Group.Group>.None : Option<Group.Group>.Some(binaryGroup);
                })
                .Sequence()
                .Some(groups => groups)
                .None(Seq<Group.Group>()))
            .None(Seq<Group.Group>());
    }
}