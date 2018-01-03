using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tiveria.Common.Extensions;

namespace Tiveria.Common.Bootstrapper.Core
{
    public static class BootstrapperContextExtensions
    {
        public static IEnumerable<Type> GetTypesImplementing<T>(this IBootstrapperContext context)
        {
            var list = new List<Type>();
            context.AssembliesConfiguration.Assemblies.ToList()
                .ForEach(a => list.AddRange(a.GetTypesImplementing<T>()));
            return list;
        }

        public static List<T> GetInstancesOfTypesImplementing<T>(this IBootstrapperContext context)
        {
            var instances = new List<T>();
            foreach(var assembly in context.AssembliesConfiguration.Assemblies)
                assembly.GetTypesImplementing<T>().ToList()
                    .ForEach(t => instances.Add((T)Activator.CreateInstance(t)));
            return instances;
        }
    }
}