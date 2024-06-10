using LanguageExt;
using static LanguageExt.Prelude;
using Nymph.Model.Item;
using Nymph.Model.Group;
using Nymph.Model.Strategy;

namespace Nymph.Model.Strategy;

public class DefaultStrategy : IStrategy
{
    public Seq<Group.Group> GetGroups(LayerState state)
    {
        return state.Item
            .Some(_ => Seq<Group.Group>())
            .None(!string.IsNullOrWhiteSpace(state.Text) 
                ? Seq<Group.Group>() 
                : state.Bindings
                    .Map(binding => binding.Item)
                    .Map(item =>
                    {
                        var type = item.GetType();
                        var group = (Group.Group?)Activator.CreateInstance(typeof(ItemGroup<>).MakeGenericType(type), item);
                        return group == null ? Option<Group.Group>.None : Option<Group.Group>.Some(group);
                    })
                    .Sequence()
                    .Some(group => group)
                    .None(Seq<Group.Group>())
            );
    }
}