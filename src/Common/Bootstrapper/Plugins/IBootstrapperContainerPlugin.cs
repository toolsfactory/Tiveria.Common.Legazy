using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiveria.Common.Bootstrapper.Plugins
{
    /// <summary>
    /// Interface ro be implemented by a Dependency Injection container
    /// <remarks>The Plugin should initialize the container inside the OnStartup method so that afterwards all Resolve methods provide valid results.
    /// A DI Plugin is always the first plugion to be initialized and started and the last one to be shut down</remarks>
    /// </summary>
    public interface IBootstrapperContainerPlugin : IBootstrapperPlugin, IBootstrapperContainer
    { }
}