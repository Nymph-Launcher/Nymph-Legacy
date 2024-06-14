using System.Reactive.Disposables;
using System.Windows.Controls;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.GroupViewModel;
using Octokit;
using ReactiveUI;

namespace Nymph.Plugin.GitHub;

public partial class GitHubRepositoryPreviewView : ReactiveUserControl<IItemPreviewViewModel>
{
    public GitHubRepositoryPreviewView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            this.OneWayBind(ViewModel,
                    vm => vm.GetItem,
                    v => v.FullName.Text,
                    item => (item as AtomItem<Repository>)?.Value.FullName)
                .DisposeWith(d);

            this.OneWayBind(ViewModel,
                    vm => vm.GetItem,
                    v => v.DescriptionBlock.Text,
                    item => (item as AtomItem<Repository>)?.Value.Description)
                .DisposeWith(d);
        });
    }
}