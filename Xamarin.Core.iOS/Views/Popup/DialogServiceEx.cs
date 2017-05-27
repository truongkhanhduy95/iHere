using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using UIKit;
using Xamarin.Core.Interfaces.Views;

namespace Xamarin.Core.iOS.Views
{
    public class DialogServiceEx : IDialogServiceEx
    {
        private int _numOfLoading = 0;
        private ILoading _loadingView;

        private ILoading LoadingView
        {
            get
            {
                return _loadingView ?? (_loadingView = ServiceLocator.Current.GetInstance<ILoading>());
            }
        }

        public void HideLoading(Action onCompleted = null)
        {
            lock (LoadingView)
            {
                if (_numOfLoading == 1)
                {
                    LoadingView.Hide(onCompleted);
                }
                else
                {
                    onCompleted?.Invoke();
                }
                if (_numOfLoading > 0)
                {
                    _numOfLoading--;
                }
            }
        }

        public void ShowLoading(string title = null, Action onCompleted = null)
        {
            lock (LoadingView)
            {
                if (_numOfLoading < 1)
                {
                    LoadingView.Title = title;
                    LoadingView.Show(onCompleted);
                }
                else
                {
                    onCompleted?.Invoke();
                }
                _numOfLoading++;
            }
        }

        /// <summary>
        /// Displays information about an error.
        /// </summary>
        /// <param name="message">The message to be shown to the user.</param>
        /// <param name="title">The title of the dialog box. This may be null.</param>
        /// <param name="buttonText">The text shown in the only button
        /// in the dialog box. If left null, the text "OK" will be used.</param>
        /// <param name="afterHideCallback">A callback that should be executed after
        /// the dialog box is closed by the user.</param>
        /// <returns>A Task allowing this async method to be awaited.</returns>
        /// <remarks>Displaying dialogs in iOS is synchronous. As such,
        /// this method will be executed synchronously even though it can be awaited
        /// for cross-platform compatibility purposes.</remarks>
        public Task ShowError(string message, string title, string buttonText, Action afterHideCallback)
        {
            var tcs = new TaskCompletionSource<bool>();

            var av = new UIAlertView(
                title,
                message,
                null,
                buttonText,
                null);

            av.Dismissed += (s, e) =>
            {
                if (afterHideCallback != null)
                {
                    afterHideCallback();
                }

                tcs.SetResult(true);
            };

            av.Show();
            return tcs.Task;
        }

        /// <summary>
        /// Displays information about an error.
        /// </summary>
        /// <param name="error">The exception of which the message must be shown to the user.</param>
        /// <param name="title">The title of the dialog box. This may be null.</param>
        /// <param name="buttonText">The text shown in the only button
        /// in the dialog box. If left null, the text "OK" will be used.</param>
        /// <param name="afterHideCallback">A callback that should be executed after
        /// the dialog box is closed by the user.</param>
        /// <returns>A Task allowing this async method to be awaited.</returns>
        /// <remarks>Displaying dialogs in iOS is synchronous. As such,
        /// this method will be executed synchronously even though it can be awaited
        /// for cross-platform compatibility purposes.</remarks>
        public Task ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
            var tcs = new TaskCompletionSource<bool>();

            var av = new UIAlertView(
                title,
                error.Message,
                null,
                buttonText,
                null);

            av.Dismissed += (s, e) =>
            {
                if (afterHideCallback != null)
                {
                    afterHideCallback();
                }

                tcs.SetResult(true);
            };

            av.Show();
            return tcs.Task;
        }

        /// <summary>
        /// Displays information to the user. The dialog box will have only
        /// one button with the text "OK".
        /// </summary>
        /// <param name="message">The message to be shown to the user.</param>
        /// <param name="title">The title of the dialog box. This may be null.</param>
        /// <returns>A Task allowing this async method to be awaited.</returns>
        /// <remarks>Displaying dialogs in Android is synchronous. As such,
        /// this method will be executed synchronously even though it can be awaited
        /// for cross-platform compatibility purposes.</remarks>
        public Task ShowMessage(string message, string title)
        {
            var tcs = new TaskCompletionSource<bool>();

            var av = new UIAlertView(
                title,
                message,
                null,
                "OK",
                null);

            av.Dismissed += (s, e) => tcs.SetResult(true);
            av.Show();
            return tcs.Task;
        }

