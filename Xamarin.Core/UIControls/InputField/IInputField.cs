namespace Xamarin.Core
{
	public interface IInputField : IControl
	{
		string Input { get; set; }

		string Hint { get; set; }

		InputType InputFieldType { set; }
	}
}
