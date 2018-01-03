using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Tiveria.Common.Command
{
    public interface ICommandBinder
    {
        Type SourceType { get; }
        void Bind(ICommand command, object source);
        void UpdateCommandStates(Dictionary<object, ICommand> bindings);
    }
}
