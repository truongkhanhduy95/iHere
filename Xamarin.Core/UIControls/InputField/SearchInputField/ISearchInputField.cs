using System;
using System.Collections.Generic;

namespace Xamarin.Core
{
	public interface ISearchInputField : IInputField
	{
		List<ISearchResultItem> SearchResultList { get; }

		event EventHandler<PerformSearchEventArgs> PerformSearch;

		event EventHandler OnCancelSearch;

		event EventHandler<SearchResultItemClickEventArgs> ItemClick;

		void SetSearchResult(List<ISearchResultItem> results);
	}
}
