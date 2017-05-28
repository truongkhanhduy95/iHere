using System;
using iHere.Shared.Storage;
using Microsoft.Practices.ServiceLocation;
using Xamarin.Core;
using Xamarin.Core.Interfaces.Views;

namespace iHere.Shared.Manager
{
    public class BaseManager
    {
        private IHereStorage _repository;
        private IAppStorage _appStorage;
        private IDialogServiceEx _dialogService;



        protected IHereStorage Repository
        {
            get
            {
                return _repository ?? (_repository = ServiceLocator.Current.GetInstance<IHereStorage>());
            }
        }

        protected IAppStorage AppStorage
        {
            get
            {
                return _appStorage ?? (_appStorage = ServiceLocator.Current.GetInstance<IAppStorage>());
            }
        }

        protected IDialogServiceEx DialogService
        {
            get
            {
                return _dialogService ?? (_dialogService = ServiceLocator.Current.GetInstance<IDialogServiceEx>());
            }
        }
    }
}
