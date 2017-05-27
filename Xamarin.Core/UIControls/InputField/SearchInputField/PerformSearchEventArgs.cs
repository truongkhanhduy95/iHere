namespace Xamarin.Core
{
	public class PerformSearchEventArgs
	{
		public string SearchText { get; private set; }

		public PerformSearchEventArgs(string searchText)
		{
			SearchText = searchText;
		}
	}
}
