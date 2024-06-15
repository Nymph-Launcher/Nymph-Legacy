using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using Nymph.App.Service;
using Nymph.Shared.ViewModel;
using ReactiveUI;

namespace Nymph.App;

public partial class MainView
{
    public MainView()
    {
        InitializeComponent();

        ViewModel = MainViewModelBootstrapper.CreateMainViewModel();

        this.WhenActivated(d =>
        {
            this.OneWayBind(ViewModel,
                vm => vm.ConstraintItemViewModel,
                v => v.ConstraintItem.ViewModel,
                optionalConstraintItem => optionalConstraintItem.MatchUnsafe(
                    Some: constraint => constraint,
                    None: () => null
                ))
                .DisposeWith(d);

            this.Bind(ViewModel,
                vm => vm.SearchText,
                v => v.SearchBox.Text)
                .DisposeWith(d);
            
            this.OneWayBind(ViewModel,
                vm => vm.GroupViewModels,
                v => v.GroupsListBox.ItemsSource)
                .DisposeWith(d);

            this.BindCommand(ViewModel,
                vm => vm.ClearConstraintCommand,
                v => v.ClearConstraintButton);

            this.OneWayBind(ViewModel,
                vm => vm.ConstraintItemViewModel,
                v => v.ConstraintPanel.Visibility,
                optionalConstraintItem => optionalConstraintItem.MatchUnsafe(
                    Some: _ => Visibility.Visible,
                    None: () => Visibility.Collapsed
                ));
        });
    }
}