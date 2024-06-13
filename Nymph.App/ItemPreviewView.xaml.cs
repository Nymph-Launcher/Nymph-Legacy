using System.Reactive.Disposables;
using System.Windows.Controls;
using ReactiveUI;

namespace Nymph.App;

public partial class ItemPreviewView
{
    public ItemPreviewView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            this.OneWayBind(ViewModel,
                    vm => vm.GetItem,
                    v => v.ItemValue.Text,
                    item => item.ToString())
                .DisposeWith(d);
        });
    }
}