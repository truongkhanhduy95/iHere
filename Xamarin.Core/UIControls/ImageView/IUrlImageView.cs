using System;
namespace Xamarin.Core
{
	public interface IUrlImageView : IImageView
	{
		void SetImageUrl(string url);

		void SetImageUrl(string url, int width, int height);
	}
}
