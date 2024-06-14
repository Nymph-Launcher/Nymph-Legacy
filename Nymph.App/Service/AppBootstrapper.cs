using System.ComponentModel;
using Autofac;
using Nymph.Model.Item;
using Nymph.Plugin.Everything;
using Nymph.Shared.ViewModel.GroupViewModel;
using Nymph.Shared.ViewModel.ItemViewModel;
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
        Locator.CurrentMutable.Register(() => new EverythingSearchItemView(), typeof(IViewFor<FunctionItemViewModel<AtomItem<string>, AtomItem<FileInfo>>>));
        Locator.CurrentMutable.Register(() => new EverythingFileInfoItemView(), typeof(IViewFor<AtomItemViewModel<FileInfo>>));
        // Register overriden ViewLocator
        // Locator.CurrentMutable.UnregisterCurrent(typeof(IViewLocator));
        Locator.CurrentMutable.InitializeReactiveUI();
        Locator.CurrentMutable.RegisterLazySingleton(() => new NymphViewLocator(), typeof(IViewLocator));
        
        // Locator.CurrentMutable.InitializeReactiveUI();
    }
}