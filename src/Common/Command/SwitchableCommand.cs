using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Tiveria.Common.Command
{
    public class SwitchableCommand : ICommand
    {
        private bool _Enabled;
        internal readonly Action<object> _ExecuteAction;

        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public SwitchableCommand(Action<object> execute)
        {
            _ExecuteAction = execute;
            _Enabled = true;
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
                OnCanExecuteChanged();
            }
        }


        #region ICommand interface
        public bool CanExecute(object parameter)
        {
            return _Enabled;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (_Enabled)
                _ExecuteAction(parameter);
        }
        #endregion

        private void OnCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

    }
}
