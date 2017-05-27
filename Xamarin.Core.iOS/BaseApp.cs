using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using UIKit;
using Xamarin.Core.Interfaces.Views;
using Xamarin.Core.iOS.Views;
using Xamarin.Core.Utils;

namespace Xamarin.Core.iOS
{
    public abstract class BaseApp
    {
        protected BaseApp()
        {
        }

        public void Initialize(UIApplication application)
        {
            if (!ServiceLocator.IsLocationProviderSet)
            {
                ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
                RegisterIoc(application);
                RegisterIocOnProject();
            }
        }

        private void RegisterIoc(UIApplication application)
        {
            // Dispatcher
            // Create new instance here to force init dispatcher bind to UI thread
            SimpleIoc.Default.Register<DispatcherHelper>(() => new DispatcherHelperImpl(application), true);
            SimpleIoc.Default.Register<INavigator, Navigator>();
            // Dialog service
            SimpleIoc.Default.Register<IDialogServiceEx, DialogServiceEx>();
            SimpleIoc.Default.Register<ILoading, LoadingOverlayView>();

            SimpleIoc.Default.Register<IAppStorage, iOSUserDefaults>();

        }

        protected abstract void RegisterIocOnProject();

        public void UnregisterIoc()
        {
            // Implement if need
        }
    }
}

