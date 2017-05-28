using System;
using GalaSoft.MvvmLight.Ioc;
using iHere.Shared;
using iHere.Shared.ViewModels;
using Xamarin.Core;
using Xamarin.Core.iOS;

namespace iHere.iOS
{
    public class iOSApp : BaseApp
    {
        public static iOSApp _instance;

        public static iOSApp Instance
        {
            get
            {
                return _instance ?? (_instance = new iOSApp());
            }
        }

        public iOSApp()
        {
        }

        protected override void RegisterIocOnProject()
        {
            ViewModelLocator.RegisterViewModels();
            SimpleIoc.Default.Register<INavigationConfig,NavigationConfig>();
            SimpleIoc.Default.Register<IHereDatabaseStorage,iOSDatabaseStorage>();
        }
    }
}
