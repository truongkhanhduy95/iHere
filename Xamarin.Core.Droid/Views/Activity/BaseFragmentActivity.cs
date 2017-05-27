using System;
using Android.App;
using Android.Content;
using Android.OS;
using Fragment = Android.Support.V4.App.Fragment;

namespace Xamarin.Core.Droid
{
	public abstract class BaseFragmentActivity : BaseActivity
	{
		protected abstract int FragmentContainerId { get; }

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			var fragment = SupportFragmentManager.FindFragmentById(FragmentContainerId);
			if (fragment != null)
			{
				fragment.OnActivityResult(requestCode, (int)resultCode, data);
			}
		}

		protected override bool HandleBackPressed()
		{
			if (SupportFragmentManager.BackStackEntryCount > 1)
			{
				var currentFragment = SupportFragmentManager.FindFragmentById(FragmentContainerId);
				if (currentFragment != null)
				{
					if (currentFragment is IFragmentBackHandler)
					{
						var isHandled = ((IFragmentBackHandler)currentFragment).HandleBackPressed();
						if (isHandled)
						{
							return true;
						}
					}

					if (currentFragment.TargetFragment != null)
					{
						currentFragment.TargetFragment.OnActivityResult(currentFragment.TargetRequestCode, (int)Result.Canceled, null);
					}
				}
				return false;
			}
			else
			{
				return base.HandleBackPressed();
			}
		}

		protected void SetContentFragment(Fragment fragment)
		{
			var transaction = SupportFragmentManager.BeginTransaction();
			transaction.SetCustomAnimations(Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out, Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
			transaction.Replace(FragmentContainerId, fragment, fragment.Class.SimpleName);
			transaction.AddToBackStack(null);
			transaction.Commit();
		}
	}

	public interface IFragmentBackHandler
	{
		bool HandleBackPressed();
	}
}
