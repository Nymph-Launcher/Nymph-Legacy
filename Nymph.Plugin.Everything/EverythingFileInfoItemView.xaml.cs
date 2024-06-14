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
                    vm => vm.GetValue,
                    v => v.FileNameBlock.Text,
                    value => (value as FileInfo)?.Name ?? ""
                )
                .DisposeWith(d);
            this.OneWayBind(ViewModel, 
                    vm => vm.GetValue, 
                    v => v.FilePathBlock.Text,
                    value => (value as FileInfo)?.Path ?? ""
                    )
                .DisposeWith(d);
        });
    }
}