using System.Reactive.Disposables;
using System.Windows.Controls;
using ReactiveUI;

namespace Nymph.App;

public partial class DynamicUnaryFunctionGroupView
{
    public DynamicUnaryFunctionGroupView()
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
                    vm => vm.IsAutoExecute,
                    v => v.AutoSwitch.IsChecked)
                .DisposeWith(d);
        });
    }
}