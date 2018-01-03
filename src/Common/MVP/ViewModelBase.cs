using System;
using System.Collections.Generic;
using System.ComponentModel;
using Tiveria.Common.Extensions;
using System.Linq;
using System.Linq.Expressions;

namespace Tiveria.Common.MVP
{

    public abstract class ViewModelBase<TViewModelContract, TViewContract> : NotifyBase, IViewModelValidation, IViewModel<TViewModelContract, TViewContract>
        where TViewContract : IView<TViewContract, TViewModelContract>
        where TViewModelContract : IViewModel<TViewModelContract, TViewContract>
    {
        private Dictionary<string, string> _ValidationMessages = new Dictionary<string, string>();
        private bool _ValidationOk;

        public virtual string DisplayName { get; protected set; }
        public virtual string Error { get; protected set; }
        public event EventHandler Validated;
        public event EventHandler<CancelEventArgs> Validating;
        public string this[string columnName]
        {
            get
            {
                if (_ValidationMessages != null && _ValidationMessages.ContainsKey(columnName))
                {
                    return _ValidationMessages[columnName];
                }

                return string.Empty;
            }
        }
        public TViewContract View { get; set; }
        #region Validation
        public Dictionary<string, string> ValidationMessages { get { return _ValidationMessages; } }
        public bool IsValidating { get; protected set; }
        /// <summary>
        /// Used to signal that no Errors were detected in validation - directly bind it to controls to enable them if needed.
        /// </summary>
        public bool ValidationOk
        {
            get
            {
                return _ValidationOk;
            }
            set
            {
                _ValidationOk = value;
                RaisePropertyChanged(() => this.ValidationOk);
            }
        }

        public void Validate()
        {
            var cancelArgs = new CancelEventArgs(false);
            RaiseValidating(this, cancelArgs);

            if (!cancelArgs.Cancel && !IsValidating)
            {
                IsValidating = true;
                InternalValidate();
                RaiseValidated(this, EventArgs.Empty);
                IsValidating = false;
            }
        }

        #endregion

        #region Internal Validation Implementation

        private void InternalValidate()
        {
            ValidationMessages.Clear();
            ExecuteValidation();
            Error = string.Join(Environment.NewLine, ValidationMessages.Select(m => m.Value));
            ValidationOk = ValidationMessages.Count == 0;
        }

        /// <summary>
        /// Needs to be overwritten to perform the actual validation
        /// </summary>
        /// <returns>Validation Errors</returns>
        protected virtual void ExecuteValidation()
        {
        }
        #endregion

        #region Validation Helpers
        protected void AddMemberError(string memberName, string errorMessage)
        {
            if (!ValidationMessages.ContainsKey(memberName))
                ValidationMessages.Add(memberName, errorMessage);
            else
                ValidationMessages[memberName] += Environment.NewLine + errorMessage;
        }
        protected void AddMemberError<T>(Expression<Func<T>> member, string errorMessage)
        {
            var expression = (MemberExpression)member.Body;
            var memberName = expression.Member.Name;

            AddMemberError(memberName, errorMessage);
        }
        protected void AddMemberError<T>(Expression<Func<T>> member, Func<bool> checkFunc, string errorMessage)
        {
            if (!checkFunc())
                AddMemberError(member, errorMessage);
        }
        #endregion

        public void Dispose()
        {
            this.OnDispose();
        }

        /// <summary>
        /// Child classes can override this method to perform 
        /// clean-up logic, such as removing event handlers.
        /// </summary>
        protected virtual void OnDispose()
        {
        }

        protected virtual void RaiseValidated(object sender, EventArgs e)
        {
            EventHandler handler = Validated;
            if (handler != null)
                handler(sender, e);
        }

        protected virtual void RaiseValidating(object sender, CancelEventArgs e)
        {
            EventHandler<CancelEventArgs> handler = Validating;
            if (handler != null)
                handler(sender, e);
        }
    }
}