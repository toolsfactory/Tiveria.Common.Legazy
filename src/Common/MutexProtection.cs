using System;
using System.Threading;

namespace Tiveria.Common
{
		/// <summary>
		/// Helper class for serializing access to a using section
		/// </summary>
		sealed class MutexProtection : IDisposable
		{
			readonly Mutex mutex;

            public MutexProtection(string name)
			{
				bool createdNew;
				this.mutex = new Mutex(true, name, out createdNew);
				if (!createdNew) 
                {
					try 
                    {
						mutex.WaitOne();
					} 
                    catch (AbandonedMutexException) 
                    {
					}
				}
			}
			
			public void Dispose()
			{
				mutex.ReleaseMutex();
				mutex.Dispose();
			}
		}}
