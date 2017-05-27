using System;

namespace Xamarin.Core
{
	public interface IButton : IControl
	{
		string Text { get; set; }

		event EventHandler OnClick;
	}
}
