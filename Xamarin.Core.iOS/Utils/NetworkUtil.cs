namespace Xamarin.Core.iOS
{
	public class NetworkUtil : INetworkDetector
	{
		public bool IsWifiConnected
		{
			get
			{
				return Reachability.InternetConnectionStatus() == NetworkStatus.ReachableViaWiFiNetwork;
			}
		}
	}
}
