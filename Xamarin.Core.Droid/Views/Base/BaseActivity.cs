using System;
using Android.Content.PM;
using Android.Support.V7.App;
using Android.Views;
using Android.OS;
using Android.Widget;

namespace Xamarin.Core.Droid
{
    public class BaseActivity : AppCompatActivity
    {
        private static BaseActivity _instance;

        public static BaseActivity Instance
        {
            get
            {
                return _instance;
            }
            protected set
            {
                _instance = value;
            }
        }

        public View RootView
        {
            get
            {
                // NOTE
                // - DecorView.RootView includes status bar
                // - Content view doesn't include status bar
                //                return (ViewGroup)Window.DecorView.RootView;
                return Window.DecorView.FindViewById(Android.Resource.Id.Content);
            }
        }

        public BaseActivity()
        {
        }

        public override void OnBackPressed()
        {
            SystemBackHandler.Instance.GoBack();
        }

        public event EventHandler<PermissionEventArgs> OnPermissionsResult;
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            OnPermissionsResult?.Invoke(this, new PermissionEventArgs()
            {
                RequestCode = requestCode,
                Permissions = permissions,
                GrantResults = grantResults
            });
        }
    }

    public class PermissionEventArgs : EventArgs
    {
        public int RequestCode { get; set; }
        public string[] Permissions { get; set; }
        public Permission[] GrantResults { get; set; }
    }
}

