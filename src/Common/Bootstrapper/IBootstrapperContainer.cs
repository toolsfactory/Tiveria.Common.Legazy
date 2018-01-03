using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiveria.Common.Bootstrapper
{
    public interface IBootstrapperContainer
    {
        bool HasRegistration<TContract>();
        bool HasRegistration(Type contractType);
        TContract Resolve<TContract>();
        object Resolve(Type contractType);
        IEnumerable<TContract> ResolveAll<TContract>();
        IEnumerable<object> ResolveAll(Type contractType);
        IEnumerable<object> ResolveAll();
    }
}
