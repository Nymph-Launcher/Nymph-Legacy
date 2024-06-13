using System.Windows.Controls;
using ReactiveUI;
using System.Reactive.Disposables;

namespace Nymph.App;

public partial class AtomItemView
{
    public AtomItemView()
    {
        InitializeComponent();
        this.WhenActivated(d =>
        {
            this.OneWayBind(ViewModel,
                    viewModel => viewModel.GetValue,
                    view => view.ValueBlock.Text,
                    x => x.ToString())
                .DisposeWith(d);
        });
    }
}