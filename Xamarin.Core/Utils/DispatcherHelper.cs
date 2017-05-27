using System;
using System.Threading.Tasks;

namespace Xamarin.Core.Utils
{
    public abstract class DispatcherHelper
    {
        public abstract void RunOnUIThread(Action action);

        public async Task InvokeAsync(Action action, Action onCompleted = null)
        {
            try
            {
                await Task.Run(action);
            }
            catch (Exception ex)
            {
				System.Diagnostics.Debug.WriteLine($"Uncaught exception >>> {ex}");
            }
            finally
            {
                onCompleted?.Invoke();
            }
        }
    }
}

