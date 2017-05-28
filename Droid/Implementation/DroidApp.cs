using System;
using GalaSoft.MvvmLight.Ioc;
using iHere.Shared;
using iHere.Shared.ViewModels;
using Xamarin.Core;
using Xamarin.Core.Droid;

namespace iHere.Droid.Implementation
{
	public class DroidApp : BaseApp
	{
		private static DroidApp _instance;

		public static DroidApp Instance
		{
			get
			{
				return _instance ?? (_instance = new DroidApp());
			}
		}

		private DroidApp()
		{
		}

		protected override void RegisterIocOnProject()
		{
			ViewModelLocator.RegisterViewModels();
			SimpleIoc.Default.Register<INavigationConfig, NavigationConfig>();
			SimpleIoc.Default.Register<IHereDatabaseStorage, AndroidDatabaseStorage>();


			RegisterManager();
			// TODO: Register more
		}

		private void RegisterManager()
		{

		}
    }
}
