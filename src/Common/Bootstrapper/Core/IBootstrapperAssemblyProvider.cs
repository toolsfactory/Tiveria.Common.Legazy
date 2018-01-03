using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Tiveria.Common.Bootstrapper.Core
{
    public interface IBootstrapperAssemblyProvider
    {
        /// <summary>
        /// Provides the initial list of Assemblies. 
        /// This list is then modified using the IncludeAssembly and ExcludeAssembly configurations.
        /// </summary>
        /// <returns>The base list of Assemblies to use</returns>
        IEnumerable<Assembly> GetAssemblies();

        /// <summary>
        /// Is called at the end of the overall assemblies list generation and allows a final sanitization.
        /// <remarks>Sanitizing the list should be done very carefully as other plugins are not aware of this and might run into trouble</remarks>
        /// </summary>
        /// <param name="list">The list to sanitize</param>
        /// <returns>The final list</returns>
        IEnumerable<Assembly> SanitizeAssemblies(IEnumerable<Assembly> list);
    }
}
