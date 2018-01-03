using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiveria.Common.Data
{
    public interface IRepositoryResolver
    {
        T Resolve<T>() where T : class, IRepository;
    }
}
