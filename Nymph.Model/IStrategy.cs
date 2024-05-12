using LanguageExt;

namespace Nymph.Model;

public interface IStrategy
{
    public Seq<Group.Group> GetGroups(LayerState state);
}