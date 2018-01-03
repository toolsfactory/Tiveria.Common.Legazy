using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace Tiveria.Common.Extensions
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetTypesImplementing<T>(this Assembly assembly)
        {
            return assembly.GetExportedTypes().Where(t => t.IsPublic &&
                                                          !t.IsAbstract &&
                                                          typeof(T).IsAssignableFrom(t));
        }
    }
}
