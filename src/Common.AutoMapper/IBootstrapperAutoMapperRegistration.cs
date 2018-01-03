using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiveria.Common.Bootstrapper
{
    public interface IBootstrapperAutoMapperRegistration
    {
        void RegisterMappings(AutoMapper.IProfileExpression mapper, IDictionary<string, object> context);
    }

}
