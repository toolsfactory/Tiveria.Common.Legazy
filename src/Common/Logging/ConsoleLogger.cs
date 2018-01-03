using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiveria.Common.Logging
{
    class ConsoleLogger : ILogger
    {
        public void Debug(object message)
        {
            Console.WriteLine("Debug: " + message);
        }

        public void Debug(object message, Exception exception)
        {
            Console.WriteLine("Debug: " + message);
            Console.WriteLine("Debug Exception: " + exception);
        }

        public void Info(object message)
        {
            Console.WriteLine("Info: " + message);
        }

        public void Info(object message, Exception exception)
        {
            Console.WriteLine("Info: " + message);
            Console.WriteLine("Info Exception: " + exception);
        }

        public void Warn(object message)
        {
            Console.WriteLine("Warn: " + message);
        }

        public void Warn(object message, Exception exception)
        {
            Console.WriteLine("Warn: " + message);
            Console.WriteLine("Warn Exception: " + exception);
        }

        public void Error(object message)
        {
            Console.WriteLine("Error: " + message);
        }

        public void Error(object message, Exception exception)
        {
            Console.WriteLine("Error: " + message);
            Console.WriteLine("Error Exception: " + exception);
        }

        public void Fatal(object message)
        {
            Console.WriteLine("Fatal: " + message);
        }

        public void Fatal(object message, Exception exception)
        {
            Console.WriteLine("Fatal: " + message);
            Console.WriteLine("Fatal Exception: " + exception);
        }

        public bool IsDebugEnabled
        {
            get { return true; }
        }

        public bool IsInfoEnabled
        {
            get { return true; }
        }

        public bool IsWarnEnabled
        {
            get { return true; }
        }

        public bool IsErrorEnabled
        {
            get { return true; }
        }

        public bool IsFatalEnabled
        {
            get { return true; }
        }
    }
}
