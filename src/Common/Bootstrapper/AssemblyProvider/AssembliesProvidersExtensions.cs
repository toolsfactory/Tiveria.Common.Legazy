using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Tiveria.Common.Bootstrapper
{
    public static class AssembliesProvidersExtensions
    {
        public static IBootstrapperConfiguration WithLoadedAssemblies(this IBootstrapperConfiguration config)
        {
            config.WithAssemblyProvider(new LoadedAssembliesProvider());
            return config;
        }

        public static IBootstrapperConfiguration WithWebReferencedAssemblies(this IBootstrapperConfiguration config)
        {
            config.WithAssemblyProvider(new WebReferencedAssemblyProvider());
            return config;
        }

        public static IBootstrapperConfiguration WithReferencedAssemblies(this IBootstrapperConfiguration config)
        {
            config.WithAssemblyProvider(new ReferencedAssemblyProvider(Assembly.GetEntryAssembly()));
            return config;
        }

        public static IBootstrapperConfiguration WithReferencedAssemblies(this IBootstrapperConfiguration config, Assembly rootAssembly)
        {
            config.WithAssemblyProvider(new ReferencedAssemblyProvider(rootAssembly));
            return config;
        }

    }
}
