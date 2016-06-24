using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Text.RegularExpressions;

namespace OpenData.Common.Caching
{
    /// <summary>
    /// Represents a MemoryCacheCache
    /// </summary>
    public partial class MemoryCacheManager : ICacheManager
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        public T Get<T>(string key)
        {
            try
            {
                if (IsSet(key))
                {
                    return (T)Cache[key];
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception ex)
            {

                log.Error("MemoryCacheManager", ex);
                return default(T);
            }
        }

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache time</param>
        public void Set(string key, object data, int cacheTime)
        {
            try
            {
                if (data == null)
                    return;

                var policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromSeconds(cacheTime);
                Cache.Add(new CacheItem(key, data), policy);
            }
            catch (Exception ex)
            {
                log.Error("MemoryCacheManager", ex);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Result</returns>
        public bool IsSet(string key)
        {
            try
            {
                return (Cache.Contains(key));
            }
            catch (Exception ex)
            {
                log.Error("MemoryCacheManager", ex);
                return true;
            }
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">/key</param>
        public void Remove(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="pattern">pattern</param>
        public void RemoveByPattern(string pattern)
        {
            try
            {
                var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
                var keysToRemove = new List<String>();

                foreach (var item in Cache)
                    if (regex.IsMatch(item.Key))
                        keysToRemove.Add(item.Key);

                foreach (string key in keysToRemove)
                {
                    Remove(key);
                }
            }
            catch (Exception ex)
            {

                log.Error("MemoryCacheManager", ex);
            }
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public void Clear()
        {
            foreach (var item in Cache)
                Remove(item.Key);
        }
    }
}