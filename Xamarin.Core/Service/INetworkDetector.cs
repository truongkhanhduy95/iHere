using System;
namespace Xamarin.Core
{
	public interface INetworkDetector
	{
		bool IsWifiConnected { get; }
	}
}
