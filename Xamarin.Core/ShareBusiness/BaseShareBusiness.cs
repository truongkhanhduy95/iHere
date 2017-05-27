using System;
namespace Xamarin.Core
{
	public class BaseShareBusiness<TScreen> where TScreen : IScreen
	{
		protected TScreen Screen { get; private set; }

		public BaseShareBusiness(TScreen screen)
		{
			Screen = screen;
		}
	}
}
