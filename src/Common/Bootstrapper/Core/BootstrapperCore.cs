using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Tiveria.Common.Bootstrapper.Core
{
    public class BootstrapperCore : IBootstrapperConfiguration, IBootstrapperCore
    {
        private readonly IBootstrapperAssembliesStore _AssembliesStore = new BootstrapperAssembliesStore();
        private readonly IBootstrapperPluginsStore _PluginsStore = new BootstrapperPluginsStore();
        private BootstrapperContext _Context;

        public BootstrapperCore()
        {
            Started = false;
            Stopped = false;
        }

        public bool Started { get; private set; }
        public bool Stopped { get; private set; }
        public Plugins.IBootstrapperContainerPlugin Container { get { return _PluginsStore.Container; } }
        public IReadOnlyList<Assembly> Assemblies { get { return _AssembliesStore.Assemblies; } }



        public void Startup()
        {
            Startup(new Dictionary<string,object>());
        }

        public void Startup(IDictionary<string, object> context)
        {
            if (Stopped)
                throw new InvalidOperationException("Cannot call Startup after Bootstrapper Shutdown was called");

            _Context = new BootstrapperContext() { Bag = new Dictionary<string, object>(context), AssembliesConfiguration = this };
            _PluginsStore.InitializePlugins(_Context);
            _AssembliesStore.InitializeAssembliesList();
            Started = true;
            _PluginsStore.StartupPlugins(_Context);
        }

        public void Shutdown()
        {
            if (!Started || Stopped)
                return;

            _PluginsStore.ShutDownPlugins(_Context);
            Stopped = true;
        }

        #region Fluent Configuration Interface (IBootstrapperConfiguration)

        public IBootstrapperConfiguration AddPlugin(Plugins.IBootstrapperPlugin plugin)
        {
            if (Started)
                throw new InvalidOperationException("Cannot add Plugins after Bootstrapper Startup was called");

            _PluginsStore.AddPlugin(plugin);
            return this;
        }

        public IBootstrapperConfiguration IncludeAssembly(Assembly assembly)
        {
            if (Started)
                throw new InvalidOperationException("Cannot include assemblies after Bootstrapper Startup was called");

            _AssembliesStore.IncludeAssembly(assembly);
            return this;
        }

        public IBootstrapperConfiguration ExcludeAssembly(string assemblyName)
        {
            if (Started)
                throw new InvalidOperationException("Cannot exclude assemblies after Bootstrapper Startup was called");

            _AssembliesStore.ExcludeAssembly(assemblyName);
            return this;
        }

        public IBootstrapperConfiguration WithAssemblyProvider(IBootstrapperAssemblyProvider provider)
        {
            if (Started)
                throw new InvalidOperationException("Cannot change Assembly provider after Bootstrapper Startup was called");

            _AssembliesStore.SetAssembliesProvider(provider);
            return this;
        }
        #endregion
    }
}