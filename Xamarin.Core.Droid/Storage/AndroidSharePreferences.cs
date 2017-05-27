using System.Collections.Generic;
using Android.Content;

namespace Xamarin.Core.Droid
{
	public class AndroidSharePreferences : IAppStorage
	{
		private const string Tag = "Share Preferences";

		private readonly Context _context;
		private readonly ISharedPreferences _preferences;

		public bool IsDebugLog { get; set; }

		public AndroidSharePreferences(Context context) : this(context, context.PackageName) { }

		public AndroidSharePreferences(Context context, string name) : this(context, name, FileCreationMode.Private) { }

		public AndroidSharePreferences(Context context, FileCreationMode mode) : this(context, context.PackageName, mode) { }

		public AndroidSharePreferences(Context context, string name, FileCreationMode mode)
		{
			_context = context;
			_preferences = context.GetSharedPreferences(name, mode);

			IsDebugLog = false;
		}

		public bool Contains(string key)
		{
			return _preferences.Contains(key);
		}

		public bool SetValue(string key, int value)
		{
			var editor = _preferences.Edit();
			editor.PutInt(key, value);
			if (IsDebugLog)
			{
				var log = string.Format("{0}: Set [Key:{1}] [Value:{2}]", Tag, key, value);
				System.Diagnostics.Debug.WriteLine(log);
			}

			return editor.Commit();
		}

		public int GetValue(string key, int defValue)
		{
			if (_preferences.Contains(key))
			{
				var value = _preferences.GetInt(key, defValue);
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

		public bool SetValue(string key, string value)
		{
			var editor = _preferences.Edit();
			editor.PutString(key, value);
			if (IsDebugLog)
			{
				var log = string.Format("{0}: Set [Key:{1}] [Value:{2}]", Tag, key, value);
				System.Diagnostics.Debug.WriteLine(log);
			}
			return editor.Commit();
		}

		public string GetValue(string key, string defValue)
		{
			if (_preferences.Contains(key))
			{
				var value = _preferences.GetString(key, defValue);
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

		public bool SetValue(string key, bool value)
		{
			var editor = _preferences.Edit();
			editor.PutBoolean(key, value);
			if (IsDebugLog)
			{
				var log = string.Format("{0}: Set [Key:{1}] [Value:{2}]", Tag, key, value);
				System.Diagnostics.Debug.WriteLine(log);
			}
			return editor.Commit();
		}

		public bool GetValue(string key, bool defValue)
		{
			if (_preferences.Contains(key))
			{
				var value = _preferences.GetBoolean(key, defValue);
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

		public bool SetValue(string key, float value)
		{
			var editor = _preferences.Edit();
			editor.PutFloat(key, value);
			if (IsDebugLog)
			{
				var log = string.Format("{0}: Set [Key:{1}] [Value:{2}]", Tag, key, value);
				System.Diagnostics.Debug.WriteLine(log);
			}
			return editor.Commit();
		}

		public float GetValue(string key, float defValue)
		{
			if (_preferences.Contains(key))
			{
				var value = _preferences.GetFloat(key, defValue);
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

		public bool SetValue(string key, long value)
		{
			var editor = _preferences.Edit();
			editor.PutLong(key, value);
			if (IsDebugLog)
			{
				var log = string.Format("{0}: Set [Key:{1}] [Value:{2}]", Tag, key, value);
				System.Diagnostics.Debug.WriteLine(log);
			}
			return editor.Commit();
		}

		public long GetValue(string key, long defValue)
		{
			if (_preferences.Contains(key))
			{
				var value = _preferences.GetLong(key, defValue);
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

		public bool SetValue(string key, List<string> value)
		{
			var editor = _preferences.Edit();
			editor.PutStringSet(key, value);
			if (IsDebugLog)
			{
				var log = string.Format("{0}: Set [Key:{1}] [Value:{2}]", Tag, key, value);
				System.Diagnostics.Debug.WriteLine(log);
			}
			return editor.Commit();
		}

		public List<string> GetValue(string key, List<string> defValue)
		{
			if (_preferences.Contains(key))
			{
				var value = new List<string>();
				value.AddRange(_preferences.GetStringSet(key, defValue));

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
			if (_preferences.Contains(key))
			{
				var editor = _preferences.Edit();
				editor.Remove(key);
				if (IsDebugLog)
				{
					var log = string.Format("{0}: Remove [Key:{1}]", Tag, key);
					System.Diagnostics.Debug.WriteLine(log);
				}
				return editor.Commit();
			}
			else
			{
				return false;
			}
		}
	}
}
