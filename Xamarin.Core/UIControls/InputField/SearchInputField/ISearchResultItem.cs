namespace Xamarin.Core
{
	public interface ISearchResultItem
	{
		int SearchResultId { get; }

		string SearchResultText { get; }

		string SearchResultKey { get; }
	}
}
