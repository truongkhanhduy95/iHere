namespace Xamarin.Core
{
	public interface IDataItemView<TData>
	{
		TData ItemData { get; }

		int ItemPosition { get; }

		IControl GetSharedControlInterface(string tag);
	}
}
