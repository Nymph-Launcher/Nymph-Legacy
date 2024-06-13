using System.Reactive.Disposables;
using System.Windows.Controls;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.GroupViewModel;
using Nymph.Shared.ViewModel.ItemViewModel;
using ReactiveUI;

namespace Nymph.App.Service;

public partial class GithubRepoPreviewView : ReactiveUserControl<IItemPreviewViewModel>
{
    public GithubRepoPreviewView()
    {
        InitializeComponent();
        this.WhenActivated(d =>
        {
            this.OneWayBind(ViewModel,
                    vm => ((ItemPreviewViewModel<AtomItem<string>>)vm).Item.Value,
                    v => v.GithubPreviewBlock.Text)
                .DisposeWith(d);
        });
    }
}