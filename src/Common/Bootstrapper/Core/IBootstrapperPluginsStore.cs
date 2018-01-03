using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Tiveria.Common.Bootstrapper.Core
{
    public interface IBootstrapperPluginsStore
    {
        Plugins.IBootstrapperContainerPlugin Container { get; }
        void InitializePlugins(IBootstrapperContext context);
        void StartupPlugins(IBootstrapperContext context);
        void ShutDownPlugins(IBootstrapperContext context);
        void AddPlugin(Plugins.IBootstrapperPlugin plugin);
    }
}
