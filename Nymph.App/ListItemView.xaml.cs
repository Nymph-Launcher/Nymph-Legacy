using System.Reactive.Disposables;
using System.Windows.Controls;
using ReactiveUI;

namespace Nymph.App;

public partial class ListItemView
{
    public ListItemView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            this.OneWayBind(ViewModel, 
                vm => vm.GetItems.Count,
                v => v.CountBlock.Text)
                .DisposeWith(d);
            this.OneWayBind(ViewModel, 
                vm => vm.GetItems,
                v => v.Items.ItemsSource)
                .DisposeWith(d);
        });
    }
}