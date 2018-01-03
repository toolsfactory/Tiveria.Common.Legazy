using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace Tiveria.Common.Data
{
    public static class RepositoryResolver
    {
        private static IRepositoryResolver _Resolver;
        public static void SetRepositoryResolver(IRepositoryResolver resolver)
        {
            _Resolver = resolver;
        }

        internal static T ResolveRepository<T>() where T : class, IRepository
        {
            T repo = _Resolver.Resolve<T>();
            return (T)repo;
        }
    }
}
