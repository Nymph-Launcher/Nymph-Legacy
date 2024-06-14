using System.ComponentModel;
using Autofac;
using Nymph.Model.Item;
using Nymph.Plugin.Everything;
using Nymph.Plugin.GitHub;
using Nymph.Shared.ViewModel.GroupViewModel;
using Nymph.Shared.ViewModel.ItemViewModel;
using Octokit;
using ReactiveUI;
using Splat;
using Splat.Autofac;


namespace Nymph.App.Service;

public static class AppBootstrapper
{
    public static void BootstrapApplication()
    {
        // Register specific view for complete viewmodel
        Locator.CurrentMutable.Register(() => new EverythingFileInfoItemView(), typeof(IViewFor<AtomItemViewModel<FileInfo>>));
        Locator.CurrentMutable.Register(() => new EverythingFileInfoPreviewView(), typeof(IViewFor<ItemPreviewViewModel<AtomItem<FileInfo>>>));
        
        Locator.CurrentMutable.Register(() => new GitHubRepositoryItemView(), typeof(IViewFor<AtomItemViewModel<Repository>>));
        Locator.CurrentMutable.Register(() => new GitHubRepositoryPreviewView(), typeof(IViewFor<ItemPreviewViewModel<AtomItem<Repository>>>));
        Locator.CurrentMutable.Register(() => new GitHubCodeItemView(),
            typeof(IViewFor<AtomItemViewModel<Octokit.SearchCode>>));
        Locator.CurrentMutable.Register(() => new GitHubCodePreviewView(),
            typeof(IViewFor<ItemPreviewViewModel<AtomItem<Octokit.SearchCode>>>));
        // Register overriden ViewLocator
        // Locator.CurrentMutable.UnregisterCurrent(typeof(IViewLocator));
        Locator.CurrentMutable.InitializeReactiveUI();
        Locator.CurrentMutable.RegisterLazySingleton(() => new NymphViewLocator(), typeof(IViewLocator));
        
        // Locator.CurrentMutable.InitializeReactiveUI();
    }
}