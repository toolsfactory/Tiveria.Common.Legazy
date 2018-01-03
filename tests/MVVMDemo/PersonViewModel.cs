using System;
using Tiveria.Common.MVP;

namespace MVVMDemo
{
    public interface IPersonViewModel : IViewModel
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string FullName { get; }
        DateTime BirthDate { get; set; }
        bool ValidationOk { get; set; }
        bool Enabled { get; set; }
        void Save();
        void Cancel();
    }

    public class PersonViewModel : ValidatingViewModelBase<IPersonViewModel, IPersonView, Person>
    {
        private bool _Enabled;
        public PersonViewModel(IPersonView view)
        {
            View = view;
            Model = new Person();
        }

        public string FirstName
        {
            get { return Model.FirstName; }
            set
            {
                if (value == Model.FirstName) return;
                Model.FirstName = value;
                RaisePropertyChanged(() => this.FirstName);
                RaisePropertyChanged(() => this.FullName);
            }
        }

        public string LastName
        {
            get { return Model.LastName; }
            set
            {
                if (value == Model.LastName) return;
                Model.LastName = value;
                RaisePropertyChanged(() => this.LastName);
                RaisePropertyChanged(() => this.FullName);
            }
        }

        public string Email
        {
            get { return Model.Email; }
            set
            {
                if (value == Model.Email) return;
                Model.Email = value;
                RaisePropertyChanged(() => this.Email);
            }
        }

        public string FullName
        {
            get { return Model.FullName; }
        }

        public DateTime BirthDate
        {
            get { return Model.BirthDate; }
            set
            {
                if (value == Model.BirthDate) return;
                Model.BirthDate = value;
                RaisePropertyChanged(() => this.BirthDate);
            }
        }

        public bool Enabled
        {
            get
            {
                return _Enabled;
            }
            set
            {
                _Enabled = value;
                RaisePropertyChanged(() => this.Enabled);
            }
        }


        public void Save()
        {
            View.Close();
        }



        protected override void FilterValidationMessages()
        {
            ValidationOk = ValidationMessages.Count == 0;
        }



        public void Cancel()
        {
            Model = null;
            View.Close();
        }
    }
}
