﻿using Microsoft.Practices.ServiceLocation;
using Xamarin.Core;
using Xamarin.Core.Interfaces.Views;

namespace iHere.Shared.Manager
{
    public class BaseManager
    {
        private IHereDatabaseStorage _repository;
        private IAppStorage _appStorage;
        private IDialogServiceEx _dialogService;



        protected IHereDatabaseStorage Repository
        {
            get
            {
                return _repository ?? (_repository = ServiceLocator.Current.GetInstance<IHereDatabaseStorage>());
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
