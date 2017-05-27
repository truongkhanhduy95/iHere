using System;
using CoreGraphics;
using Foundation;
using GalaSoft.MvvmLight.Ioc;
using UIKit;
using Xamarin.Core.Interfaces.Views;

namespace Xamarin.Core.iOS.Views
{
    [Register("LoadingOverLayView")]
    public class LoadingOverlayView : UIView, ILoading
    {
        private UIActivityIndicatorView _activitySpinner;
        private UILabel titleLabel;
        protected UIView _containerView;

        public string Title
        {
            get { return titleLabel?.Text; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    titleLabel.Hidden = true;
                }
                else
                {
                    titleLabel.Hidden = false;
                    titleLabel.Text = value;
                }
            }
        }

        [PreferredConstructor]
        public LoadingOverlayView()
            : base(UIScreen.MainScreen.Bounds)
        {
            Initialize();
        }

        public LoadingOverlayView(UIView containerView)
            : base(containerView.Bounds)
        {
            _containerView = containerView;
            Initialize();
        }

        public LoadingOverlayView(IntPtr handle)
            : base(handle)
        {
            Initialize();
        }

        public void Hide(Action onCompleted = null)
        {
            UIView.Animate(
                        0.5,
                        () => Alpha = 0,
                        () =>
                        {
                            RemoveFromSuperview();
                            onCompleted?.Invoke();
                            //Alpha = 0.75f;
                        });
        }

        public void Show(Action onCompleted = null)
        {
            Alpha = 0.75f;
            if (_containerView == null)
            {

                UIApplication.SharedApplication.Delegate.GetWindow().RootViewController.View.Add(this);

                //UIApplication.SharedApplication.KeyWindow.AddSubview (this);
                //UIApplication.SharedApplication.KeyWindow.BringSubviewToFront (this);

                //var parentView = UIApplication.SharedApplication.Delegate.GetWindow ().RootViewController;

                //var childController = (parentView as UINavigationController).TopViewController.ChildViewControllers [0];
                //if (childController != null && childController is UITabBarController) {
                //	((childController as UITabBarController).SelectedViewController as UINavigationController).TopViewController.View.AddSubview (this);
                //	((childController as UITabBarController).SelectedViewController as UINavigationController).TopViewController.View.BringSubviewToFront (this);
                //} else {
                //	parentView.View.AddSubview (this);
                //}
            }
            else
            {
                this.Frame = new CGRect(0, 0, _containerView.Frame.Width, _containerView.Frame.Height);
                _activitySpinner.Frame = this.Frame;
                if (this.Superview == null)
                {
                    _containerView.Add(this);
                }
                else if (_containerView != this.Superview)
                {
                    this.Hide();

                    _containerView.Add(this);
                }

                //this.Center = _containerView.Center;

            }
        }

        private void Initialize()
        {
            BackgroundColor = UIColor.Black;
            Alpha = 0.75f;
            AutoresizingMask = UIViewAutoresizing.All;

            nfloat centerX = Frame.Width / 2;
            nfloat centerY = Frame.Height / 2;

            nfloat labelHeight = 22;
            nfloat labelWidth = Frame.Width - 20;

            // create the activity spinner, center it horizontall and put it 5 points above center x
            _activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
            _activitySpinner.Frame = new CGRect(
                centerX - (_activitySpinner.Frame.Width / 2),
                centerY - _activitySpinner.Frame.Height - 20,
                _activitySpinner.Frame.Width,
                _activitySpinner.Frame.Height);
            _activitySpinner.AutoresizingMask = UIViewAutoresizing.All;
            AddSubview(_activitySpinner);
            _activitySpinner.StartAnimating();


            // create and configure label
            titleLabel = new UILabel(new CGRect(
                centerX - (labelWidth / 2),
                centerY + 20,
                labelWidth,
                labelHeight
                ));
            titleLabel.BackgroundColor = UIColor.Clear;
            titleLabel.TextColor = UIColor.White;
            titleLabel.TextAlignment = UITextAlignment.Center;
            titleLabel.AutoresizingMask = UIViewAutoresizing.All;
            AddSubview(titleLabel);
        }
    }
}

