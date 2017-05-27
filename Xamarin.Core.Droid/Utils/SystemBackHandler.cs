using System;
using System.Collections.Generic;
using Android.App;
using Android.Widget;
using Xamarin.Core.Droid.Attributes;

namespace Xamarin.Core.Droid
{
    public class SystemBackHandler
    {
        private static SystemBackHandler _instance = new SystemBackHandler();

        public static SystemBackHandler Instance
        {
            get
            {
                return _instance;
            }
        }

        private Stack<IScreen> _backStack = new Stack<IScreen>();
        private DateTime _lastBackTime;

        private SystemBackHandler()
        {
        }

        public void PushToBackStack(IScreen screen)
        {
            var ignoreAttributes = screen.GetType().GetCustomAttributes(typeof(BackStackIgnoreAttribute), true);
            if (ignoreAttributes == null || ignoreAttributes.Length <= 0)
            {
                _backStack.Push(screen);
            }
        }

        public void PopFromBackStack()
        {
            _backStack.Pop();
        }

        public void GoBack()
        {
            var screen = _backStack.Peek();
            if (screen != null)
            {
                var rootAttributes = screen.GetType().GetCustomAttributes(typeof(BackStackRootAttribute), true);
                if (rootAttributes != null && rootAttributes.Length > 0)
                {
                    var twoBackTime = DateTime.Now - _lastBackTime;
                    if (twoBackTime.TotalMilliseconds < 1000)
                    {
                        BaseActivity.Instance.Finish();
                    }
                    else
                    {
                        Toast.MakeText(Application.Context, "Press back button again to exit!", ToastLength.Short).Show();
                    }
                    _lastBackTime = DateTime.Now;
                }
                else
                {
                    screen.Navigator.GoBack();
                }
            }
        }
    }
}

