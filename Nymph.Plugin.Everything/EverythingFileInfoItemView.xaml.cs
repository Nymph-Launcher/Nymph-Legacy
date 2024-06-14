using System.Reactive.Disposables;
using System.Windows.Controls;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.ItemViewModel;
using ReactiveUI;

namespace Nymph.Plugin.Everything;

public partial class EverythingFileInfoItemView
{
    public EverythingFileInfoItemView()
    {
        InitializeComponent();
        this.WhenActivated(d =>
        {
            this.OneWayBind(ViewModel,
                    vm => vm.GetItem,
                    v => v.FileNameBlock.Text,
                    item => (item as AtomItem<FileInfo>)?.Value.Name ?? ""
                )
                .DisposeWith(d);
            this.OneWayBind(ViewModel, 
                    vm => vm.GetItem, 
                    v => v.FilePathBlock.Text,
                    item => (item as AtomItem<FileInfo>)?.Value.Path ?? ""
                    )
                .DisposeWith(d);
        });
    }
}