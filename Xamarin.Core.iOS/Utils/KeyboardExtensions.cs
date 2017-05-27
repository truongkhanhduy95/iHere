using System;
using UIKit;

namespace Xamarin.Core.iOS
{
    public static class KeyboardExtensions
    {
        public static void AddHideButtonToKeyboard(this UITextField textField, UIKeyboardType keyboardType)
        {
            AddHideButtonToKeyboard(textField);
        }

        public static void AddHideButtonToKeyboard(this UITextView textView, UIKeyboardType keyboardType = UIKeyboardType.Default, string nameHideButton = "Done")
        {
            if (textView.InputAccessoryView != null)
            {
                return;
            }

            UIToolbar toolbar = new UIToolbar();
            textView.KeyboardType = keyboardType;
            toolbar.BarStyle = UIBarStyle.Black;
            toolbar.Translucent = true;
            toolbar.SizeToFit();
            UIBarButtonItem doneButton = new UIBarButtonItem(nameHideButton, UIBarButtonItemStyle.Done,
                                             (s, e) => textView.ResignFirstResponder());
            toolbar.SetItems(new UIBarButtonItem[] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton }, true);

            textView.InputAccessoryView = toolbar;
            textView.AutocorrectionType = UITextAutocorrectionType.No;
        }

        public static void AddHideButtonToKeyboard(this UITextField textField, string nameHideButton = "Done")
        {
            if (textField.InputAccessoryView != null)
            {
                return;
            }

            UIToolbar toolbar = new UIToolbar();
            toolbar.BarStyle = UIBarStyle.Black;
            toolbar.Translucent = true;
            toolbar.SizeToFit();
            UIBarButtonItem doneButton = new UIBarButtonItem(nameHideButton, UIBarButtonItemStyle.Done,
                                             (s, e) => textField.ResignFirstResponder());
            toolbar.SetItems(new UIBarButtonItem[] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton }, true);

            textField.InputAccessoryView = toolbar;
            textField.AutocorrectionType = UITextAutocorrectionType.No;
        }

        public static void AddHideButtonToKeyboard(this UITextField txt, UIKeyboardType keyboardType, Action doneAction, string buttonText = "Done")
        {
            UIToolbar toolbar = new UIToolbar();
            txt.KeyboardType = keyboardType;
            toolbar.BarStyle = UIBarStyle.Black;
            toolbar.Translucent = true;
            toolbar.SizeToFit();
            UIBarButtonItem doneButton = new UIBarButtonItem(buttonText, UIBarButtonItemStyle.Done,
                                             (s, e) =>
                                             {
                                                 txt.ResignFirstResponder();
                                                 if (doneAction != null)
                                                 {
                                                     doneAction();
                                                 }
                                             });
            toolbar.SetItems(new UIBarButtonItem[] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton }, true);

            txt.InputAccessoryView = toolbar;
            txt.AutocorrectionType = UITextAutocorrectionType.No;
        }

        public static UIView GetViewResponder(UIView viewGroup)
        {
            UIView a = null;
            if (viewGroup.Subviews != null && viewGroup.Subviews.Length > 0)
            {
                foreach (UIView view in viewGroup.Subviews)
                {
                    if (view.IsFirstResponder)
                    {
                        a = view;
                        break;
                    }
                    else
                    {
                        UIView v = GetViewResponder(view);
                        a = v;
                        if (a != null)
                        {
                            break;
                        }
                    }
                }
            }
            return a;
        }

        public static void AddHideButtonToKeyboard(this UIView txt, string nameHideButton = "Done")
        {
            if (txt != null)
            {
                if (txt is UITextView)
                {
                    AddHideButtonToKeyboard(txt as UITextView);
                }
                else if (txt is UITextField)
                {
                    AddHideButtonToKeyboard(txt as UITextField, UIKeyboardType.Default);
                }
            }

        }

    }
}

