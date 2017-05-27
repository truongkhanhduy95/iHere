using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Foundation;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using UIKit;
using Xamarin.Core;

namespace Xamarin.Core.iOS
{
    public class Navigator : UINavigationControllerDelegate, INavigator
    {
        private readonly Dictionary<int, Type> _screenSpecs = new Dictionary<int, Type>();
        private readonly List<int> _lastScreenKeys = new List<int> { -1 };
        private INavigationConfig _configuration;

        public Action OnDismissViewController { get; set; }

        public UINavigationController NavigationController { get; set; }

        public INavigationConfig Configuration
        {
            get
            {
                return _configuration ?? (_configuration = ServiceLocator.Current.GetInstance<INavigationConfig>());
            }
        }

        public int CurrentPageKey
        {
            get { return CurrentScreen?.Key ?? -1; }
        }

        public IScreen CurrentScreen
        {
            get
            {
                return NavigationController.TopViewController as IScreen;
            }
        }

        [PreferredConstructor]
        public Navigator()
            : this(new UINavigationController())
        {
        }

        public Navigator(UINavigationController navigationController)
        {
            NavigationController = navigationController;
            NavigationController.Delegate = this;
            NavigationController.NavigationBarHidden = true;

            Configuration.Setup(this);
        }

        public bool ContainsKey(int key)
        {
            return _screenSpecs.ContainsKey(key);
        }

        public void GoBack()
        {
            var fromViewController = NavigationController.TopViewController;
            if (fromViewController != null && fromViewController is IScreen)
            {
                GoBack(-1);
            }
            else
            {
                throw new Exception("Dev >>> Don't support non-INavigableScreen type!!!!!!!!!. Also, cannot back from null UIViewController");
            }
        }

        public void GoBack(int pageKey)
        {
            if (CurrentPageKey > 0 && pageKey == CurrentPageKey)
            {
                return;
            }

            if (pageKey == -1)
            {
                SetIsNavigateBack(GetViewController(_lastScreenKeys[_lastScreenKeys.Count - 2]), true);
                NavigationController.PopViewController(true);
                _lastScreenKeys.RemoveAt(_lastScreenKeys.Count - 1);
                return;
            }

            var toViewController = GetViewController(pageKey);
            if (toViewController != null)
            {
                SetIsNavigateBack(toViewController, true);
                var index = _lastScreenKeys.IndexOf(pageKey) + 1;
                var count = _lastScreenKeys.Count - index;
                for (int i = 0; i < count; i++)
                {
                    NavigationController.PopViewController(false);
                }
                _lastScreenKeys.RemoveRange(index, count);
            }
            else
            {
                toViewController = GetScreen(pageKey, null);
                SetIsNavigateBack(toViewController, false);
                for (int i = NavigationController.ViewControllers.Length - 1; i >= 0; i--)
                {
                    NavigationController.PopViewController(false);
                }
                NavigationController.PushViewController(toViewController, false);
                _lastScreenKeys.Add(pageKey);
            }
        }

        private UIViewController GetViewController(int pageKey)
        {
            return NavigationController.ViewControllers.ToList().FirstOrDefault(vc =>
            {
                IScreen screen;
                if (vc is IScreen)
                {
                    screen = vc as IScreen;
                    if (screen.Key == pageKey)
                    {
                        return true;
                    }
                }
                return false;
            });
        }

        private void SetIsNavigateBack(UIViewController viewController, bool isBack)
        {
            if (viewController is IScreen)
            {
                (viewController as IScreen).IsNavigateBack = isBack;
            }
        }

        public void Invalidate()
        {
            NavigationController.ViewControllers.ToList().ForEach((obj) =>
            {
                obj.RemoveFromParentViewController();
            });
            _lastScreenKeys.Clear();
        }

        public void NavigateTo(int pageKey)
        {
            NavigateTo(pageKey, null);
        }

