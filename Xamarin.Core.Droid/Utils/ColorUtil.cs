using Android.Content;
using Android.Graphics;

namespace Xamarin.Core.Droid
{
	public static class ColorUtil
	{
		public static Color GetColor(Context context, int id)
		{
			return context.Resources.GetColor(id);
		}
	}
}
