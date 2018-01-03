using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiveria.Common.Bootstrapper
{
    public static class BootstrapperAutoMapperPluginExtensions
    {
        public static IBootstrapperConfiguration WithAutoMapper(this IBootstrapperConfiguration config)
        {
            return config.AddPlugin(new BootstrapperAutoMapperPlugin());
        }
    }
}
