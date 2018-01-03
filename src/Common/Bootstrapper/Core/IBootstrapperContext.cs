using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiveria.Common.Bootstrapper.Core
{
    public interface IBootstrapperContext
    {
        IDictionary<string, object> Bag { get; }
        IBootstrapperAssembliesConfiguration AssembliesConfiguration { get; }
    }

}
