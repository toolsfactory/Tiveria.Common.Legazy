using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Tiveria.Common.Command
{

    [ToolboxItem(false)]
    public class CommandManager : Component, ICommandManager
    {
        #region static part
        private static IList<ICommandBinder> _Binders  = new List<ICommandBinder>();

        static CommandManager()
        {
            RegisterCommandBinder(new ControlBinder());
            RegisterCommandBinder(new MenuItemCommandBinder());
        }

        /// <summary>
        /// Registers a new Commandbinder if the sourcetype of the provided commandBinder is not yet registered.
        /// </summary>
        /// <param name="commandBinder">The new commandBinder</param>
        public static void RegisterCommandBinder(ICommandBinder commandBinder)
        {
            if (commandBinder == null)
                throw new ArgumentNullException("Empty commandBinder provided");

            if (GetBinderFor(commandBinder.SourceType) != null)
                return;

            _Binders.Add(commandBinder);
        }

        private static ICommandBinder FindBinder(IComponent component)
        {
            var binder = GetBinderFor(component.GetType());

            if (binder == null)
                throw new ArgumentException(string.Format("No binding found for component of type {0}", component.GetType().Name));

            return binder;
        }

        /// <summary>
        /// Find the correct Binder used for a certain component by checking the types it it based on
        /// </summary>
        /// <param name="component">Component a binder is required</param>
        /// <returns>The binder</returns>
        private static ICommandBinder GetBinderFor(Type type)
        {
            while (type != null)
            {
                var binder = _Binders.FirstOrDefault(x => x.SourceType == type);
                if (binder != null)
                    return binder;
                type = type.BaseType;
            }
            return null;
        }
        #endregion


        private IList<ICommand> _Commands = new List<ICommand>();
        private Dictionary<object, ICommand> _Bindings = new Dictionary<object, ICommand>();

        public void UpdateCommandsStates()
        {
            foreach (var binder in _Binders)
                binder.UpdateCommandStates(_Bindings);
        }


        /// <summary>
        /// Binds a command to a component
        /// </summary>
        /// <param name="command">the command to bind</param>
        /// <param name="component">the component to bind</param>
        /// <returns>CommandBinder object to chain bindings</returns>
        public CommandManager Bind(ICommand command, IComponent component)
        {
            if (command == null)
                throw new ArgumentNullException("Empty argument provided");
            if (component == null)
                throw new ArgumentNullException("Empty component provided");
            if (_Bindings.ContainsKey(component))
                throw new ArgumentException("component already bound");

            var binder = FindBinder(component);

            if (!_Commands.Contains(command))
                _Commands.Add(command);

            binder.Bind(command, component);
            _Bindings.Add(component, command);

            return this;
        }

        public bool CanBind(Type componentType)
        {
            return GetBinderFor(componentType) != null;
        }
    }
}