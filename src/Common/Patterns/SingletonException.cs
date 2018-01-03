using System;

namespace Tiveria.Common.Patterns
{
    /// <summary>
    /// Represents any kind of errors that occur while creating a singleton of a class.
    /// </summary>
    [Serializable]
    public class SingletonException : InvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public SingletonException()
        {
        }

        /// <summary>
        /// Initializes a new instance with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SingletonException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance with a reference to the inner 
        /// exception that is the cause of this exception.
        /// </summary>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, 
        /// or a null reference if no inner exception is specified.
        /// </param>
        public SingletonException(Exception innerException)
            : base(null, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance with a specified error message and a 
        /// reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, 
        /// or a null reference if no inner exception is specified.
        /// </param>
        public SingletonException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
