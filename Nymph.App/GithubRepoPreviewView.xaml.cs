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
                    vm => vm,
                    v => v.GithubPreviewBlock.Text,
                    vm => (vm as ItemPreviewViewModel<AtomItem<string>>)?.Item.Value ?? "")
                .DisposeWith(d);
        });
    }
}