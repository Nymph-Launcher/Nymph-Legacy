using System.Reactive.Disposables;
using System.Windows.Controls;

using ReactiveUI;

namespace Nymph.App;

public partial class RecordItemView 
{
    public RecordItemView()
    {
        InitializeComponent();
        this.WhenActivated(d =>
        {
            this.OneWayBind(ViewModel, 
                    vm => vm.Properties, 
                    v => v.Properties.ItemsSource)
                .DisposeWith(d);
        });
    }
}