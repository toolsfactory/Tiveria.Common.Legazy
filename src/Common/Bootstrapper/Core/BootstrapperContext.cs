using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tiveria.Common.Bootstrapper.Core
{
    public class BootstrapperContext : IBootstrapperContext
    {
        public IDictionary<string, object> Bag
        {
            get;
            internal set;
        }

        public IBootstrapperAssembliesConfiguration AssembliesConfiguration
        {
            get;
            internal set;
        }
    }
}