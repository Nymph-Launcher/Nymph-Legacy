using LanguageExt;

namespace Nymph.Model.Strategy;

public interface IStrategy
{
    public Seq<Group.Group> GetGroups(LayerState state);
}