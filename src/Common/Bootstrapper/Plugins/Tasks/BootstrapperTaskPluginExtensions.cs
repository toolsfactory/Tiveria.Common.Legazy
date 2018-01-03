using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiveria.Common.Bootstrapper
{
    public static class BootstrapperTaskPluginExtensions
    {
        public static IBootstrapperConfiguration WithTasks(this IBootstrapperConfiguration bootstrapper)
        {
            var result = bootstrapper.AddPlugin(new Plugins.BootstrapperStartupTaskPlugin());
            return result.AddPlugin(new Plugins.BootstrapperShutdownTaskPlugin());
        }

        public static IBootstrapperConfiguration WithStartupTasks(this IBootstrapperConfiguration bootstrapper)
        {
            return bootstrapper.AddPlugin(new Plugins.BootstrapperStartupTaskPlugin());
        }

        public static IBootstrapperConfiguration WithShutdownTasks(this IBootstrapperConfiguration bootstrapper)
        {
            return bootstrapper.AddPlugin(new Plugins.BootstrapperShutdownTaskPlugin());
        }
    }
}
