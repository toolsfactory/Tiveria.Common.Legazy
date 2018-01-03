using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Tiveria.Common.Bootstrapper
{
    public interface IBootstrapperAssembliesConfiguration
    {
        IBootstrapperConfiguration IncludeAssembly(Assembly assembly);
        IBootstrapperConfiguration ExcludeAssembly(string assemblyname);
        IReadOnlyList<System.Reflection.Assembly> Assemblies {get;} 
    }
}
