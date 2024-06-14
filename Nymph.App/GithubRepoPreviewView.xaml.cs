using System.Reactive.Disposables;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.GroupViewModel;
using ReactiveUI;

namespace Nymph.App;

public partial class GithubRepoPreviewView : ReactiveUserControl<IItemPreviewViewModel>
{
    public GithubRepoPreviewView()
    {
        InitializeComponent();
        this.WhenActivated(d =>
        {
            this.OneWayBind(ViewModel,
                    vm => vm.GetItem,
                    v => v.GithubPreviewBlock.Text,
                    item => "Preview" + (item as AtomItem<string>)?.Value)
                .DisposeWith(d);
        });
    }
}