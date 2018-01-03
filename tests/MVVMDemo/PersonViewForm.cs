using System.Windows.Forms;
using Tiveria.Common.MVP;

namespace MVVMDemo
{
    public partial class PersonViewForm : Form, IPersonView
    {
        public IPersonViewModel ViewModel { get; set; }

        private BindingManager<IPersonViewModel, IPersonView> _BM = new BindingManager<IPersonViewModel,IPersonView>();

        public PersonViewForm()
        {
            InitializeComponent();
        }

        public void InitializeUIBindings(IPersonViewModel viewModel)
        {
            ViewModel = viewModel;
            _BM.Connect(ViewModel, this);
            _BM.Bind(eFirstname, t => t.Text, vm => vm.FirstName, false, DataSourceUpdateMode.OnPropertyChanged);
            _BM.Bind(eLastname, t => t.Text, vm => vm.LastName, false, DataSourceUpdateMode.OnPropertyChanged);
            _BM.Bind(eEmail, t => t.Text, vm => vm.Email, false, DataSourceUpdateMode.OnPropertyChanged);
            _BM.Bind(eBirthdate, t => t.Value, vm => vm.BirthDate, false, DataSourceUpdateMode.OnPropertyChanged);
            _BM.Bind(lCombined, t => t.Text, vm => vm.FullName, false, DataSourceUpdateMode.OnPropertyChanged);
            _BM.Bind(bSave, t => t.Enabled, vm => vm.ValidationOk, false, DataSourceUpdateMode.OnPropertyChanged);

            /*
            _BM.Bind(eFirstname, t => t.Enabled, vm => vm.Enabled, false, DataSourceUpdateMode.OnPropertyChanged);
            _BM.Bind(eLastname, t => t.Enabled, vm => vm.Enabled, false, DataSourceUpdateMode.OnPropertyChanged);
             */

            eLastname.DataBindings.Add("Enabled", ViewModel, "Enabled");

        }

        public void ReleaseUIBindings()
        {
            //_BM.DisConnect();
        }

        public void SetControlValidationError(Control control, string error)
        {
            ep.SetError(control, error);
        }

        private void bSave_Click(object sender, System.EventArgs e)
        {
            ViewModel.Save();
        }

        private void bCancel_Click(object sender, System.EventArgs e)
        {
            ViewModel.Cancel();
        }

        private void cbEnabled_CheckedChanged(object sender, System.EventArgs e)
        {
            ViewModel.Enabled = cbEnabled.Checked;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            ViewModel.FirstName = "Test";
        }
    }
}
