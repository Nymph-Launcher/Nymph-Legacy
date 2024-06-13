using System.Reactive.Disposables;
using System.Windows.Controls;
using ReactiveUI;

namespace Nymph.App;

public partial class PathItemView
{
    public PathItemView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            this.OneWayBind(ViewModel,
                    vm => vm.GetDecoratedItem,
                    v => v.DecoratedItem.ViewModel)
                .DisposeWith(d);
            this.OneWayBind(ViewModel,
                    vm => vm.GetDecorator,
                    v => v.Decorator.ViewModel)
                .DisposeWith(d);
        });
    }
}