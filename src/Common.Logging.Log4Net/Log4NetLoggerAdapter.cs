using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiveria.Common.Logging.Log4Net
{
     public class Log4NetLoggerAdapter : ILogger
    {
        private log4net.ILog _Logger;

        public Log4NetLoggerAdapter(log4net.ILog log4netlogger)
        {
            _Logger = log4netlogger;
        }

        public void Debug(object message)
        {
            _Logger.Debug(message);
        }

        public void Debug(object message, Exception exception)
        {
            _Logger.Debug(message,exception);
        }

        public void Info(object message)
        {
            _Logger.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            _Logger.Info(message, exception);
        }

        public void Warn(object message)
        {
            _Logger.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            _Logger.Warn(message, exception);
        }

        public void Error(object message)
        {
            _Logger.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            _Logger.Error(message, exception);
        }

        public void Fatal(object message)
        {
            _Logger.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            _Logger.Fatal(message, exception);
        }

        public bool IsDebugEnabled
        {
            get { return _Logger.IsDebugEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return _Logger.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return _Logger.IsWarnEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return _Logger.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return _Logger.IsFatalEnabled; }
        }
    }
}
