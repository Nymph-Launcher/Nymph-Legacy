using System.Reactive.Disposables;
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
                    vm => vm.Description,
                    v => v.DescriptionBlock.Text)
                .DisposeWith(d);
            
            this.OneWayBind(ViewModel,
                vm => vm.Items.Count,
                v => v.CountBlock.Text)
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