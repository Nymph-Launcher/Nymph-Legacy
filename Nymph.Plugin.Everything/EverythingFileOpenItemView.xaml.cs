using System.Reactive.Disposables;
using System.Windows.Controls;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.ItemViewModel;
using ReactiveUI;

namespace Nymph.Plugin.Everything;

public partial class EverythingFileOpenItemView
{
    public EverythingFileOpenItemView()
    {
        InitializeComponent();
        this.WhenActivated(d =>
        {
            this.OneWayBind(ViewModel,
                    vm => vm,
                    v => v.FileNameBlock.Text,
                    vm => (vm as FunctionItemViewModel<AtomItem<string>, AtomItem<string>>)?.Item.Description ?? ""
                )
                .DisposeWith(d);
        });
    }
}