using System.Reactive.Disposables;
using System.Windows.Controls;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.GroupViewModel;
using ReactiveUI;

namespace Nymph.Plugin.GitHub;

public partial class GitHubCodePreviewView : ReactiveUserControl<IItemPreviewViewModel>
{
    public GitHubCodePreviewView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            this.OneWayBind(ViewModel,
                    vm => vm.GetItem,
                    v => v.NameBlock.Text,
                    item => (item as AtomItem<Octokit.SearchCode>)?.Value.Name)
                .DisposeWith(d);

            this.OneWayBind(ViewModel,
                    vm => vm.GetItem,
                    v => v.PathBlock.Text,
                    item => (item as AtomItem<Octokit.SearchCode>)?.Value.Path)
                .DisposeWith(d);

            this.OneWayBind(ViewModel,
                    vm => vm.GetItem,
                    v => v.Link.NavigateUri,
                    item => new Uri((item as AtomItem<Octokit.SearchCode>)?.Value.HtmlUrl ?? ""))
                .DisposeWith(d);
        });
    }
}