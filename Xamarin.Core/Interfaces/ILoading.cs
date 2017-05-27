using System;
namespace Xamarin.Core.Interfaces.Views
{
    public interface ILoading
    {
        string Title { get; set; }

        void Show(Action onCompleted = null);

        void Hide(Action onCompleted = null);
    }
}

