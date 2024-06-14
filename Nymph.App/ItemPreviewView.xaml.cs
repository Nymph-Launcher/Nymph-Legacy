using System.Reactive.Disposables;
using System.Windows.Controls;
using Nymph.Model.Item;
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
                    ItemPreviewConverter)
                .DisposeWith(d);
        });
    }

    private static string ItemPreviewConverter(Item item)
    {
        return item switch
        {
            AtomItem atomItem => atomItem.GetValue().ToString() ?? "Null AtomItem",
            FunctionItem functionItem => string.Join(" => ", functionItem.GetType().GetGenericArguments().Select(t => t.Name)),
            _ => item.ToString()
        };
    }
}