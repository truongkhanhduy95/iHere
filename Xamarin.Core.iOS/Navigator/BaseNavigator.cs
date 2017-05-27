using System;
using CoreAnimation;
using UIKit;

namespace Xamarin.Core.iOS
{
	public class BaseNavigator : INavigator
	{
		protected UINavigationController NavigationController { get; private set; }

        public int CurrentPageKey
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public INavigationConfig Configuration
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private readonly CATransition _transitionAnimation;

		public BaseNavigator(UINavigationController navigationController)
		{
			NavigationController = navigationController;
			_transitionAnimation = GetTransitionAnimation();
		}

		public void Navigate(IScreen screen, bool isAnimated = true)
		{
			if (NavigationController != null && screen != null)
			{
				var viewController = screen as UIViewController;
				if (viewController != null)
				{
					if (isAnimated && _transitionAnimation != null)
					{
						NavigationController.View.Layer.RemoveAllAnimations();
						NavigationController.View.Layer.AddAnimation(_transitionAnimation, "animation");
						isAnimated = false;
					}
					NavigationController.PushViewController(viewController, isAnimated);
				}
			}
		}

		public void NavigateAsRoot(IScreen screen, bool isAnimated = true)
		{
			if (NavigationController != null && screen != null)
			{
				while (NavigationController.ViewControllers.Length > 1)
				{
					var topViewController = NavigationController.TopViewController;
					NavigationController.PopViewController(false);
				}

				var viewController = screen as UIViewController;
				if (viewController != null)
				{
					if (isAnimated && _transitionAnimation != null)
					{
						NavigationController.View.Layer.RemoveAllAnimations();
						NavigationController.View.Layer.AddAnimation(_transitionAnimation, "animation");
						isAnimated = false;
					}
					NavigationController.SetViewControllers(new UIViewController[] { viewController }, isAnimated);
				}
			}
		}

		public void NavigateBack()
		{
			if (NavigationController != null && NavigationController.TopViewController != null)
			{
				NavigationController.PopViewController(true);
			}
		}

		protected virtual CATransition GetTransitionAnimation()
		{
			return null;
		}

        public void GoBack()
        {
            throw new NotImplementedException();
        }

        public void NavigateTo(int pageKey)
        {
            throw new NotImplementedException();
        }

        public void NavigateTo(int pageKey, object parameter)
        {
            throw new NotImplementedException();
        }

        public void Invalidate()
        {
            throw new NotImplementedException();
        }

        public void GoBack(int pageKey)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(int key)
        {
            throw new NotImplementedException();
        }

        public void Present(int pageKey, Action onDismissed = null)
        {
            throw new NotImplementedException();
        }

        public void Present(int pageKey, object parameter, Action onDismissed = null)
        {
            throw new NotImplementedException();
        }

        public void Dismiss()
        {
            throw new NotImplementedException();
        }

        public void NavigateExternal(string url)
        {
            throw new NotImplementedException();
        }
    }
}
