using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiveria.Common.Logging
{
    public interface ILogManager
    {
        ILogger GetLogger(string name);
        ILogger GetLogger(Type type);  
    }
}
