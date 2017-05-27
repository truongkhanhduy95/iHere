using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Xamarin.Core.Interfaces.Views;
using Xamarin.Core.Utils;

namespace Xamarin.Core.Droid
{
    public abstract class BaseApp
    {
        protected BaseApp()
        {
        }

        public void Initialize()
        {
            if (!ServiceLocator.IsLocationProviderSet)
            {
                ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
                RegisterIoc();
                RegisterIocOnProject();
            }
        }

        private void RegisterIoc()
        {
            // Dialog service
            SimpleIoc.Default.Register<IDialogServiceEx, DialogServiceEx>();
            SimpleIoc.Default.Register<ILoading, LoadingOverlayView>();
            // Dispatcher
            // Create new instance here to force init dispatcher bind to UI thread
            SimpleIoc.Default.Register<DispatcherHelper>(() => new DispatcherHelperImpl(), true);
            SimpleIoc.Default.Register<INavigator, Navigator>();
			
			// Storage
			SimpleIoc.Default.Register<IAppStorage>(() => new AndroidSharePreferences(BaseActivity.Instance), false);

        }

        protected abstract void RegisterIocOnProject();

        public void UnregisterIoc()
        {
            // Implement if need
        }

    }
}

