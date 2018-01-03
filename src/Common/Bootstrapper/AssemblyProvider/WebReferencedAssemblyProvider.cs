using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;

namespace Tiveria.Common.Bootstrapper
{
    public class WebReferencedAssemblyProvider : Core.IBootstrapperAssemblyProvider
    {
        public IEnumerable<Assembly> GetAssemblies()
        {
            return BuildManager.GetReferencedAssemblies().Cast<Assembly>();
        }

        public IEnumerable<Assembly> SanitizeAssemblies(IEnumerable<Assembly> list)
        {
            return list;
        }
    }

}
