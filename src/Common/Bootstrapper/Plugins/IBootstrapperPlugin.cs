using System;
using System.Collections.Generic;
using System.Linq;
using Tiveria.Common.Bootstrapper.Core;

namespace Tiveria.Common.Bootstrapper.Plugins
{
    public interface IBootstrapperPlugin
    {
        /// <summary>
        /// Method used to initialize a plugion. Within this method, a plugin can exclude or incluide assemblies using the <see cref="IBootstrapperContext.AssembliesConfiguration"/> object.
        /// <remarks>A DI Container might not be ready in this step of the boot process</remarks>
        /// </summary>
        /// <param name="context"></param>
        void Initialize(IBootstrapperContext context);
        /// <summary>
        /// The method called after all plugins are initialized and the DI COntainer is ready.
        /// <remarks>Modifying the list of assemblies is not possible any more and the container is already set up.</remarks>
        /// </summary>
        /// <param name="context"></param>
        void Startup(IBootstrapperContext context);
        void Shutdown(IBootstrapperContext context);
    }

}
