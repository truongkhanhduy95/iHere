using Android.Content;
using Android.Net;

namespace Xamarin.Core.Droid
{
	public class NetworkUtil : INetworkDetector
	{
		private readonly ConnectivityManager _connectivityManager;

		public NetworkUtil(Context context)
		{
			_connectivityManager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
		}

		public bool IsWifiConnected
		{
			get
			{
				return _connectivityManager.ActiveNetworkInfo != null && _connectivityManager.ActiveNetworkInfo.Type == ConnectivityType.Wifi && _connectivityManager.ActiveNetworkInfo.IsConnected;
			}
		}
	}
}
