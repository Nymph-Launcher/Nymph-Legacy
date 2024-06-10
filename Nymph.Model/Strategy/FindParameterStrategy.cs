using System.Reflection;
using LanguageExt;
using static LanguageExt.Prelude;
using Nymph.Model.Group;
using Nymph.Model.Item;

namespace Nymph.Model.Strategy;

public class FindParameterStrategy : IStrategy
{
    public Seq<Group.Group> GetGroups(LayerState state)
    {
        return state.Item
            .Bind(item => item is FunctionItem
                ? Option<Item.Item>.Some(item)
                : Option<Item.Item>.None)
            .Some(stateItem => state.Bindings
                .Map(binding => binding.Item)
                .Filter(item => stateItem.GetType().GetGenericArguments()[0].IsAssignableFrom(item.GetType()))
                .Map(item =>
                {
                    //var genericType = item.GetType().GetGenericArguments();
                    var itemType = typeof(ItemGroup<>).MakeGenericType(item.GetType());
                    var items = (Group.Group?)Activator.CreateInstance(itemType, [item]);
                    return items == null ? Option<Group.Group>.None : Option<Group.Group>.Some(items);
                })
                .Sequence()
                .Some(groups => groups)
                .None(Seq<Group.Group>())
            )
            .None(Seq<Group.Group>());
            
            
            
            //(state.Item is FunctionItem ? Option<LayerState>.Some(state) : Option<LayerState>.None);

    }
}