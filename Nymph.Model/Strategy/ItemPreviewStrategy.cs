using LanguageExt;
using static LanguageExt.Prelude;
using Nymph.Model.Item;
using Nymph.Model.Group;
using Nymph.Model.Strategy;

namespace Nymph.Model.Strategy;

public class ItemPreviewStrategy : IStrategy
{
    public Seq<Group.Group> GetGroups(LayerState state)
    {
        return state.Item
            .Bind(item =>
            {
                var itemGroupType = typeof(ItemGroup<>).MakeGenericType(item.GetType());
                var group = (Group.Group?)Activator.CreateInstance(itemGroupType, item);
                return group == null ? Option<Group.Group>.None : Option<Group.Group>.Some(group);
            })
            .Some(group => Seq<Group.Group>([group]))
            .None(Seq<Group.Group>());
    }
}