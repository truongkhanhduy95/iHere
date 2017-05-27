using System;
namespace Xamarin.Core
{
	public class SearchResultItemClickEventArgs
	{
		public int Position { get; private set; }

		public ISearchResultItem ResultItem { get; private set; }

		public SearchResultItemClickEventArgs(ISearchResultItem item, int position)
		{
			ResultItem = item;
			Position = position;
		}
	}
}
