namespace Xamarin.Core
{
	public interface IAppStorage
	{
		bool SetValue(string key, int value);
		int GetValue(string key, int defValue);
		bool SetValue(string key, bool value);
		bool GetValue(string key, bool defValue);
		bool SetValue(string key, long value);
		long GetValue(string key, long defValue);
		bool SetValue(string key, float value);
		float GetValue(string key, float defValue);
		bool SetValue(string key, string value);
		string GetValue(string key, string defValue);
		bool Remove(string key);
		bool Contains(string key);
	}
}
