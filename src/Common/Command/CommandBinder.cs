using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Tiveria.Common.Command
{
    public abstract class CommandBinder<T> : ICommandBinder
    {
        public Type SourceType
        {
            get { return typeof(T); }
        }

        public void Bind(ICommand command, object source)
        {
            if (!(source is T))
                throw new ArgumentException("Invalid component type for this binder");

            Bind(command, (T)source);
        }

        public abstract void UpdateCommandStates(Dictionary<object, ICommand> bindings);

        protected abstract void Bind(ICommand command, T source);

    }
}
