using System.Reactive.Disposables;
using System.Windows.Controls;
using MahApps.Metro.IconPacks;
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
                v => v.Icon.Kind,
                GetItemType).DisposeWith(d);
        });
    }
    private static PackIconMaterialKind GetItemType(ItemViewModel itemViewModel)
    {
        return itemViewModel switch
        {
            IAtomItemViewModel => PackIconMaterialKind.Atom,
            IListItemViewModel => PackIconMaterialKind.ListBox,
            IPathItemViewModel => PackIconMaterialKind.ArrowRight,
            RecordItemViewModel => PackIconMaterialKind.Record,
            IFunctionItemViewModel => PackIconMaterialKind.Function,
            _ => PackIconMaterialKind.Null
        };
    }
}