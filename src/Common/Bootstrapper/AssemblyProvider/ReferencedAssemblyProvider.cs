using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;

namespace Tiveria.Common.Bootstrapper
{
    public class ReferencedAssemblyProvider : Core.IBootstrapperAssemblyProvider
    {
        private readonly Assembly _RootAssembly;

        public ReferencedAssemblyProvider(Assembly rootAssembly)
        {
            _RootAssembly = rootAssembly;
        }
        public IEnumerable<Assembly> GetAssemblies()
        {
            var assemblyNames = _RootAssembly.GetReferencedAssemblies();
            var assemblies = new List<Assembly>();
            foreach (var assembly in assemblyNames)
                assemblies.Add(Assembly.Load(assembly.FullName));

            return assemblies;
        }

        public IEnumerable<Assembly> SanitizeAssemblies(IEnumerable<Assembly> list)
        {
            return list;
        }
    }
}
