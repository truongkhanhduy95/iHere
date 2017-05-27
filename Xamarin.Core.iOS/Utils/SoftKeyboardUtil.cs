using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Xamarin.Core.iOS
{
	public class SoftKeyboardUtil
	{
		private UIView _containerView;
		private CGRect _keyboardFrame;
		private nfloat _height;

		public void RegisterNotification(UIView containerView)
		{
			_containerView = containerView;
			_height = _containerView.Frame.Height;

			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardHidden);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardShowed);
		}

		public void UnregisterNotification()
		{
			if (_containerView != null)
			{
				_containerView.EndEditing(true);
				_containerView = null;

				NSNotificationCenter.DefaultCenter.RemoveObserver(UIKeyboard.WillHideNotification);
				NSNotificationCenter.DefaultCenter.RemoveObserver(UIKeyboard.WillShowNotification);
			}
		}

		public static void AddDoneButton(UITextField textField, EventHandler handler)
		{
			UIToolbar keyboardToolbar = new UIToolbar();
			keyboardToolbar.SizeToFit();
			UIBarButtonItem flexBarButton = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace, null);
			UIBarButtonItem doneBarButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, handler);
			keyboardToolbar.Items = new[] { flexBarButton, doneBarButton };
			textField.InputAccessoryView = keyboardToolbar;
		}

		public static void AddDoneButton(UITextView textView, EventHandler handler)
		{
			UIToolbar keyboardToolbar = new UIToolbar();
			keyboardToolbar.SizeToFit();
			UIBarButtonItem flexBarButton = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace, null);
			UIBarButtonItem doneBarButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, handler);

			keyboardToolbar.Items = new[] { flexBarButton, doneBarButton };
			textView.InputAccessoryView = keyboardToolbar;
		}

		private void OnKeyboardShowed(NSNotification notification)
		{
			if (_containerView != null)
			{
				var keyboardFrame = UIKeyboard.FrameEndFromNotification(notification);
				if (_keyboardFrame.Height != keyboardFrame.Height)
				{
					_keyboardFrame = keyboardFrame;

					UIView.BeginAnimations("AnimateForKeyboard");
					UIView.SetAnimationBeginsFromCurrentState(true);
					UIView.SetAnimationDuration(UIKeyboard.AnimationDurationFromNotification(notification));
					UIView.SetAnimationCurve((UIViewAnimationCurve)UIKeyboard.AnimationCurveFromNotification(notification));

					var frame = _containerView.Frame;
					frame.Height = _height - _keyboardFrame.Height;

					_containerView.Frame = frame;
					UIView.CommitAnimations();
				}
			}
		}

		private void OnKeyboardHidden(NSNotification notification)
		{
			if (_containerView != null)
			{
				_keyboardFrame = CGRect.Empty;

				UIView.BeginAnimations("AnimateForKeyboard");
				UIView.SetAnimationBeginsFromCurrentState(true);
				UIView.SetAnimationDuration(UIKeyboard.AnimationDurationFromNotification(notification));
				UIView.SetAnimationCurve((UIViewAnimationCurve)UIKeyboard.AnimationCurveFromNotification(notification));

				var frame = _containerView.Frame;
				frame.Height = _height;

				_containerView.Frame = frame;
				UIView.CommitAnimations();
			}
		}
	}
}
