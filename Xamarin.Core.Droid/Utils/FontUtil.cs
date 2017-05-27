using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;

namespace Xamarin.Core.Droid
{
	public static class FontUtil
	{
		private const string FontFamilySansSerif = "sans-serif";
		private const string FontFamilySansSerifLight = "sans-serif-light";
		private const string FontFamilySansSerifCondensed = "sans-serif-condensed";
		private const string FontFamilySansSerifThin = "sans-serif-thin";
		private const string FontFamilySansSerifMedium = "sans-serif-medium";

		public const int RobotoRegular = 1;
		public const int RobotoItalic = 2;
		public const int RobotoBold = 3;
		public const int RobotoBoldItalic = 4;
		public const int RobotoLight = 5;
		public const int RobotoLightItalic = 6;
		public const int RobotoThin = 7;
		public const int RobotoThinItalic = 8;
		public const int RobotoCondensed = 9;
		public const int RobotoCondensedItalic = 10;
		public const int RobotoCondensedBold = 11;
		public const int RobotoCondensedBoldItalic = 12;
		public const int RobotoMedium = 13;
		public const int RobotoMediumItalic = 14;

		private static Dictionary<int, Typeface> Typefaces = new Dictionary<int, Typeface>();

		public static Typeface LoadFont(Context context, int font)
		{
			try
			{
				if (Typefaces.ContainsKey(font))
				{
					return Typefaces[font];
				}

				var typeface = CreateFont(context, font);

				if (typeface == null)
				{
					Typefaces.Add(font, typeface);
					return typeface;
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.Write(ex.StackTrace);
			}

			return Typeface.Default;
		}

		private static Typeface CreateFont(Context context, int font)
		{
			Typeface typeface = null;
			switch (font)
			{
				case RobotoRegular:
					typeface = Typeface.Create(FontFamilySansSerif, TypefaceStyle.Normal);
					break;
				case RobotoItalic:
					typeface = Typeface.Create(FontFamilySansSerif, TypefaceStyle.Italic);
					break;
				case RobotoBold:
					typeface = Typeface.Create(FontFamilySansSerif, TypefaceStyle.Bold);
					break;
				case RobotoBoldItalic:
					typeface = Typeface.Create(FontFamilySansSerif, TypefaceStyle.BoldItalic);
					break;
				case RobotoLight:
					typeface = Typeface.Create(FontFamilySansSerifLight, TypefaceStyle.Normal);
					break;
				case RobotoLightItalic:
					typeface = Typeface.Create(FontFamilySansSerifLight, TypefaceStyle.Italic);
					break;
				case RobotoThin:
					typeface = Typeface.Create(FontFamilySansSerifThin, TypefaceStyle.Normal);
					break;
				case RobotoThinItalic:
					typeface = Typeface.Create(FontFamilySansSerifThin, TypefaceStyle.Italic);
					break;
				case RobotoCondensed:
					typeface = Typeface.Create(FontFamilySansSerifCondensed, TypefaceStyle.Normal);
					break;
				case RobotoCondensedItalic:
					typeface = Typeface.Create(FontFamilySansSerifCondensed, TypefaceStyle.Italic);
					break;
				case RobotoCondensedBold:
					typeface = Typeface.Create(FontFamilySansSerifCondensed, TypefaceStyle.Bold);
					break;
				case RobotoCondensedBoldItalic:
					typeface = Typeface.Create(FontFamilySansSerifCondensed, TypefaceStyle.BoldItalic);
					break;
				case RobotoMedium:
					typeface = Typeface.Create(FontFamilySansSerifMedium, TypefaceStyle.Normal);
					break;
				case RobotoMediumItalic:
					typeface = Typeface.Create(FontFamilySansSerifMedium, TypefaceStyle.Italic);
					break;
				default:
					typeface = Typeface.SansSerif;
					break;
			}
			return typeface;
		}
	}
}
