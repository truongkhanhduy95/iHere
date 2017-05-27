using System;
using System.Net.Http;

namespace Xamarin.Core
{
	public class ServiceResponseEventArgs<TResponseObject> : EventArgs where TResponseObject : BaseResponseObject
	{
		public string Tag { get; private set; }

		public string Url { get; set; }

		public string ResponseString { get; set; }

		public HttpResponseMessage ResponseMessage { get; set; }

		public TResponseObject ResponseObject { get; set; }

		public ServiceResponseEventArgs(string tag)
		{
			Tag = tag;
		}
	}
}
