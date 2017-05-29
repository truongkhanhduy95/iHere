using Android.App;
using Android.Widget;
using Android.OS;
using Xamarin.Core.Droid;
using iHere.Droid.Implementation;
using iHere.Shared;
using Microsoft.Practices.ServiceLocation;
using Xamarin.Core;

namespace iHere.Droid
{
	[Activity(Label = "LarvikHK", MainLauncher = true, Icon = "@mipmap/icon", Theme = "@style/MyTheme")]
	public class MainActivity : BaseActivity
	{
		private static MainActivity _context;

		public static MainActivity Context
		{
			get
			{
				return MainActivity._context;
			}
		}

		//protected override int ActivityLayoutResource
		//{
		//  get
		//  {
		//      throw new NotImplementedException();
		//  }
		//}

		//protected override void BindControls()
		//{
		//  throw new NotImplementedException();
		//}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Cached instance
			Instance = this;
			// Init app
			DroidApp.Instance.Initialize();
			var nav = SetupRootNavigator();
			nav.NavigateTo((int)ScreenKey.Dummy);
		}

		private Navigator SetupRootNavigator()
		{
			var nav = ServiceLocator.Current.GetInstance<INavigator>() as Navigator;
			nav.Initialize(this, FragmentManager, RootView.Id);
			return nav;
		}
	}
}

