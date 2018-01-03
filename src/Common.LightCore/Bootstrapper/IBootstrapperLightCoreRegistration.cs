using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightCore;

namespace Tiveria.Common.Bootstrapper
{
    public interface IBootstrapperLightCoreRegistration
    {
        void Register(IContainerBuilder builder, IDictionary<string, object> context);
    }
}
