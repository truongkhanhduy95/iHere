using System;
using GalaSoft.MvvmLight.Views;

namespace Xamarin.Core.Interfaces.Views
{
    public interface IDialogServiceEx : IDialogService
    {
        void ShowLoading(string title = null, Action onCompleted = null);

        void HideLoading(Action onCompleted = null);
    }
}

