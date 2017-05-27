using System;
using Android.Content;

namespace Xamarin.Core.Droid
{
	public class Translator : ITranslator
	{
		private readonly Context _context;

		public Translator(Context context)
		{
			_context = context;
		}

		public string Translate(string key)
		{
			int resId = _context.Resources.GetIdentifier(key, "string", _context.PackageName);
			return resId > 0 ? _context.GetString(resId) : key;
		}

		public string Translate(string key, string defaultText)
		{
			int resId = _context.Resources.GetIdentifier(key, "string", _context.PackageName);
			return resId > 0 ? _context.GetString(resId) : defaultText;
		}
	}
}
