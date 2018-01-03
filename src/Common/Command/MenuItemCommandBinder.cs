using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
namespace Tiveria.Common.Command
{
    public class MenuItemCommandBinder : CommandBinder<ToolStripItem>
    {
        protected override void Bind(ICommand command, ToolStripItem source)
        {
            source.Enabled = command.CanExecute(null);
            source.Click += (o, e) => command.Execute(e);

            command.CanExecuteChanged += (o, e) => source.Enabled = command.CanExecute(null);
        }

        public override void UpdateCommandStates(Dictionary<object, ICommand> bindings)
        {
            foreach (var pair in bindings)
            {
                if (pair.Key is ToolStripItem)
                ((ToolStripItem)pair.Key).Enabled = pair.Value.CanExecute(null);
            }
        }

    }
}
