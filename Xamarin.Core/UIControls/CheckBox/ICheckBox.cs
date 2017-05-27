namespace Xamarin.Core
{
	public interface ICheckBox : IControl
	{
		string Text { get; set; }

		bool IsChecked { get; set; }
	}
}
