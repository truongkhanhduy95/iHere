namespace Xamarin.Core
{
	public interface IScreen : ICanNavigate
	{
		void Log(string message);

		int Key { get; set; }
		bool IsRoot { get; set; }
		bool NoCache { get; set; }
		bool IsNavigateBack { get; set; }
		object NavigationParameter { get; set; }
		AnimationType AnimationType { get; set; }

		#region Icontrol 
		IControl GetSharedControlInterface(string tag);
		#endregion
	}
}

