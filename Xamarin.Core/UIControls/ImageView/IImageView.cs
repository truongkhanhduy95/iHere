using System;
namespace Xamarin.Core
{
	public interface IImageView : IControl
	{
		bool HasImage { get; }

		void SetImage(string imagePath);
	}
}
