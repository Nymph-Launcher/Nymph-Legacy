using System.Reactive.Disposables;
using System.Windows.Controls;
using ReactiveUI;

namespace Nymph.App;

public partial class BinaryFunctionGroupView
{
    public BinaryFunctionGroupView()
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
                    v => v.ItemsList.ItemsSource)
                .DisposeWith(d);

            this.Bind(ViewModel,
                    vm => vm.AutoOperation,
                    v => v.AutoOperateSwitch.IsChecked)
                .DisposeWith(d);
        });
    }
}