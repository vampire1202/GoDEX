using System;
using System.Configuration;
namespace Maticsoft.Common
{
	public sealed class ConfigHelper
	{
		public static string GetConfigString(string key)
		{
			string cacheKey = "AppSettings-" + key;
			object obj = DataCache.GetCache(cacheKey);
			if (obj == null)
			{
				try
				{
					obj = ConfigurationManager.AppSettings[key];
					if (obj != null)
					{
						DataCache.SetCache(cacheKey, obj, DateTime.Now.AddMinutes(180.0), TimeSpan.Zero);
					}
				}
				catch
				{
				}
			}
			return obj.ToString();
		}
		public static bool GetConfigBool(string key)
		{
			bool result = false;
			string configString = ConfigHelper.GetConfigString(key);
			if (configString != null && string.Empty != configString)
			{
				try
				{
					result = bool.Parse(configString);
				}
				catch (FormatException)
				{
				}
			}
			return result;
		}
		public static decimal GetConfigDecimal(string key)
		{
			decimal result = 0m;
			string configString = ConfigHelper.GetConfigString(key);
			if (configString != null && string.Empty != configString)
			{
				try
				{
					result = decimal.Parse(configString);
				}
				catch (FormatException)
				{
				}
			}
			return result;
		}
		public static int GetConfigInt(string key)
		{
			int result = 0;
			string configString = ConfigHelper.GetConfigString(key);
			if (configString != null && string.Empty != configString)
			{
				try
				{
					result = int.Parse(configString);
				}
				catch (FormatException)
				{
				}
			}
			return result;
		}
	}
}
