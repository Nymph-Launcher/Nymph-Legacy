using System.Reactive.Disposables;
using System.Windows.Controls;
using ReactiveUI;

namespace Nymph.App;

public partial class StaticUnaryFunctionGroupView
{
    public StaticUnaryFunctionGroupView()
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

            this.BindCommand(ViewModel,
                    vm => vm.ExecuteFunc,
                    v => v.ExecuteButton)
                .DisposeWith(d);
        });
    }
}