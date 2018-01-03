using System.Windows.Forms;

namespace Tiveria.Common.MVP
{
    public interface IView<TViewContract, TViewModelContract>
        where TViewContract : IView<TViewContract, TViewModelContract>
        where TViewModelContract : IViewModel<TViewModelContract, TViewContract>
    {
        TViewModelContract ViewModel { get; }
        void InitializeUIBindings(TViewModelContract viewModel);
        void ReleaseUIBindings();
        void SetControlValidationError(Control control, string error);
    } 
}
