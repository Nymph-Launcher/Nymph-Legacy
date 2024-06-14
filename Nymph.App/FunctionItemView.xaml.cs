using System.Windows.Controls;
using ReactiveUI;

namespace Nymph.App;

public partial class FunctionItemView
{
    public FunctionItemView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            this.OneWayBind(ViewModel,
                vm => vm.Description,
                v => v.Description.Text);
        });
    }
}