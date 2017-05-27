using System;

namespace Xamarin.Core
{
	public interface INavigator
	{
        int CurrentPageKey { get; }
        INavigationConfig Configuration { get; }
        void GoBack();
        void NavigateTo(int pageKey);
        void NavigateTo(int pageKey, object parameter);
        void Invalidate();
        void GoBack(int pageKey);
        bool ContainsKey(int key);
        void Present(int pageKey, Action onDismissed = null);
        void Present(int pageKey, object parameter, Action onDismissed = null);
        void Dismiss();
        void NavigateExternal(string url);

        #region IScreen Navigator
        //void Navigate(IScreen screen, bool isAnimated = true);

        //void NavigateAsRoot(IScreen screen, bool isAnimated = true);

        //void NavigateBack();
        #endregion
    }

    public interface ICanNavigate
    {
        INavigator Navigator { get; set; }
    }

    public interface INavigationConfig
    {
        void Setup(INavigator nav);
        void Decorate(IScreen screen);
    }
}

