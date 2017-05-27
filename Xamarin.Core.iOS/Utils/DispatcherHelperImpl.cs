using System;
using UIKit;
using Xamarin.Core.Utils;

namespace Xamarin.Core.iOS
{
    public class DispatcherHelperImpl : DispatcherHelper
    {
        public DispatcherHelperImpl(UIApplication application)
        {
            GalaSoft.MvvmLight.Threading.DispatcherHelper.Initialize(application);
        }

        public override void RunOnUIThread(Action action)
        {
            GalaSoft.MvvmLight.Threading.DispatcherHelper.CheckBeginInvokeOnUI(action);
        }
    }
}