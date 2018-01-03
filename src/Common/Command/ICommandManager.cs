using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Tiveria.Common.Command
{
    public interface ICommandManager
    {
        CommandManager Bind(ICommand command, IComponent component);
        bool CanBind(Type componentType);
        void UpdateCommandsStates();
    }
}
