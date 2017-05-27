using System;
using Xamarin.Core.Utils;

namespace Xamarin.Core.Droid
{
    public class DispatcherHelperImpl : DispatcherHelper
    {
        public DispatcherHelperImpl()
        {
            GalaSoft.MvvmLight.Threading.DispatcherHelper.Initialize();
        }

        public override void RunOnUIThread(Action action)
        {
			// GalaSoft.MvvmLight.Threading.DispatcherHelper.CheckBeginInvokeOnUI(action);

			BaseActivity.Instance.RunOnUiThread(action);
        }
    }
}

