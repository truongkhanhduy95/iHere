using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Helpers;

namespace Xamarin.Core.iOS
{
    public static class BindingExtensions
    {
        public static void AttachTo(this Binding binding, List<Binding> bindings)
        {
            bindings?.Add(binding);
        }
    }
}