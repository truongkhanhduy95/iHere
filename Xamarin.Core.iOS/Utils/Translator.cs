using System;
using Foundation;

namespace Xamarin.Core.iOS
{
	public class Translator : ITranslator
	{
		private static Translator _instance;

		public static Translator Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new Translator();
				}
				return _instance;
			}
		}

		public string Translate(string key)
		{
			var text = NSBundle.MainBundle.LocalizedString(key, key);
			return !string.IsNullOrEmpty(text) ? text : key;
		}

		public string Translate(string key, string defaultText)
		{
			var text = NSBundle.MainBundle.LocalizedString(key, key);
			return !string.IsNullOrEmpty(text) && text != key ? text : defaultText;
		}
	}
}
