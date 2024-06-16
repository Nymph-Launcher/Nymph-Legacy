using System.Reactive.Disposables;
using System.Windows.Controls;
using ReactiveUI;

namespace Nymph.App;

public partial class CandidateItemView
{
    public CandidateItemView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            this.BindCommand(ViewModel, vm => vm.Choose, v => v.ChooseButton)
                .DisposeWith(d);
            this.OneWayBind(ViewModel, vm => vm.ItemViewModel, v => v.ItemModelViewHost.ViewModel)
                .DisposeWith(d);
        });
    }
}