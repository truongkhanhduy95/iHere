using Android.Util;
using Android.Graphics;

namespace iHere.Droid.Utils
{
	public static class ScreenUtils
	{
		private static int _screenWidth, _screenHeight, _density, _iconSize;
		private static bool _isTablet = false;

		static ScreenUtils()
		{
			Point size = new Point();
			MainActivity.Instance.WindowManager.DefaultDisplay.GetSize(size);
			_screenWidth = size.X;
			_screenHeight = size.Y;
			_density = (int)(MainActivity.Instance.Resources.DisplayMetrics.Density * 160f);
			_iconSize = GetIconSize();

			_isTablet = (MainActivity.Instance.Resources.Configuration.ScreenLayout & Android.Content.Res.ScreenLayout.SizeMask) >= Android.Content.Res.ScreenLayout.SizeLarge;
		}

		public static int ScreenWidth
		{
			get
			{
				return _screenWidth;
			}
		}

		public static int ScreenHeight
		{
			get
			{
				return _screenHeight;
			}
		}

		public static int Density
		{
			get
			{
				return _density;
			}
		}

		public static int IconSize
		{
			get
			{
				return _iconSize;
			}
		}

		public static bool IsTablet
		{
			get
			{
				return _isTablet;
			}
		}

		public static float PxToDp(float px)
		{
			return px / MainActivity.Instance.Resources.DisplayMetrics.Density;
		}

		public static float DpToPx(float dp)
		{
			return dp * MainActivity.Instance.Resources.DisplayMetrics.Density;
		}

		private static int GetIconSize()
		{
			int size = 96;
			var densityDpi = MainActivity.Instance.Resources.DisplayMetrics.DensityDpi;

			switch (densityDpi)
			{
				case DisplayMetricsDensity.Low:
					// LDPI
					size = 48;
					break;

				case DisplayMetricsDensity.Medium:
					// MDPI
					size = 64;
					break;

				case DisplayMetricsDensity.High:
					size = 72;
					break;

				case DisplayMetricsDensity.Xhigh:
					size = 96;
					break;

				case DisplayMetricsDensity.Xxhigh:
					size = 144;
					break;

				case DisplayMetricsDensity.Xxxhigh:
					size = 192;
					break;

				case DisplayMetricsDensity.Tv:
					size = 72;

					break;
			}

			return size;
		}
	}
}
