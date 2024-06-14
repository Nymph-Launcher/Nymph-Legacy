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
                param => Task.Run(() => Seq<AtomItem<string>>([new("Hello, " + param.Value)])),
                "Say Hello before the parameter")),
            new Binding("a bin func",
                new FunctionItem<AtomItem<string>, FunctionItem<AtomItem<string>, AtomItem<string>>>(
                    param1 => Task.FromResult(Seq<FunctionItem<AtomItem<string>, AtomItem<string>>>([
                        new FunctionItem<AtomItem<string>, AtomItem<string>>(param2 =>
                            Task.FromResult(Seq<AtomItem<string>>([new AtomItem<string>($"{param1.Value},{param2.Value}")])),
                            $"Combine string with ${param1}")
                    ])),
                    "Combine two strings"))
        ], new SynthesisStrategy());
    }
}