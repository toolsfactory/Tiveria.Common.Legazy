using System.Windows.Forms;
using System.Windows.Input;
using System.Collections.Generic;

namespace Tiveria.Common.Command
{
    public class ControlBinder : CommandBinder<Control>
    {

        protected override void Bind(ICommand command, Control source)
        {
            source.Click += (o, e) => command.Execute(null);
            command.CanExecuteChanged += (o, e) => source.Enabled = command.CanExecute(null);
        }

        public override void UpdateCommandStates(Dictionary<object, ICommand> bindings)
        {
            foreach (var pair in bindings)
            {
                if (pair.Key is Control)
                    ((Control)pair.Key).Enabled = pair.Value.CanExecute(null);
            }
        }
    }
}
