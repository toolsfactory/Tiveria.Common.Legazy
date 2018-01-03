using System;

namespace Tiveria.Common.Caching
{
    public interface ICachingProvider : IDisposable
    {
        long Count { get; }
        void Set<T>(string key, T value, int? timeoutInSeconds = null);
        void Remove(string key);
        bool Exists(string key);
        bool Get<T>(string key, out T value);

/*
        void SetAndUnlock<T>(string key, T value, object lockhandle, int? timeoutInSeconds = null);
        bool GetAndLock<T>(string key, int locktimeInSeconds, out T value, out object lockhandle);
        void Unlock(string key, object lockhandle); 
 */ 

        int Increment(string key, int defaultValue, int incrementValue); 
    }
}
