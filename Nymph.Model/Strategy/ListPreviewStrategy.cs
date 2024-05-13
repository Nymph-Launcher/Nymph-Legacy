using LanguageExt;
using static LanguageExt.Prelude;
using Nymph.Model.Group;
using Nymph.Model.Helper;
using Nymph.Model.Item;

namespace Nymph.Model.Strategy;

public class ListPreviewStrategy : IStrategy
{
    public Seq<Group.Group> GetGroups(LayerState state)
    {
        return state.Item
            .Bind(item => item.IsInstanceOfGenericType(typeof(ListItem<>))
                ? Option<Item.Item>.Some(item)
                : Option<Item.Item>.None)
            .Bind(item =>
            {
                var typeParameter = item.GetType().GetGenericArguments()[0];
                var listGroupType = typeof(ListGroup<>).MakeGenericType(typeParameter);
                var listGroup = (Group.Group?)Activator.CreateInstance(listGroupType, item);
                return listGroup == null ? Option<Group.Group>.None : Option<Group.Group>.Some(listGroup);
            })
            .Some<Seq<Group.Group>>(listGroup => Seq<Group.Group>([listGroup]))
            .None(Seq<Group.Group>());
    }
}