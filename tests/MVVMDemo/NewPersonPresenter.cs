
namespace MVVMDemo
{
    public interface INewPersonPresenter : Tiveria.Common.MVP.IPresenter
    {
        bool ShowNewPersonDialog();
        Person Person { get; }
    }


    class NewPersonPresenter : INewPersonPresenter
    {
        public bool ShowNewPersonDialog()
        {
            using (var view = new PersonViewForm())
            {
                var vm = new PersonViewModel(view);
//                view.ViewModel = vm;
//                view.InitializeUIBindings(vm);
                view.ShowDialog();
                Person = vm.Model;
                return vm.Model != null;
            }
        }

        public Person Person { get; private set; }

        public void Dispose()
        {
        }
    }
}