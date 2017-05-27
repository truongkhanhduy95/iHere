using System;
using Foundation;

namespace Xamarin.Core.iOS
{
	public class iOSUserDefaults : IAppStorage
	{
		private const string Tag = "iOSUserDefaults";

		private readonly NSUserDefaults _userDefaults;

		public bool IsDebugLog { get; set; }

		public iOSUserDefaults()
		{
			_userDefaults = NSUserDefaults.StandardUserDefaults;
		}

		public bool GetValue(string key, bool defValue)
		{
			if (Contains(key))
			{
				var value = _userDefaults.BoolForKey(key);
				if (IsDebugLog)
				{
					var log = string.Format("{0}: Get [Key:{1}] [Value:{2}]", Tag, key, value);
					System.Diagnostics.Debug.WriteLine(log);
				}
				return value;
			}
			else
			{
				return defValue;
			}
		}

		public long GetValue(string key, long defValue)
		{
			if (Contains(key))
			{
				var value = Convert.ToInt64(_userDefaults.DoubleForKey(key));
				if (IsDebugLog)
				{
					var log = string.Format("{0}: Get [Key:{1}] [Value:{2}]", Tag, key, value);
					System.Diagnostics.Debug.WriteLine(log);
				}
				return value;
			}
			else
			{
				return defValue;
			}
		}

		public float GetValue(string key, float defValue)
		{
			if (Contains(key))
			{
				var value = _userDefaults.FloatForKey(key);
				if (IsDebugLog)
				{
					var log = string.Format("{0}: Get [Key:{1}] [Value:{2}]", Tag, key, value);
					System.Diagnostics.Debug.WriteLine(log);
				}
				return value;
			}
			else
			{
				return defValue;
			}
		}

		public string GetValue(string key, string defValue)
		{
			if (Contains(key))
			{
				var value = _userDefaults.StringForKey(key);
				if (IsDebugLog)
				{
					var log = string.Format("{0}: Get [Key:{1}] [Value:{2}]", Tag, key, value);
					System.Diagnostics.Debug.WriteLine(log);
				}
				return value;
			}
			else
			{
				return defValue;
			}
		}

		public int GetValue(string key, int defValue)
		{
			if (Contains(key))
			{
				var value = Convert.ToInt32(_userDefaults.IntForKey(key));
				if (IsDebugLog)
				{
					var log = string.Format("{0}: Get [Key:{1}] [Value:{2}]", Tag, key, value);
					System.Diagnostics.Debug.WriteLine(log);
				}
				return value;
			}
			else
			{
				return defValue;
			}
		}

		public bool Remove(string key)
		{
			if (Contains(key))
			{
				_userDefaults.RemoveObject(key);
				if (IsDebugLog)
				{
					var log = string.Format("{0}: Remove [Key:{1}]", Tag, key);
					System.Diagnostics.Debug.WriteLine(log);
				}
				return _userDefaults.Synchronize();
			}
			else
			{
				return false;
			}
		}

		public bool SetValue(string key, long value)
		{
			_userDefaults.SetDouble(value, key);
			if (IsDebugLog)
			{
				var log = string.Format("{0}: Set [Key:{1}] [Value:{2}]", Tag, key, value);
				System.Diagnostics.Debug.WriteLine(log);
			}

			return _userDefaults.Synchronize();
		}

		public bool SetValue(string key, string value)
		{
			_userDefaults.SetString(value, key);
			if (IsDebugLog)
			{
				var log = string.Format("{0}: Set [Key:{1}] [Value:{2}]", Tag, key, value);
				System.Diagnostics.Debug.WriteLine(log);
			}

			return _userDefaults.Synchronize();
		}

		public bool SetValue(string key, float value)
		{
			_userDefaults.SetFloat(value, key);
			if (IsDebugLog)
			{
				var log = string.Format("{0}: Set [Key:{1}] [Value:{2}]", Tag, key, value);
				System.Diagnostics.Debug.WriteLine(log);
			}

			return _userDefaults.Synchronize();
		}

		public bool SetValue(string key, bool value)
		{
			_userDefaults.SetBool(value, key);
			if (IsDebugLog)
			{
				var log = string.Format("{0}: Set [Key:{1}] [Value:{2}]", Tag, key, value);
				System.Diagnostics.Debug.WriteLine(log);
			}

			return _userDefaults.Synchronize();
		}

		public bool SetValue(string key, int value)
		{
			_userDefaults.SetInt(value, key);
			if (IsDebugLog)
			{
				var log = string.Format("{0}: Set [Key:{1}] [Value:{2}]", Tag, key, value);
				System.Diagnostics.Debug.WriteLine(log);
			}

			return _userDefaults.Synchronize();
		}

		public bool Contains(string key)
		{
			var obj = _userDefaults.ValueForKey(new NSString(key));
			return obj != null;
		}
	}
}
