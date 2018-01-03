using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiveria.Common.Bootstrapper
{
    public interface IBootstrapperStartupTask
    {
        void OnStartup(IDictionary<string, object> context);
    }
}
