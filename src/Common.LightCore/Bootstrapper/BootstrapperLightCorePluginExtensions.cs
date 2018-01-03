using System;
using System.Collections.Generic;
using System.Linq;
using Tiveria.Common.Bootstrapper;
using LightCore;

namespace Tiveria.Common.Bootstrapper
{
    public static class BootstrapperLightCorePluginExtensions
    {
        public static IBootstrapperConfiguration WithLightCore(this IBootstrapperConfiguration bootstrapper)
        {
            return bootstrapper.AddPlugin(new BootstrapperLightCorePlugin(new ContainerBuilder()));
        }

        public static IBootstrapperConfiguration WithLightCore(this IBootstrapperConfiguration bootstrapper, IContainerBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException();

            return bootstrapper.AddPlugin(new BootstrapperLightCorePlugin(builder));
        }

    }
}
