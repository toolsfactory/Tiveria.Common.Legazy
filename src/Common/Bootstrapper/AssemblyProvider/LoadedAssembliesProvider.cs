using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tiveria.Common.Bootstrapper.Core;

namespace Tiveria.Common.Bootstrapper
{
    public class LoadedAssembliesProvider : IBootstrapperAssemblyProvider
    {
        public IEnumerable<Assembly> GetAssemblies() { return AppDomain.CurrentDomain.GetAssemblies(); }

        public IEnumerable<Assembly> SanitizeAssemblies(IEnumerable<Assembly> list)
        {
            return list;
        }
    }

}