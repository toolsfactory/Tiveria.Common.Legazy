using Tiveria.Common.Command;
using System.Windows.Input;

namespace Tiveria.Common.Controls
{
    public class DevExButtonBinder : CommandBinder<DevExpress.XtraBars.BarButtonItem>
    {
        static DevExButtonBinder()
        {
            CommandManager.RegisterCommandBinder(new DevExButtonBinder());
        }

        protected override void Bind(ICommand command, DevExpress.XtraBars.BarButtonItem source)
        {
            source.ItemClick += (o, e) => command.Execute(null);
            command.CanExecuteChanged += (o, e) => source.Enabled = command.CanExecute(null);
        }

        public override void UpdateCommandStates(System.Collections.Generic.Dictionary<object, ICommand> bindings)
        {
            foreach (var pair in bindings)
            {
                if(pair.Key is DevExpress.XtraBars.BarButtonItem)
                    ((DevExpress.XtraBars.BarButtonItem)pair.Key).Enabled = pair.Value.CanExecute(null);
            }
        }
    }
}
