using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Views;

namespace Xamarin.Core.Droid
{
	public abstract class BaseDrawerActivity : BaseFragmentActivity, IDrawerScreen
	{
		private XamarinDrawer Drawer;

		protected abstract int DrawerLayoutId { get; }

		protected abstract int LeftDrawerFragmentContainerId { get; }

		protected abstract int RightDrawerFragmentContainerId { get; }

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			var drawerLayout = FindViewById<DrawerLayout>(DrawerLayoutId);
			Drawer = new XamarinDrawer(drawerLayout);
		}

		protected void SetLeftDrawerFragment(Fragment fragment)
		{
			var trasaction = SupportFragmentManager.BeginTransaction();
			trasaction.Replace(LeftDrawerFragmentContainerId, fragment, fragment.Class.SimpleName);
			trasaction.Commit();
		}

		protected void SetRightDrawerFragment(Fragment fragment)
		{
			var trasaction = SupportFragmentManager.BeginTransaction();
			trasaction.Replace(RightDrawerFragmentContainerId, fragment, fragment.Class.SimpleName);
			trasaction.Commit();
		}

		public override void OnBackPressed()
		{
			if (IsDrawerOpened())
			{
				CloseDrawer();
			}
			else
			{
				base.OnBackPressed();
			}
		}

		public void OpenDrawer()
		{
			Drawer.OpenDrawer(GravityFlags.Left);
		}

		public void CloseDrawer()
		{
			Drawer.CloseDrawers();
		}

		public void SetDrawerEnabled(bool enabled)
		{
			Drawer.SetDrawerEnabled(enabled, GravityFlags.Left);
		}

		public bool IsDrawerOpened()
		{
			return Drawer.IsDrawersOpened();
		}
	}
}

