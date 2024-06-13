using System.Reactive.Disposables;
using System.Windows.Controls;
using ReactiveUI;

namespace Nymph.App;

public partial class ItemGroupView
{
    public ItemGroupView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            this.OneWayBind(ViewModel,
                    vm => vm.Preview,
                    v => v.Preview.ViewModel)
                .DisposeWith(d);

            this.OneWayBind(ViewModel,
                    vm => vm.Items,
                    v => v.ItemsList.ItemsSource)
                .DisposeWith(d);
        });
    }
}