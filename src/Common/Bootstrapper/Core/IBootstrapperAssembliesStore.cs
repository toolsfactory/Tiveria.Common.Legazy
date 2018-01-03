using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Tiveria.Common.Bootstrapper.Core
{
    public interface IBootstrapperAssembliesStore
    {
        void IncludeAssembly(Assembly assembly);
        void ExcludeAssembly(string assemblyname);
        IReadOnlyList<Assembly> Assemblies { get; }
        void InitializeAssembliesList();
        void SetAssembliesProvider(IBootstrapperAssemblyProvider provider);
    }
}
