using System;
using iHere.Shared;
using Xamarin.Core;
using Xamarin.Core.Droid;

namespace iHere.Droid
{
    public class NavigationConfig : INavigationConfig
    {
        public void Setup(INavigator nav)
        {
            var navigator = nav as Navigator;
            if (navigator == null)
            {
                throw new Exception("Dev >>> Navigator must be inherited from Navigator class");
            }
            // Config
            navigator.Configure((int)ScreenKey.Dummy, typeof(DummyFragment));

            // Configure more here
        }

        public void Decorate(IScreen screen)
        {
            if (screen == null)
            {
                return;
            }

            // Set screen props here, check by screen's key
            var key = (ScreenKey)screen.Key;
            switch (key)
            {
                default:
                    break;
            }
        }
    }
}
