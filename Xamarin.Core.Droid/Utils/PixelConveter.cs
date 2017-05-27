using Android.Content;
using Android.Util;

namespace Xamarin.Core.Droid
{
	public static class PixelConveter
	{
		public static int FromDp(Context context, int dp)
		{
			return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, context.Resources.DisplayMetrics);
		}

		public static float FromDp(Context context, float dp)
		{
			return TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, context.Resources.DisplayMetrics);
		}
	}
}
