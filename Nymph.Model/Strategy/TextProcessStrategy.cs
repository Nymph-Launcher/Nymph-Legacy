using System.Reflection;
using LanguageExt;
using Nymph.Model.Group;
using static LanguageExt.Prelude;
using Nymph.Model.Item;

namespace Nymph.Model.Strategy;

public class TextProcessStrategy:  IStrategy
{
    public Seq<Group.Group> GetGroups(LayerState state)
    {
        return Option<LayerState>.Some(state)
            .Bind(layerState => string.IsNullOrWhiteSpace(layerState.Text)
                ? Option<LayerState>.None
                : Option<LayerState>.Some(layerState))
            .Bind(layerState => layerState.Bindings
                .Map(binding => binding.Item)
                .Filter(item => item is FunctionItem)
                .Filter(item =>
                {
                    var genericTypes = item.GetType().GetGenericArguments();
                    return genericTypes.Length == 2
                           && genericTypes[0].IsAssignableFrom(typeof(AtomItem<string>))
                           && !genericTypes[1].IsAssignableFrom(typeof(FunctionItem));
                })
                .Bind(item =>
                {
                    var genericTypes = item.GetType().GetGenericArguments();
                    var unaryFuncGroupType =
                        typeof(UnaryFunctionGroup<,>).MakeGenericType(genericTypes);
                    var unaryFuncGroup =
                        (Group.Group?)Activator.CreateInstance(unaryFuncGroupType, [item, new AtomItem<string>(layerState.Text)]);
                    return unaryFuncGroup == null ? Option<Group.Group>.None : Option<Group.Group>.Some(unaryFuncGroup);
                })
            )
            .ToSeq();
    }
}