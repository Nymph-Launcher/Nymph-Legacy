using System.Reactive.Disposables;
using System.Windows.Controls;
using Newtonsoft.Json.Linq;
using Nymph.Model.Item;
using Nymph.Shared.ViewModel.GroupViewModel;
using ReactiveUI;

namespace Nymph.Plugin.JQ;

public partial class JTokenPreviewItem : ReactiveUserControl<IItemPreviewViewModel>
{
    public JTokenPreviewItem()
    {
        InitializeComponent();
        this.WhenActivated(d =>
        {
            this.OneWayBind(ViewModel,
                    vm => vm.GetItem,
                    v => v.JsonTreeView.ItemsSource,
                    GetTreeViewItems)
                .DisposeWith(d);
        });
    }
    
    private static ItemCollection GetTreeViewItems(Item item)
    {
        var json = (item as AtomItem<JToken>)?.Value;
        var root = new TreeViewItem{Header = "root"};
        if (json is null)
        {
            return new TreeView().Items;
        }
        root.IsExpanded = true;
        PopulateTreeView(json, root);
        var result = new TreeView();
        result.Items.Add(root);
        return result.Items;
    }
    
    private static void PopulateTreeView(JToken json, TreeViewItem parent)
    {
        switch (json)
        {
            case JObject obj:
            {
                foreach (var property in obj.Properties())
                {
                    var item = new TreeViewItem { Header = property.Name };
                    parent.Items.Add(item);
                    PopulateTreeView(property.Value, item);
                }

                break;
            }
            case JArray array:
            {
                for (int i = 0; i < array.Count; i++)
                {
                    var item = new TreeViewItem { Header = $"[{i}]" };
                    parent.Items.Add(item);
                    PopulateTreeView(array[i], item);
                }

                break;
            }
            default:
            {
                var item = new TreeViewItem { Header = json.ToString() };
                parent.Items.Add(item);
                break;
            }
        }
    }
}