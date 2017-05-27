using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Xamarin.Core.iOS
{
    public class KeyboardHandler
    {
        private const float OFFSET = 5f;
        public const float KEYBOARD_OFFSET = 260f;

        private NSObject _keyboardUp, _keyboardDown, _keyboardDidShow, _keyboardDidHide;
        private UIView _activeView;
        private nfloat _scrollAmount = 0;
        private bool _isKeyboardHidden = true;

        private CGRect _originFrame;
        private bool _isFirstTime = true;

        public UIView View { get; set; }

        public KeyboardHandler(UIView view)
        {
            View = view;
        }

        public virtual void RegisterKeyboardObserver()
        {
            if (_keyboardUp == null && _keyboardDown == null)
            {
                _keyboardUp = NSNotificationCenter
                    .DefaultCenter
                    .AddObserver(UIKeyboard.WillShowNotification, KeyboardUpNotification);
                _keyboardDown = NSNotificationCenter
                    .DefaultCenter
                    .AddObserver(UIKeyboard.WillHideNotification, KeyboardDownNotification);
                _keyboardDidShow = NSNotificationCenter
                    .DefaultCenter
                    .AddObserver(UIKeyboard.DidShowNotification, KeyboardDidShowNotification);
                _keyboardDidHide = NSNotificationCenter
                    .DefaultCenter
                    .AddObserver(UIKeyboard.DidHideNotification, KeyboardDidHideNotification);
            }
        }

        public void RemoveKeyboardObserver()
        {
            if (_keyboardUp != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardUp);
                _keyboardUp.Dispose();
                _keyboardUp = null;
            }

            if (_keyboardDown != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardDown);
                _keyboardDown.Dispose();
                _keyboardDown = null;
            }

            if (_keyboardDidShow != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardDidShow);
                _keyboardDidShow.Dispose();
                _keyboardDown = null;
            }

            if (_keyboardDidHide != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardDidHide);
                _keyboardDidHide.Dispose();
                _keyboardDidHide = null;
            }
        }

        private void KeyboardUpNotification(NSNotification notification)
        {
            if (_isFirstTime)
            {
                _originFrame = View.Frame;
                _isFirstTime = false;
            }

            var keyboardFrame = UIKeyboard.BoundsFromNotification(notification);
            _activeView = KeyboardExtensions.GetViewResponder(View);
            //_activeView.AddHideButtonToKeyboard();

            //if (_activeView == null)
            //{
            //    _activeView = View;
            //}

            if (_isKeyboardHidden && _activeView != null)
            {
                nfloat bottom = 0;
                var point = _activeView.Superview.ConvertPointToView(_activeView.Frame.Location, _activeView.Window);
                if (_activeView.Window == null)
                {
                    bottom = _activeView.Frame.Height - point.Y - _activeView.Frame.Height - OFFSET;
                }
                else
                {
                    bottom = _activeView.Window.Frame.Height - point.Y - _activeView.Frame.Height - OFFSET;
                }

                _scrollAmount = (float)(keyboardFrame.Height - bottom);

                if (_scrollAmount > 0)
                {
                    ScrollTheView(false);
                }
            }

            _isKeyboardHidden = false;
        }

        private void KeyboardDownNotification(NSNotification notification)
        {
            if (!_isKeyboardHidden)
            {
                ScrollTheView(true);
            }
            _isKeyboardHidden = true;
        }

        private void KeyboardDidShowNotification(NSNotification notification)
        {
            _isKeyboardHidden = false;
        }

        private void KeyboardDidHideNotification(NSNotification notification)
        {
            _isKeyboardHidden = true;
        }

        private void ScrollTheView(bool reset)
        {
            // scroll the view up or down
            UIView.BeginAnimations(string.Empty, System.IntPtr.Zero);
            UIView.SetAnimationDuration(0.3);

            var frame = _originFrame;

            if (reset)
            {
                frame.Y = _originFrame.Y;
                _scrollAmount = 0;
            }
            else
            {
                frame.Y -= _scrollAmount;
            }
            View.Frame = frame;

            UIView.CommitAnimations();

            CorrectScrollViewContentSize(reset);

        }

        private void CorrectScrollViewContentSize(bool reset)
        {
            UIScrollView scrollView = null;
            nfloat lowestY = 0;

            foreach (UIView subView in View.Subviews)
            {
                if (subView is UIScrollView)
                {
                    scrollView = subView as UIScrollView;
                    foreach (UIView subViewSV in scrollView.Subviews)
                    {
                        if (subViewSV.Frame.Y > lowestY && subViewSV is UIButton)
                        {
                            lowestY = subViewSV.Frame.Y;
                        }
                    }
                }
            }

            if (scrollView != null && lowestY > 0)
            {
                var contentSize = scrollView.ContentSize;
                if (!reset)
                {
                    contentSize.Height = (lowestY < scrollView.Frame.Height - KEYBOARD_OFFSET) ? contentSize.Height : scrollView.Frame.Height + KEYBOARD_OFFSET / 2;
                }
                else
                {
                    contentSize.Height = scrollView.Frame.Height;
                }

                scrollView.ContentSize = contentSize;
            }
        }
    }
}

