using System;
using System.Collections.Generic;

namespace Xamarin.Core
{
	public interface IDataSource<TData>
	{
		List<TData> DataSource { get; set; }

		event Action<IDataItemView<TData>> ItemLoad;

		event EventHandler<ItemClickEventArgs<TData>> ItemClicked;

		event EventHandler<ItemClickEventArgs<TData>> ItemControlClicked;
	}
}
