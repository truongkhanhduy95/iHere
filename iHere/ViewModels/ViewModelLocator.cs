using System;
using GalaSoft.MvvmLight.Ioc;

namespace iHere.Shared.ViewModels
{
	public class ViewModelLocator
	{
		public static void RegisterViewModels()
		{
			SimpleIoc.Default.Register<DummyViewModel>();
			SimpleIoc.Default.Register<LoginViewModel>();
		}
	}
}
