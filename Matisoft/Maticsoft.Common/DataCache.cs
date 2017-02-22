using System;
using System.Web;
using System.Web.Caching;
namespace Maticsoft.Common
{
	public class DataCache
	{
		public static object GetCache(string CacheKey)
		{
			Cache cache = HttpRuntime.Cache;
			return cache[CacheKey];
		}
		public static void SetCache(string CacheKey, object objObject)
		{
			Cache cache = HttpRuntime.Cache;
			cache.Insert(CacheKey, objObject);
		}
		public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
		{
			Cache cache = HttpRuntime.Cache;
			cache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);
		}
		public static void DeleteCache(string CacheKey)
		{
			Cache cache = HttpRuntime.Cache;
			cache.Remove(CacheKey);
		}
	}
}
