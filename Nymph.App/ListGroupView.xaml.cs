using System.Reactive.Disposables;
using System.Windows.Controls;
using ReactiveUI;

namespace Nymph.App;

public partial class ListGroupView
{
    public ListGroupView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            this.OneWayBind(ViewModel,
                    vm => vm.Items.Count,
                    v => v.ItemsCount.Text)
                .DisposeWith(d);
            
            this.OneWayBind(ViewModel,
                    vm => vm.Items,
                    v => v.Items.ItemsSource)
                .DisposeWith(d);
        });
    }
}