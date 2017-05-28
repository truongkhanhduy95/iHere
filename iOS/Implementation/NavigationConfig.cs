using System;
using iHere.Shared;
using Xamarin.Core;
using Xamarin.Core.iOS;

namespace iHere.iOS
{
    public class NavigationConfig : INavigationConfig
    {
        public void Decorate(IScreen screen)
        {
        }

        public void Setup(INavigator nav)
        {
            var navigator = nav as Navigator;
			if (navigator == null)
			{
				throw new Exception("Dev >>> Navigator must be inherited from Navigator class");
			}

            navigator.Configure((int)ScreenKey.Dummy, typeof(DummyViewController));
        }
    }
}
