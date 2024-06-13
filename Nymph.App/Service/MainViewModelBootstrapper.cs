using System.Windows.Data;
using Nymph.Model.Strategy;
using Nymph.Shared.ViewModel;
using LanguageExt;
using static LanguageExt.Prelude;
using Nymph.Model.Item;
using static LanguageExt.Prelude;
using Binding = Nymph.Model.Binding;

namespace Nymph.App.Service;

public static class MainViewModelBootstrapper
{
    public static MainViewModel CreateMainViewModel()
    {
        return new MainViewModel([
            new Binding("hello", new AtomItem<string>("hello")),
            new Binding("hello func", new FunctionItem<AtomItem<string>, AtomItem<string>>(
                param => Task.Run(() => Seq<AtomItem<string>>([new("Hello, " + param.Value)])))),
        ], new SynthesisStrategy());
    }
}