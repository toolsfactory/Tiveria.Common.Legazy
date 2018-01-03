using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Tiveria.Common.Bootstrapper.Core
{
    public interface IBootstrapperCore
    {
        bool Started { get; }
        bool Stopped { get; }
        Plugins.IBootstrapperContainerPlugin Container { get; }
        void Shutdown();
    }
}
