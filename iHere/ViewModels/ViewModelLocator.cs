using System;
using GalaSoft.MvvmLight.Ioc;

namespace iHere.Shared.ViewModels
{
    public class ViewModelLocator
    {
        public static void RegisterViewModel()
        {
            SimpleIoc.Default.Register<DummyViewModel>();
        }
    }
}
