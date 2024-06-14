using Nymph.Shared.ViewModel.GroupViewModel;
using Nymph.Model.Item;
using Nymph.Plugin.Everything;
using Nymph.Shared.ViewModel.ItemViewModel;
using ReactiveUI;
using Splat;

namespace Nymph.App.Service;

public class NymphViewLocator : IViewLocator
{
    public IViewFor? ResolveView<T>(T? viewModel, string? contract = null)
    {
        if (viewModel is null) return null;
        var serviceView = Locator.Current.GetService(typeof(IViewFor<>).MakeGenericType(viewModel.GetType()));
        if (serviceView is not null) return serviceView as IViewFor;

        var generatedView = viewModel switch
        {
            IAtomItemViewModel => new AtomItemView() as IViewFor,
            IListItemViewModel => new ListItemView(),
            IPathItemViewModel => new PathItemView(),
            RecordItemViewModel => new RecordItemView(),
            IFunctionItemViewModel => new FunctionItemView(),
            IBinaryFunctionGroupViewModel => new BinaryFunctionGroupView(),
            IDynamicUnaryFunctionGroupViewModel => new DynamicUnaryFunctionGroupView(),
            IStaticUnaryFunctionGroupViewModel => new StaticUnaryFunctionGroupView(),
            IListGroupViewModel => new ListGroupView(),
            IItemGroupViewModel => new ItemGroupView(),
            IItemPreviewViewModel => new ItemPreviewView(),
            CandidateItemViewModel => new CandidateItemView(),
            ConstraintItemViewModel => new ConstraintItemView(),
            _ => null
        };
        return generatedView;
    }
}