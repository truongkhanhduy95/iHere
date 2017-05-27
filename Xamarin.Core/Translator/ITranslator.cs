namespace Xamarin.Core
{
	public interface ITranslator
	{
		string Translate(string key);

		string Translate(string key, string defaultText);
	}
}
