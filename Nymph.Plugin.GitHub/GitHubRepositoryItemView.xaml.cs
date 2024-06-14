using System.Reactive.Disposables;
using System.Windows.Controls;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.ItemViewModel;
using Octokit;
using ReactiveUI;

namespace Nymph.Plugin.GitHub;

public partial class GitHubRepositoryItemView : ReactiveUserControl<ItemViewModel>
{
    public GitHubRepositoryItemView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            this.OneWayBind(ViewModel,
                    vm => vm.GetItem,
                    v => v.FullName.Text,
                    item => (item as AtomItem<Repository>)?.Value.FullName)
                .DisposeWith(d);
        });
    }
}