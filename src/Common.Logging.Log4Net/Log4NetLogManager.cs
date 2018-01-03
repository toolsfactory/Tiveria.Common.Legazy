using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiveria.Common.Logging.Log4Net
{
    public class Log4NetLogManager : ILogManager
    {
        public ILogger GetLogger(string name)
        {
            return new Log4NetLoggerAdapter(log4net.LogManager.GetLogger(name));
        }

        public ILogger GetLogger(Type type)
        {
            return new Log4NetLoggerAdapter(log4net.LogManager.GetLogger(type));
        }
    }
}
