using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows.Forms;
using Tiveria.Common.Extensions;
using System.Linq;

namespace Tiveria.Common.MVP
{
    public class BindingManager<TViewModel, TView> : IDisposable
        where TViewModel : IViewModel<TViewModel, TView>
        where TView : class, IView<TView, TViewModel>
    {
        private Dictionary<string, IBindableComponent> _AttachedControls = new Dictionary<string, IBindableComponent>();
        private Dictionary<string, Action> _BoundActions = new Dictionary<string, Action>();
        private List<Binding> _Bindings = new List<Binding>();
        private Command.CommandManager _CommandManager = new Command.CommandManager();

        public TViewModel ViewModel { get; private set; }
        public TView View { get; private set; }
        public Command.CommandManager Commands { get { return _CommandManager; } }

        public BindingManager() { }

        public BindingManager(TViewModel viewmodel, TView view)
        {
            Connect(viewmodel, view);
        }

        public void Dispose()
        {
            if (_CommandManager != null)
            {
                _CommandManager.Dispose();
                _CommandManager = null;
            }
        }

        public void Connect(TViewModel viewmodel, TView view)
        {
            if (viewmodel == null)
                throw new ArgumentNullException("ViewModel empty");

            if (view == null)
                throw new ArgumentNullException("View empty");

            if (View != null || ViewModel != null)
                DisConnect();

            ViewModel = viewmodel;
            View = view;
            ViewModel.View = View;

            if (ViewModel is IViewModelValidation)
                ((IViewModelValidation)ViewModel).Validated += ViewModel_Validated;

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            Refresh();
        }

        public void DisConnect()
        {
            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;

            foreach (var item in _Bindings)
            {
                item.Control.DataBindings.Remove(item);
            }
        }

        protected void ViewModel_Validated(object sender, EventArgs e)
        {
            _AttachedControls.ToList().ForEach(c => View.SetControlValidationError(c.Value as Control, ""));
            if (!string.IsNullOrEmpty(((IViewModelValidation)ViewModel).Error))
            {
                ((IViewModelValidation)ViewModel).ValidationMessages.ToList().ForEach(message =>
                {
                    if (_AttachedControls.ContainsKey(message.Key))
                        View.SetControlValidationError(_AttachedControls[message.Key] as Control, message.Value);
                });
            }
        }
        public Binding Bind<TControl, TDataSource>(
            TControl control, 
            Expression<Func<TControl, object>> controlPropertyAccessor,
            TDataSource datasource,
            Expression<Func<TDataSource, object>> datasourceMemberAccesor,
            bool formattingEnabled = false,
            DataSourceUpdateMode updateMode = DataSourceUpdateMode.OnPropertyChanged,
            bool autoValidate = true)
            where TControl : IBindableComponent
            where TDataSource : class
        {
            string controlPropertyName = controlPropertyAccessor.GetPropertyName();
            string sourcePropertyName = datasourceMemberAccesor.GetPropertyName();
            var binding = control.DataBindings.Add(
                controlPropertyName,
                datasource,
                sourcePropertyName,
                formattingEnabled,
                updateMode);

            return binding;
        }

        public Binding Bind<TControl, T1, T2>(
            TControl control,
            Expression<Func<TControl, T1>> propertyName,
            Expression<Func<TViewModel, T2>> dataMember,
            bool formattingEnabled = false,
            DataSourceUpdateMode updateMode = DataSourceUpdateMode.OnPropertyChanged,
            bool autoValidate = true)
            where TControl : IBindableComponent
        {
            var name = dataMember.GetPropertyName();
            if (!_AttachedControls.ContainsKey(name))
                _AttachedControls.Add(name, control);

            if (autoValidate && (ViewModel is IViewModelValidation))
            {
                (control as Control).Validating += (s, e) => { ((IViewModelValidation)ViewModel).Validate(); };
            }

            var binding = control.DataBindings.Add(
                propertyName.GetPropertyName(),
                ViewModel,
                dataMember.GetPropertyName(),
                formattingEnabled,
                updateMode);

            _Bindings.Add(binding);

            if (ViewModel is IViewModelValidation)
                ((IViewModelValidation)ViewModel).Validate();

            return binding;
        }

        public Binding Bind<TControl, T1, T2, T3, U>(
            TControl control,
            Expression<Func<TControl, T1>> propertyName,
            Expression<Func<TViewModel, T2>> dataMember,
            Func<T3, U> transform,
            DataSourceUpdateMode updateMode = DataSourceUpdateMode.OnPropertyChanged,
            bool autoValidate = true)
            where TControl : IBindableComponent
        {
            var binding = Bind(control, propertyName, dataMember, true, updateMode, autoValidate);
            binding.Format += (sender, e) => e.Value = transform((T3)e.Value);
            return binding;
        }



        public bool BindAction<T1>(Expression<Func<TViewModel, T1>> dataMember, Action action)
        {
            if (action == null || dataMember == null)
                throw new ArgumentNullException();

            var dm = dataMember.GetPropertyName();
            if (_BoundActions.ContainsKey(dm))
                throw new ArgumentException("Only one Action per ViewModel DataMember can be bound!");

            _BoundActions.Add(dm, action);

            return true;
        }

        public void Validate()
        {
            if (ViewModel is IViewModelValidation)
                ((IViewModelValidation)ViewModel).Validate();
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_BoundActions.ContainsKey(e.PropertyName))
                _BoundActions[e.PropertyName]();
            Validate();
        }

        public void Refresh()
        {
            foreach (var action in _BoundActions)
                action.Value();

            Commands.UpdateCommandsStates();
        }

    }
}
