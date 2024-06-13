using System.Configuration;
using System.Data;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows;
using LanguageExt;
using static LanguageExt.Prelude;
using Nymph.App.Service;
using Nymph.Model.Group;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.GroupViewModel;
using Nymph.Shared.ViewModel.ItemViewModel;
using ReactiveUI;
using Splat;
using Group = System.Text.RegularExpressions.Group;

namespace Nymph.App;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    public App()
    {
        
        
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        AppBootstrapper.BootstrapApplication();
        var locator = Locator.Current.GetService<IViewLocator>();
        if (locator is null) return;
        // var githubPreview = locator.ResolveView(new ItemPreviewViewModel<AtomItem<string>>(new("Hello, github!")));
        // var atomItemView = locator.ResolveView(new ItemViewModelBuilder().Build(new AtomItem<string>("Hello, atom!")));
        //  var pathItemView =
        //       locator.ResolveView(
        //           new ItemViewModelBuilder().Build(new PathItem<AtomItem<int>, AtomItem<int>>(new(1), new(2))));
        //   var itemGroupView =
        //       locator.ResolveView(
        //           new GroupViewModelBuilder(Observable.Return(new AtomItem<string>("123"))).Build(
        //               new ItemGroup<AtomItem<int>>(new(10))));
        //   var listViewGroup =
        //       locator.ResolveView(
        //           new GroupViewModelBuilder(Observable.Return(new AtomItem<string>("123"))).Build(
        //               new ListGroup<AtomItem<int>>(new ListItem<AtomItem<int>>([new(10)]))));
        //   var staticUnaryFunctionGroupView = locator.ResolveView(
        //       new GroupViewModelBuilder(Observable.Return(new AtomItem<string>("123"))).Build(
        //           new StaticUnaryFunctionGroup<AtomItem<int>, AtomItem<int>>(
        //               new(_ => Task.Run(() => Seq<AtomItem<int>>([new(10)]))), new(10))));      var listItemView = locator.ResolveView(new ItemViewModelBuilder().Build(new ListItem<AtomItem<string>>([])));
 
        
    }
}