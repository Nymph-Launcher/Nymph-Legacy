using System.Reactive.Disposables;
using System.Windows.Controls;
using Nymph.Shared.ViewModel.ItemViewModel;
using ReactiveUI;

namespace Nymph.Plugin.GitHub;

public partial class GitHubCodeItemView : ReactiveUserControl<IAtomItemViewModel>
{
    public GitHubCodeItemView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            this.OneWayBind(ViewModel,
                    vm => vm.GetValue,
                    v => v.PathBlock.Text,
                    value => (value as Octokit.SearchCode)?.Path)
                .DisposeWith(d);
        });
    }
}