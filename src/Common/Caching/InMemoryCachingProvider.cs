using System;
using System.Linq;
using System.Runtime.Caching;

namespace Tiveria.Common.Caching
{
    public class InMemoryCachingProvider : ICachingProvider
    {
        private static ObjectCache Cache
        {
            get { return MemoryCache.Default; }
        }


        private static readonly object LockObject = new object();

        public long Count { get { return Cache.GetCount(); } }

        public void Set<T>(string key, T value, int? timeoutInSeconds = null)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");
            var cacheitem = new CacheItem() { LockedUntil = DateTime.MinValue, LockHandle = String.Empty, Value = value };
            SetCacheItem(key, cacheitem, timeoutInSeconds);
        }


        public void Remove(string key)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");
            Cache.Remove(key);
        }


        public bool Exists(string key)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");
            return Cache.Any(x => x.Key == key);
        }


        public bool Get<T>(string key, out T value)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");
            value = default(T);

            try
            {
                if (!Exists(key))
                    return false;

                value = (T) ((CacheItem)Cache[key]).Value;
            }
            catch (Exception)
            {
                // ignore and use default
                return false;
            }
            return true;
        }


        public int Increment(string key, int defaultValue, int incrementValue)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

            lock (LockObject)
            {
                int current;
                if (!Get(key, out current))
                {
                    current = defaultValue;
                }

                var newValue = current + incrementValue;
                Set(key, newValue);
                return newValue;
            }
        }


        public void SetAndUnlock<T>(string key, T value, object lockhandle, int? timeoutInSeconds = null)
        {
            var handle = (string)lockhandle;
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");
            if (String.IsNullOrEmpty(handle)) throw new ArgumentException("Invalid Handle");

            lock (LockObject)
            {
                var cacheitem = (CacheItem)Cache[key];
                if (handle != cacheitem.LockHandle)
                {
                    throw new InvalidOperationException("Invalid LockHandle");
                }

                if (cacheitem.LockedUntil < DateTime.Now)
                {
                    throw new InvalidOperationException("Invalid LockHandle");
                }

                cacheitem.LockedUntil = DateTime.MinValue;
                cacheitem.Value = value;
                SetCacheItem(key, cacheitem, timeoutInSeconds);
            }
        }

        public bool GetAndLock<T>(string key, int locktimeInSeconds, out T value, out object lockhandle)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");
            value = default(T);

            lock (LockObject)
            {
                if(!Exists(key))
                {
                    var cacheitem = new CacheItem() { LockedUntil = DateTime.Now.AddSeconds(locktimeInSeconds), LockHandle = Guid.NewGuid().ToString("N"), Value = value };
                    SetCacheItem(key, cacheitem);
                    lockhandle = cacheitem.LockHandle;
                    return true;
                }
                else
                {
                    var cacheitem = (CacheItem)Cache[key];
                    if (cacheitem.LockedUntil > DateTime.Now)
                    {
                        lockhandle = null;
                        return false;
                    }

                    cacheitem.LockedUntil = DateTime.Now.AddSeconds(locktimeInSeconds);
                    cacheitem.LockHandle = Guid.NewGuid().ToString("N");
                    SetCacheItem(key, cacheitem);
                    lockhandle = cacheitem.LockHandle;
                    value = (T)cacheitem.Value;
                    return true;
                }
            }
        }

        public void Unlock(string key, object lockhandle)
        {
            var handle = (string) lockhandle;
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");
            if (String.IsNullOrEmpty(handle)) throw new ArgumentException("Invalid Handle");

            lock (LockObject)
            {
                if (!Exists(key))
                    return;

                var cacheitem = (CacheItem)Cache[key];
                if (cacheitem.LockedUntil > DateTime.Now)
                {
                    if (handle == cacheitem.LockHandle)
                    {
                        cacheitem.LockedUntil = DateTime.MinValue;
                        SetCacheItem(key, cacheitem);
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid LockHandle");
                    }
                }
            }
        }

        public void Dispose()
        {
            // no need to do anything
        }

        private static void SetCacheItem(string key, CacheItem value, int? cacheTime = null)
        {
            var policy = new CacheItemPolicy
                                         {
                                             Priority = CacheItemPriority.Default
                                         };
            if (cacheTime.HasValue)
            {
                policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromSeconds(cacheTime.Value);
            }

            Cache.Set(key, value, policy);
        }

        internal class CacheItem
        {
            public object Value { get; set; }
            public string LockHandle { get; set; }
            public DateTime LockedUntil { get; set; }
        }
    }

}