        public void NavigateTo(int pageKey, object parameter)
        {
            lock (_screenSpecs)
            {
                System.Diagnostics.Debug.WriteLine($"From screen: {pageKey}, To Screen: {CurrentPageKey}");

                if (CurrentPageKey == pageKey)
                {
                    CurrentScreen.NavigationParameter = parameter;
                    return;
                }

                var toViewController = GetScreen(pageKey, parameter);
                if (toViewController == null)
                {
                    throw new InvalidOperationException("Can't Init ViewController");
                }

                var toScreen = toViewController as IScreen;
                if (toScreen.IsRoot)
                {
                    // Clear backstack
                    Invalidate();
                    System.Diagnostics.Debug.WriteLine($">>>>>>>>>>>>>>>> Current screen's key >>> {CurrentScreen?.Key}");
                }
                toScreen.IsNavigateBack = false;

                var fromScreen = CurrentScreen;

                if (toScreen is UIViewController)
                {
                    var toVC = toScreen as UIViewController;
                    if (NavigationController.ViewControllers.Contains(toVC))
                    {
                        NavigationController.ViewControllers.FirstOrDefault(vc => vc != toVC).RemoveFromParentViewController();
                    }
                    NavigationController.PushViewController(toVC, true);
                }
                else
                {
                    throw new Exception("Dev >>> toScreen must be derived from UIViewController. NOT support others");
                }

                _lastScreenKeys.Add(pageKey);
            }
        }

        private UIViewController GetScreen(int pageKey, object parameter)
        {
            ConstructorInfo constructor = null;
            object[] parameters = null;
            Type fragmentType = _screenSpecs[pageKey];

            if (parameter != null)
            {
                constructor = fragmentType.GetTypeInfo()
                    .DeclaredConstructors
                    .FirstOrDefault(
                    c =>
                    {
                        var p = c.GetParameters();
                        return p.Count() == 1
                        && p[0].ParameterType == parameter.GetType();
                    });

                parameters = new[]
                {
                    parameter
                };
            }
            if (constructor == null)
            {
                constructor = fragmentType.GetTypeInfo()
                    .DeclaredConstructors
                    .FirstOrDefault(c => !c.GetParameters().Any());

                parameters = new object[]
                {
                };
            }

            if (constructor == null)
            {
                return null;
            }

            var viewController = constructor.Invoke(parameters);
            var screen = viewController as IScreen;
            if (screen != null)
            {
                screen.Navigator = this;
                screen.NavigationParameter = parameter;
                screen.Key = pageKey;
                Configuration.Decorate(screen);
            }

            return viewController as UIViewController;
        }

        public void Configure(int key, Type activityType)
        {
            lock (_screenSpecs)
            {
                if (ContainsKey(key))
                {
                    _screenSpecs[key] = activityType;
                }
                else
                {
                    _screenSpecs.Add(key, activityType);
                }
            }
        }

        public void Present(int pageKey, Action onDismissed = null)
        {
            Present(pageKey, null, onDismissed);
        }

        public void Present(int pageKey, object parameter, Action onDismissed = null)
        {
            System.Diagnostics.Debug.WriteLine($"Present from screen: {pageKey}, To Screen: {CurrentPageKey}");

            if (CurrentPageKey == pageKey && (parameter == null || parameter.Equals(CurrentScreen?.NavigationParameter)))
            {
                return;
            }

            var toViewController = GetScreen(pageKey, parameter);
            if (toViewController == null)
            {
                throw new InvalidOperationException("Can't Init ViewController");
            }

            //var navigator = new Navigator();
            //navigator.OnDismissViewController = onDismissed;
            //navigator.NavigateTo(pageKey, parameter);

            var fromScreen = CurrentScreen;
            this.OnDismissViewController = onDismissed;
            NavigationController.PresentViewControllerAsync(toViewController, true);
        }

        public void Dismiss()
        {
            NavigationController.DismissViewController(true, OnDismissViewController);
        }

        public override void WillShowViewController(UINavigationController navigationController, UIViewController viewController, bool animated)
        {
            InvokeMethod(viewController, "ViewControllerWillShow", new object[] { });
        }

        public override void DidShowViewController(UINavigationController navigationController, UIViewController viewController, bool animated)
        {
            InvokeMethod(viewController, "ViewControllerDidShow", new object[] { });
        }

        public void NavigateExternal(string url)
        {
            url = url.Replace(" ", "");
            var nsUrl = new NSUrl(url);
            UIApplication.SharedApplication.OpenUrl(nsUrl);
        }

        private void InvokeMethod(object obj, string name, object[] param)
        {
            var type = obj.GetType();
            var method = type.GetMethod(name);
            method.Invoke(obj, param);
        }
    }
}

