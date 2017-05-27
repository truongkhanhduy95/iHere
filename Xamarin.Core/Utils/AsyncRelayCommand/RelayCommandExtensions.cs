using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using Xamarin.Core.Interfaces.Views;
using Xamarin.Core.Interfaces.Service;
using Xamarin.Core.ViewModels;

namespace Xamarin.Core.Utils
{
    public static class RelayCommandExtensions
    {
        private static IDialogServiceEx _dialogService;

        private static IDialogServiceEx DialogService
        {
            get
            {
                return _dialogService ?? (_dialogService = ServiceLocator.Current.GetInstance<IDialogServiceEx>());
            }
        }

		private static Exception GetInnerException(Exception ex) {
			if (ex is AggregateException
			   || ex is TargetInvocationException)
			{
				return GetInnerException(ex.InnerException);
			}
			else 
			{
				return ex;
			}
		}

        private static void HandleError(AggregateException exception, Action<Exception> onError, bool defaultErrorHandlerEnabled)
        {
            DialogService.HideLoading();

			var innerException = GetInnerException(exception);
            if (innerException != null)
            {
                if (onError != null)
                {
                    onError.Invoke(innerException);
                }

                if (defaultErrorHandlerEnabled)
                {
                    var serviceErrorHandler = ServiceLocator.Current.GetInstance<IServiceErrorHandler>();
                    serviceErrorHandler?.OnServiceFailed(innerException);
                }
            }
        }

        private static void Execute(this Task task)
        {
            Task.Run(() =>
            {
                try
                {
                    task.Wait();
                }
                catch (TargetInvocationException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
                catch (AggregateException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            });
        }

        #region RelayCommand
        public static void ExecuteAsync(this RelayCommand command, Action onCompleted = null, Action<Exception> onError = null, bool defaultErrorHandlerEnabled = true)
        {
            var task = Task.Run(() => command.Execute(null));
            task.ContinueWith((arg) =>
            {
                onCompleted?.Invoke();
            }, CancellationToken.None, TaskContinuationOptions.NotOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            task.ContinueWith((arg) =>
            {
                HandleError(arg.Exception, onError, defaultErrorHandlerEnabled);
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());

            task.Execute();
        }

        public static void ExecuteAsync(this RelayCommand command, IBusy blockElement, Action onCompleted = null, Action<Exception> onError = null, bool defaultErrorHandlerEnabled = true)
        {
            blockElement.IsBusy = true;
            var task = Task.Run(() => command.Execute(null));
            task.ContinueWith((arg) =>
            {
                blockElement.IsBusy = false;
                onCompleted?.Invoke();
            }, CancellationToken.None, TaskContinuationOptions.NotOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            task.ContinueWith((arg) =>
            {
                blockElement.IsBusy = false;
                HandleError(arg.Exception, onError, defaultErrorHandlerEnabled);
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());

            task.Execute();
        }

        #endregion

        #region RelayCommand<T>

        public static void ExecuteAsync<T>(this RelayCommand<T> command, object param, Action onCompleted = null, Action<Exception> onError = null, bool defaultErrorHandlerEnabled = true)
        {
            var task = Task.Run(() => command.Execute(param));
            task.ContinueWith((arg) =>
            {
                onCompleted?.Invoke();
            }, CancellationToken.None, TaskContinuationOptions.NotOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            task.ContinueWith((arg) =>
            {
                HandleError(arg.Exception, onError, defaultErrorHandlerEnabled);
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());

            task.Execute();
        }

        public static void ExecuteAsync<T>(this RelayCommand<T> command, object param, IBusy blockElement, Action onCompleted = null, Action<Exception> onError = null, bool defaultErrorHandlerEnabled = true)
        {
            blockElement.IsBusy = true;
            var task = Task.Run(() => command.Execute(param));
            task.ContinueWith((arg) =>
            {
                blockElement.IsBusy = false;
                onCompleted?.Invoke();
            }, CancellationToken.None, TaskContinuationOptions.NotOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            task.ContinueWith((arg) =>
            {
                blockElement.IsBusy = false;
                HandleError(arg.Exception, onError, defaultErrorHandlerEnabled);
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());

            task.Execute();
        }

        #endregion

        #region RelayCommandEx<TResult>
        public static void ExecuteAsync<TResult>(this RelayCommandEx<TResult> command, Action<TResult> onCompleted = null, Action<Exception> onError = null, bool defaultErrorHandlerEnabled = true)
        {
            var task = Task.Run(() => command.Execute(null));
            task.ContinueWith((arg) =>
            {
                onCompleted?.Invoke(arg.Result);
            }, CancellationToken.None, TaskContinuationOptions.NotOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            task.ContinueWith((arg) =>
            {
                HandleError(arg.Exception, onError, defaultErrorHandlerEnabled);
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());

            task.Execute();
        }

        public static void ExecuteAsync<TResult>(this RelayCommandEx<TResult> command, IBusy blockElement, Action<TResult> onCompleted = null, Action<Exception> onError = null, bool defaultErrorHandlerEnabled = true)
        {
            blockElement.IsBusy = true;
            var task = Task.Run(() => command.Execute(null));
            task.ContinueWith((arg) =>
            {
                blockElement.IsBusy = false;
                onCompleted?.Invoke(arg.Result);
            }, CancellationToken.None, TaskContinuationOptions.NotOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            task.ContinueWith((arg) =>
            {
                blockElement.IsBusy = false;
                HandleError(arg.Exception, onError, defaultErrorHandlerEnabled);
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());

            task.Execute();
        }

        #endregion

        #region RelayCommand<TParam, TResult>

        public static void ExecuteAsync<TParam, TResult>(this RelayCommand<TParam, TResult> command, TParam param, Action<TResult> onCompleted = null, Action<Exception> onError = null, bool defaultErrorHandlerEnabled = true)
        {
            var task = Task.Run(() => command.Execute(param));
            task.ContinueWith((arg) =>
            {
                onCompleted?.Invoke(arg.Result);
            }, CancellationToken.None, TaskContinuationOptions.NotOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            task.ContinueWith((arg) =>
            {
                HandleError(arg.Exception, onError, defaultErrorHandlerEnabled);
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());

            task.Execute();
        }

        public static void ExecuteAsync<TParam, TResult>(this RelayCommand<TParam, TResult> command, TParam param, IBusy blockElement, Action<TResult> onCompleted = null, Action<Exception> onError = null, bool defaultErrorHandlerEnabled = true)
        {
            blockElement.IsBusy = true;
            var task = Task.Run(() => command.Execute(param));
            task.ContinueWith((arg) =>
            {
                blockElement.IsBusy = false;
                onCompleted?.Invoke(arg.Result);
            }, CancellationToken.None, TaskContinuationOptions.NotOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            task.ContinueWith((arg) =>
            {
                blockElement.IsBusy = false;
                HandleError(arg.Exception, onError, defaultErrorHandlerEnabled);
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());

            task.Execute();
        }

        #endregion
    }
}