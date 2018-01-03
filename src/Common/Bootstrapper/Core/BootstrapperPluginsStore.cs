using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Tiveria.Common.Bootstrapper.Core
{
    public class BootstrapperPluginsStore : IBootstrapperPluginsStore
    {
        private readonly List<Plugins.IBootstrapperPlugin> _Plugins = new List<Plugins.IBootstrapperPlugin>();

        public Plugins.IBootstrapperContainerPlugin Container { get; private set; }

        public void InitializePlugins(IBootstrapperContext context)
        {
            if (Container != null)
                Container.Initialize(context);

            foreach (var plugin in _Plugins)
                plugin.Initialize(context);
        }

        public void StartupPlugins(IBootstrapperContext context)
        {
            if (Container != null)
                Container.Startup(context);

            foreach (var plugin in _Plugins)
                plugin.Startup(context);
        }

        public void ShutDownPlugins(IBootstrapperContext context)
        {
            _Plugins.Reverse();
            foreach (var plugin in _Plugins)
                plugin.Shutdown(context);

            if (Container != null)
                Container.Shutdown(context);
        }

        public void AddPlugin(Plugins.IBootstrapperPlugin plugin)
        {
            if (plugin == null)
                return;

            if (plugin is Plugins.IBootstrapperContainerPlugin)
                Container = plugin as Plugins.IBootstrapperContainerPlugin;
            else
                _Plugins.Add(plugin);
        }

        public int Count { get { return _Plugins.Count + ((Container == null ) ? 0 : 1); } }
    }
}
