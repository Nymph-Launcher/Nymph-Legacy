using System.Reactive.Disposables;
using System.Windows.Controls;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.ItemViewModel;
using ReactiveUI;

namespace Nymph.App;

public partial class ConstraintItemView
{
    public ConstraintItemView()
    {
        InitializeComponent();
        this.WhenActivated(d =>
        {
            this.OneWayBind(ViewModel,
                vm => vm.ItemViewModel,
                v => v.ItemIcon.Text,
                GetItemType).DisposeWith(d);
        });
    }
    private static string GetItemType(ItemViewModel itemViewModel)
    {
        return itemViewModel switch
        {
            IAtomItemViewModel => "A",
            IListItemViewModel => "L",
            IPathItemViewModel => "P",
            RecordItemViewModel => "R",
            IFunctionItemViewModel => "F",
            _ => "N"
        };
    }
}