using System;
namespace Xamarin.Core
{
	public class ItemClickEventArgs<TItemData> : EventArgs
	{
		public int Position { get; set; }

		public TItemData Data { get; set; }
	}
}
