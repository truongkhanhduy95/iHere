using System;
namespace Xamarin.Core
{
   public interface IControl
    {
        bool Enabled { get; set; }

        bool IsVisible { get; set; }
    }
}

