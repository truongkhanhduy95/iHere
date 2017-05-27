using System;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using Xamarin.Core.Interfaces.Views;

namespace Xamarin.Core.ViewModels
{
    public class BaseViewModel : ViewModelBase, ICanNavigate, IBusy
    {
        protected readonly object _locker = new object();
        private INavigator _navigator;

        public INavigator Navigator
        {
            get
            {
                return _navigator ?? ServiceLocator.Current.GetInstance<INavigator>();
            }
            set
            {
                _navigator = value;
            }
        }

        private IDialogServiceEx _dialogService;

        public IDialogServiceEx DialogService
        {
            get
            {
                return _dialogService ?? (_dialogService = ServiceLocator.Current.GetInstance<IDialogServiceEx>());
            }
        }

        private bool _isBusy;

        public virtual bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                lock (_locker)
                {
                    if (_isBusy != value)
                    {
                        if (value)
                        {
                            System.Diagnostics.Debug.WriteLine($">>>>>>>>>>>>>> Show loading... on Busy {this.GetType().ToString()}");
                            DialogService?.ShowLoading();
                        }
                        else {
                            System.Diagnostics.Debug.WriteLine($">>>>>>>>>>>>>> Hide loading... on Busy");
                            DialogService?.HideLoading();
                        }
                        _isBusy = value;
                    }
                }
            }
        }

        public event Action RequestingEnded;

        public BaseViewModel()
        {
            InitCommands();

            RegisterMessages();
        }

        protected virtual void InitCommands() { }

        public virtual void ApplyDefaultValue()
        {

        }

        public virtual void RegisterMessages()
        {
            UnregisterMessages();
        }

        public virtual void UnregisterMessages() { }

        protected virtual void OnRequestingEnded()
        {
            RequestingEnded?.Invoke();
        }
    }

    public interface IBusy
    {
        bool IsBusy { get; set; }
    }
}

