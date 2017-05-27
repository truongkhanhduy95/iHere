using System.Collections.Generic;
using UIKit;

namespace Xamarin.Core.iOS
{
	public static class ColorUtil
	{
		public const int ColorPrimary = 1;
		public const int ColorPrimaryDark = 2;

		private static Dictionary<int, UIColor> _colors = new Dictionary<int, UIColor>();

		public static UIColor LoadColor(int colorKey)
		{
			if (_colors.ContainsKey(colorKey))
			{
				return _colors[colorKey];
			}
			else
			{
				var color = CreateColor(colorKey);
				_colors.Add(colorKey, color);
				return color;
			}
		}

		private static UIColor CreateColor(int colorKey)
		{
			switch (colorKey)
			{
				case ColorPrimary:
					return UIColor.FromRGB(255, 87, 34);
				case ColorPrimaryDark:
					return UIColor.FromRGB(230, 74, 25);
				default:
					return null;
			}
		}
	}
}
