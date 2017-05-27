namespace Xamarin.Core
{
	public interface IDrawerScreen
	{
		void SetDrawerEnabled(bool enabled);

		void OpenDrawer();

		void CloseDrawer();

		bool IsDrawerOpened();
	}
}
