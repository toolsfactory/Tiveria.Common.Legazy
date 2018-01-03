using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Tiveria.Common.Bootstrapper.Core
{
    public class BootstrapperAssembliesStore : IBootstrapperAssembliesStore
        {
            private readonly List<string> _ExcludeAssemblies = new List<string>();
            private readonly List<Assembly> _IncludeAssemblies = new List<Assembly>();
            private IBootstrapperAssemblyProvider _AssemblyProvider = new LoadedAssembliesProvider();

            public BootstrapperAssembliesStore()
            {
                _ExcludeAssemblies.Add("System");
                _ExcludeAssemblies.Add("Microsoft");
                _ExcludeAssemblies.Add("mscorlib");
                _ExcludeAssemblies.Add("Tiveria.Common");
                _ExcludeAssemblies.Add("Owin");
                _ExcludeAssemblies.Add("EntityFramework");
                _IncludeAssemblies.Add(Assembly.GetEntryAssembly());
                Assemblies = new List<Assembly>().AsReadOnly();
            }

            public void IncludeAssembly(Assembly assembly)
            {
                if (assembly == null)
                    return;

                if (!_IncludeAssemblies.Contains(assembly))
                    _IncludeAssemblies.Add(assembly);
            }

            public void ExcludeAssembly(string assemblyname)
            {
                if (string.IsNullOrWhiteSpace(assemblyname))
                    return;

                if (!_ExcludeAssemblies.Contains(assemblyname))
                    _ExcludeAssemblies.Add(assemblyname);
            }

            public IReadOnlyList<Assembly> Assemblies { get; private set; }

            public void InitializeAssembliesList()
            {
                var list = new List<Assembly>();
                list.AddRange(_AssemblyProvider.GetAssemblies());
                list = ApplyIncludeToAssembliesList(list);
                list = ApplyExcludeToAssembliesList(list);

                var finallist = new List<Assembly>(_AssemblyProvider.SanitizeAssemblies(list));
                Assemblies = finallist.AsReadOnly();
            }

            public void SetAssembliesProvider(IBootstrapperAssemblyProvider provider)
            {
                if (provider == null)
                    throw new ArgumentNullException("AssemblyProvider cannot be null");

                _AssemblyProvider = provider;
            }

            private List<Assembly> ApplyIncludeToAssembliesList(List<Assembly> list)
            {
                foreach (var item in _IncludeAssemblies)
                    if (!list.Contains(item))
                        list.Add(item);

                return list;
            }

            private List<Assembly> ApplyExcludeToAssembliesList(List<Assembly> list)
            {
                return list.Where(a => a != null && !a.IsDynamic && !IsExcluded(a)).ToList();
            }

            private bool IsExcluded(Assembly assembly)
            {
                var exclude = _ExcludeAssemblies.Any(e => assembly.FullName.StartsWith(e, true, null));
                return exclude;
            }

        }
}
