using Android.App;
using Android.Content;
using Android.Views.InputMethods;
using Android.Widget;

namespace Xamarin.Core.Droid
{
	public static class SoftKeyboardUtil
	{
		public static void HideKeyboard(Context context, EditText editText = null)
		{
			var inputManager = (InputMethodManager)context.GetSystemService(Context.InputMethodService);
			if (editText != null && editText.IsFocused)
			{
				editText.ClearFocus();
				inputManager.HideSoftInputFromWindow(editText.WindowToken, HideSoftInputFlags.NotAlways);
			}
			else
			{
				var activity = context as Activity;
				if (activity != null)
				{
					if (activity.CurrentFocus != null)
					{
						activity.CurrentFocus.ClearFocus();
						inputManager.HideSoftInputFromWindow(activity.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
					}
					else
					{
						inputManager.HideSoftInputFromWindow(activity.Window.DecorView.RootView.WindowToken, HideSoftInputFlags.NotAlways);
					}
				}

			}
		}
	}
}
