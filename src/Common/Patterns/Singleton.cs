
using System;
using System.Reflection;

namespace Tiveria.Common.Patterns
{
    /// <summary>
    /// Manages the single instance of a class.
    /// </summary>
    /// <typeparam name="T">Type of the singleton class.</typeparam>
    public static class Singleton<T>
        where T : class
    {
        #region Fields

        /// <summary>
        /// The single instance of the target class.
        /// </summary>
        static volatile T _Instance;

        /// <summary>
        /// The dummy object used for locking.
        /// </summary>
        static object _Lock = new object();

        #endregion Fields


        #region Constructors

        /// <summary>
        /// Type-initializer to prevent type to be marked with beforefieldinit.
        /// </summary>
        /// <remarks>
        /// This simply makes sure that static fields initialization occurs 
        /// when Instance is called the first time and not before.
        /// </remarks>
        static Singleton()
        {
        }

        #endregion Constructors


        #region Properties

        /// <summary>
        /// Gets the single instance of the class.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_Lock)
                    {
                        if (_Instance == null)
                        {
                            ConstructorInfo constructor = null;

                            try
                            {
                                constructor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[0], null);
                            }
                            catch (Exception exception)
                            {
                                throw new SingletonException(exception);
                            }

                            if (constructor == null || constructor.IsAssembly)
                                throw new SingletonException(string.Format("No private or protected constructor found in '{0}'.", typeof(T).Name));

                            _Instance = (T)constructor.Invoke(null);
                        }
                    }
                }
                return _Instance;
            }
        }

        #endregion Properties
    }
}
