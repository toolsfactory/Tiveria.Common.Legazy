using System;
using System.Collections.Generic;
using System.Linq;
using Tiveria.Common.Bootstrapper.Core;

namespace Tiveria.Common.Bootstrapper
{
    public interface IBootstrapperConfiguration : IBootstrapperAssembliesConfiguration
    {
        IBootstrapperConfiguration AddPlugin(Plugins.IBootstrapperPlugin plugin);
        IBootstrapperConfiguration WithAssemblyProvider(IBootstrapperAssemblyProvider provider);
        void Startup();
        void Startup(IDictionary<string, object> context);
    }
}