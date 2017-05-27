using System;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using AD = Android;

namespace Xamarin.Core.Droid
{
	public abstract class BaseActivity : AppCompatActivity
	{
		private static BaseActivity _instance;

		public static BaseActivity Instance
		{
			get
			{
				return _instance;
			}
			protected set
			{
				_instance = value;
			}		 }


		public View RootView
		{
			get
			{
				// NOTE
				// - DecorView.RootView includes status bar
				// - Content view doesn't include status bar
				//                return (ViewGroup)Window.DecorView.RootView;
				return Window.DecorView.FindViewById(AD.Resource.Id.Content);
			}		 }


		//protected abstract int ActivityLayoutResource { get; }

		protected bool ExitAppOnBack { get; set; }

		protected int ExitAppOnBackDelay { get; set; }

		protected string ExitMessage { get; set; }

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			//SetContentView(ActivityLayoutResource);
			//BindControls();
			ExitAppOnBack = true;
			ExitAppOnBackDelay = 2 * 1000;
			ExitMessage = "Please click BACK again to exit";
		}

		//protected abstract void BindControls();

		public override void OnBackPressed()
		{
			var backHandled = HandleBackPressed();
			if (!backHandled)
			{
				base.OnBackPressed();
			}
		}

		protected virtual bool HandleBackPressed()
		{
			if (ExitAppOnBack)
			{
				Finish();
			}
			else
			{
				ExitAppOnBack = true;
				Toast.MakeText(this, ExitMessage, ToastLength.Short).Show();

				new Handler().PostDelayed(() =>
				{
					ExitAppOnBack = false;
				}, ExitAppOnBackDelay);
			}
			return true;
		}

		public event EventHandler<PermissionEventArgs> OnPermissionsResult;
		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			OnPermissionsResult?.Invoke(this, new PermissionEventArgs()
			{
				RequestCode = requestCode,
				Permissions = permissions,
				GrantResults = grantResults
			}); 
		}
	}

	public class PermissionEventArgs : EventArgs
	{
		public int RequestCode { get; set; }
		public string[] Permissions { get; set; }
		public Permission[] GrantResults { get; set; }	 }
}