        /// <summary>
        /// Displays information to the user. The dialog box will have only
        /// one button.
        /// </summary>
        /// <param name="message">The message to be shown to the user.</param>
        /// <param name="title">The title of the dialog box. This may be null.</param>
        /// <param name="buttonText">The text shown in the only button
        /// in the dialog box. If left null, the text "OK" will be used.</param>
        /// <param name="afterHideCallback">A callback that should be executed after
        /// the dialog box is closed by the user.</param>
        /// <returns>A Task allowing this async method to be awaited.</returns>
        /// <remarks>Displaying dialogs in Android is synchronous. As such,
        /// this method will be executed synchronously even though it can be awaited
        /// for cross-platform compatibility purposes.</remarks>
        public Task ShowMessage(
            string message,
            string title,
            string buttonText,
            Action afterHideCallback)
        {
            var tcs = new TaskCompletionSource<bool>();

            var av = new UIAlertView(
                title,
                message,
                null,
                buttonText,
                null);

            av.Dismissed += (s, e) =>
            {
                if (afterHideCallback != null)
                {
                    afterHideCallback();
                }

                tcs.SetResult(true);
            };

            av.Show();
            return tcs.Task;
        }

        /// <summary>
        /// Displays information to the user. The dialog box will have only
        /// one button.
        /// </summary>
        /// <param name="message">The message to be shown to the user.</param>
        /// <param name="title">The title of the dialog box. This may be null.</param>
        /// <param name="buttonConfirmText">The text shown in the "confirm" button
        /// in the dialog box. If left null, the text "OK" will be used.</param>
        /// <param name="buttonCancelText">The text shown in the "cancel" button
        /// in the dialog box. If left null, the text "Cancel" will be used.</param>
        /// <param name="afterHideCallback">A callback that should be executed after
        /// the dialog box is closed by the user. The callback method will get a boolean
        /// parameter indicating if the "confirm" button (true) or the "cancel" button
        /// (false) was pressed by the user.</param>
        /// <returns>A Task allowing this async method to be awaited. The task will return
        /// true or false depending on the dialog result.</returns>
        /// <remarks>Displaying dialogs in Android is synchronous. As such,
        /// this method will be executed synchronously even though it can be awaited
        /// for cross-platform compatibility purposes.</remarks>
        public Task<bool> ShowMessage(
            string message,
            string title,
            string buttonConfirmText,
            string buttonCancelText,
            Action<bool> afterHideCallback)
        {
            var tcs = new TaskCompletionSource<bool>();

            var av = new UIAlertView(
                title,
                message,
                null,
                buttonCancelText,
                buttonConfirmText);

            av.Dismissed += (s, e) =>
            {
                if (afterHideCallback != null)
                {
                    afterHideCallback(e.ButtonIndex > 0);
                }

                tcs.SetResult(e.ButtonIndex > 0);
            };

            av.Show();
            return tcs.Task;
        }

        /// <summary>
        /// Displays information to the user in a simple dialog box. The dialog box will have only
        /// one button with the text "OK". This method should be used for debugging purposes.
        /// </summary>
        /// <param name="message">The message to be shown to the user.</param>
        /// <param name="title">The title of the dialog box. This may be null.</param>
        /// <returns>A Task allowing this async method to be awaited.</returns>
        /// <remarks>Displaying dialogs in Android is synchronous. As such,
        /// this method will be executed synchronously even though it can be awaited
        /// for cross-platform compatibility purposes.</remarks>
        public Task ShowMessageBox(string message, string title)
        {
            var tcs = new TaskCompletionSource<bool>();

            var av = new UIAlertView(
                title,
                message,
                null,
                "OK",
                null);

            av.Dismissed += (s, e) => tcs.SetResult(true);
            av.Show();
            return tcs.Task;
        }
    }
}