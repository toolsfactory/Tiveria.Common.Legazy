using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiveria.Common.Extensions
{
    public static class DictionaryExtensions
    {
        public static void SetValue<TKey, TValue>(this IDictionary<TKey, TValue> bag, TKey key, TValue value)
        {
            if (bag.ContainsKey(key))
            {
                bag[key] = value;
            }
            else
            {
                bag.Add(key, value);
            }
        }

        public static void SetValue<TKey, TValue>(this IDictionary<TKey, TValue> bag, TKey key, object value, TValue defaultValue = default(TValue))
        {
            TValue newValue;
            try
            {
                newValue = (TValue)Convert.ChangeType(value, typeof(TValue));
            }
            catch
            {
                newValue = defaultValue;
            }
            if (bag.ContainsKey(key))
            {
                bag[key] = newValue;
            }
            else
            {
                bag.Add(key, newValue);
            }
        }


        public static T GetValue<T, TKey, TValue>(this IDictionary<TKey, TValue> bag, TKey key, T defaultvalue)
        {
            if (bag == null)
                return defaultvalue;


            if (bag.ContainsKey(key))
            {
                var coreValue = bag[key];
                try
                {
                    T value = (T)Convert.ChangeType(coreValue, typeof(T));
                    return value;
                }
                catch
                {
                    return defaultvalue;
                }
            }
            else
                return defaultvalue;
        }
    }
}
