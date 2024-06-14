using System.IO;
using System.Reactive.Disposables;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.GroupViewModel;
using ReactiveUI;

namespace Nymph.Plugin.Everything;

public partial class EverythingFileInfoPreviewView : ReactiveUserControl<IItemPreviewViewModel>
{
    public EverythingFileInfoPreviewView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            this.OneWayBind(ViewModel,
                vm => vm.GetItem,
                v => v.TypeBlock.Text,
                GetType)
                .DisposeWith(d);

            this.OneWayBind(ViewModel,
                    vm => vm.GetItem,
                    v => v.NameBlock.Text,
                    item => (item as AtomItem<FileInfo>)?.Value.Name)
                .DisposeWith(d);
            
            this.OneWayBind(ViewModel,
                    vm => vm.GetItem,
                    v => v.PathBlock.Text,
                    item => (item as AtomItem<FileInfo>)?.Value.Path)
                .DisposeWith(d);
        });
    }

    private string GetType(Item item)
    {
        var result = "";
        if (item is not AtomItem<FileInfo> fileItem) return result;
        result = "Unknown";
        var path = fileItem.Value.Path;
        if (File.Exists(path)) result = "File";
        if (Directory.Exists(path)) result = "Dir";

        return result;
    }
}