using Android.Support.V4.Widget;
using Android.Views;

namespace Xamarin.Core.Droid
{
	public class XamarinDrawer
	{
		private DrawerLayout _drawerLayuout;

		public XamarinDrawer(DrawerLayout drawerLayout)
		{
			_drawerLayuout = drawerLayout;
		}

		public bool IsDrawerOpened(GravityFlags drawerGravity)
		{
			if (_drawerLayuout != null)
			{
				const GravityFlags leftGravity = GravityFlags.Left;
				const GravityFlags rightGravity = GravityFlags.Right;
				if (drawerGravity == leftGravity || drawerGravity == rightGravity)
				{
					return _drawerLayuout.IsDrawerOpen((int)drawerGravity);
				}
			}
			return false;
		}

		public bool IsDrawersOpened()
		{
			return IsDrawerOpened(GravityFlags.Left) || IsDrawerOpened(GravityFlags.Right);
		}

		public void CloseDrawer(GravityFlags drawerGravity)
		{
			if (IsDrawerOpened(drawerGravity))
			{
				_drawerLayuout.CloseDrawer((int)drawerGravity);
			}
		}

		public void CloseDrawers()
		{
			if (IsDrawersOpened())
			{
				_drawerLayuout.CloseDrawers();
			}
		}

		public void OpenDrawer(GravityFlags drawerGravity)
		{
			if (!IsDrawerOpened(drawerGravity))
			{
				_drawerLayuout.OpenDrawer((int)drawerGravity);
			}
		}

		public void SetDrawerEnabled(bool enabled, GravityFlags drawerGravity)
		{
			if (_drawerLayuout != null)
			{
				int lockMode = enabled ? DrawerLayout.LockModeUnlocked : DrawerLayout.LockModeLockedClosed;
				_drawerLayuout.SetDrawerLockMode(lockMode, (int)drawerGravity);
			}
		}

		public void SetDrawersEnabled(bool enabled)
		{
			if (_drawerLayuout != null)
			{
				int lockMode = enabled ? DrawerLayout.LockModeUnlocked : DrawerLayout.LockModeLockedClosed;
				_drawerLayuout.SetDrawerLockMode(lockMode);
			}
		}
	}
}
