using System.ComponentModel;
using Autofac;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.GroupViewModel;
using ReactiveUI;
using Splat;
using Splat.Autofac;


namespace Nymph.App.Service;

public static class AppBootstrapper
{
    public static void BootstrapApplication()
    {
        // Register specific view for complete viewmodel
        Locator.CurrentMutable.Register(() => new GithubRepoPreviewView(), typeof(IViewFor<ItemPreviewViewModel<AtomItem<string>>>));
        
        // Register overriden ViewLocator
        // Locator.CurrentMutable.UnregisterCurrent(typeof(IViewLocator));
        Locator.CurrentMutable.InitializeReactiveUI();
        Locator.CurrentMutable.RegisterLazySingleton(() => new NymphViewLocator(), typeof(IViewLocator));
        
        // Locator.CurrentMutable.InitializeReactiveUI();
    }
}