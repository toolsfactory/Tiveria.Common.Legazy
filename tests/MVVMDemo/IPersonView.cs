using Tiveria.Common.MVP;

namespace MVVMDemo
{
    public interface IPersonView : IView<IPersonViewModel>
    {
        void Close();
    }
}
