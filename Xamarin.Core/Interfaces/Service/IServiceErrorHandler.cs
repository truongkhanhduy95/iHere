using System;

namespace Xamarin.Core.Interfaces.Service
{
    public interface IServiceErrorHandler
    {
        void OnServiceFailed(Exception exception);
    }
}
