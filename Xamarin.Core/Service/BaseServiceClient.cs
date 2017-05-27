using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ModernHttpClient;
using Newtonsoft.Json;

namespace Xamarin.Core
{
	public class BaseServiceClient<TResponseObject> where TResponseObject : BaseResponseObject
	{
		protected bool IsDebugLog { get; set; }

		protected HttpClient HttpClient { get; private set; }

		public INetworkDetector NetworkDetector { get; set; }

		public event EventHandler<ServiceResponseEventArgs<TResponseObject>> OnResponseSuccess;
		public event EventHandler<ServiceResponseEventArgs<TResponseObject>> OnResponseFailed;
		public event EventHandler<ServiceResponseEventArgs<TResponseObject>> OnParseError;
		public event EventHandler<ServiceResponseEventArgs<TResponseObject>> OnNetworkConnectionFailed;

		public BaseServiceClient()
		{
			HttpClient = new HttpClient(new NativeMessageHandler());
			IsDebugLog = false;
		}

		public BaseServiceClient(HttpClient httpClient)
		{
			HttpClient = httpClient;
			IsDebugLog = false;
		}

		public async Task ExecuteGet<TResponse>(string tag, string url) where TResponse : TResponseObject
		{
			await Execute<TResponse>("GET", tag, url);
		}

		public async Task ExecutePost<TResponse>(string tag, string url, object body) where TResponse : TResponseObject
		{
			await Execute<TResponse>("POST", tag, url, body, null);
		}

		public async Task ExecutePost<TResponse>(string tag, string url, string body) where TResponse : TResponseObject
		{
			await Execute<TResponse>("POST", tag, url, null, body);
		}

		public async Task ExecutePost<TResponse>(string tag, string url, List<KeyValuePair<string, string>> formData) where TResponse : TResponseObject
		{
			await Execute<TResponse>("POST", tag, url, formData: formData);
		}

		public async Task ExecutePut<TResponse>(string tag, string url, object body) where TResponse : TResponseObject
		{
			await Execute<TResponse>("PUT", tag, url, body, null);
		}

		public async Task ExecutePut<TResponse>(string tag, string url, string body) where TResponse : TResponseObject
		{
			await Execute<TResponse>("PUT", tag, url, null, body);
		}

		public async Task ExecutePut<TResponse>(string tag, string url, List<KeyValuePair<string, string>> formData) where TResponse : TResponseObject
		{
			await Execute<TResponse>("PUT", tag, url, formData: formData);
		}

		public void CancelCurrentRequest()
		{
			if (HttpClient != null)
			{
				HttpClient.CancelPendingRequests();
			}
		}

		private async Task Execute<TResponse>(string method, string tag, string url, object bodyObject = null, string bodyString = "", List<KeyValuePair<string, string>> formData = null) where TResponse : TResponseObject
		{
			if (IsDebugLog)
			{
				var message = string.Format("[RequestTag:{0}] [Url:{1}]", tag, url);
				System.Diagnostics.Debug.WriteLine(message);
			}

			if (NetworkDetector == null || NetworkDetector.IsWifiConnected)
			{
				try
				{
					var response = await Execute(method, url, bodyObject, bodyString, formData);
					if (response.IsSuccessStatusCode)
					{
						var responseString = await response.Content.ReadAsStringAsync();
						if (responseString == "[]")
							await HandleResponseFailed<TResponseObject>(tag, url, null);
						else
							HandleResponseSuccess<TResponse>(tag, url, responseString);
					}
					else
					{
						await HandleResponseFailed<TResponseObject>(tag, url, response);
					}
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.StackTrace);
					await HandleResponseFailed<TResponseObject>(tag, url, null);
				}
			}
			else
			{
				HandleNetworkConnectionFailed(tag, url);
			}
		}

		private async Task<HttpResponseMessage> Execute(string method, string url, object bodyObject = null, string bodyString = "", List<KeyValuePair<string, string>> formData = null)
		{
			switch (method)
			{
				case "GET":
					return await HttpClient.GetAsync(url);
				case "POST":
					var postContent = GetRequestContent(bodyObject, bodyString, formData);
					return await HttpClient.PostAsync(url, postContent);
				case "PUT":
					var putContent = GetRequestContent(bodyObject, bodyString, formData);
					return await HttpClient.PutAsync(url, putContent);
				default:
					throw new Exception(string.Format("Method {0} not supported.", method));
			}
		}

		protected virtual HttpContent GetRequestContent(object bodyObject, string bodyString, List<KeyValuePair<string, string>> formData = null)
		{
			var body = string.Empty;

			if (bodyObject != null)
			{
				var settings = new JsonSerializerSettings();
				settings.NullValueHandling = NullValueHandling.Ignore;

				body = JsonConvert.SerializeObject(bodyObject, settings);
			}
			else if (!string.IsNullOrEmpty(bodyString))
			{
				body = bodyString;
			}
			else if (formData != null && formData.Count > 0)
			{
				var content = new MultipartFormDataContent();
				foreach (var p in formData)
				{
					content.Add(new StringContent(p.Value), p.Key);
				}
				return content;
			}

			if (IsDebugLog)
			{
				var message = string.Format("[Body:{0}]", body.Length > 500 ? body.Substring(0, 500) : body);
				System.Diagnostics.Debug.WriteLine(message);
			}

			return new StringContent(body, Encoding.UTF8, "application/json");
		}

		private void HandleResponseSuccess<TResponse>(string tag, string url, string responseString) where TResponse : TResponseObject
		{
			if (IsDebugLog)
			{
				var message = string.Format("[ResponseTag:{0}] [Response:{1}]", tag, responseString);
				System.Diagnostics.Debug.WriteLine(message);
			}

			try
			{
				if (OnResponseSuccess != null)
				{
					var responseObject = ParseResponse<TResponse>(tag, responseString);
					var args = new ServiceResponseEventArgs<TResponseObject>(tag);
					args.Url = url;
					args.ResponseString = responseString;
					args.ResponseObject = responseObject;
					OnResponseSuccess.Invoke(this, args);
				}
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e.StackTrace);
				if (IsDebugLog)
				{
					var message = string.Format("[ResponseTag:{0}] [Parse Error]", tag);
					System.Diagnostics.Debug.WriteLine(message);
				}

				if (OnParseError != null)
				{
					var args = new ServiceResponseEventArgs<TResponseObject>(tag);
					args.Url = url;
					args.ResponseString = responseString;
					OnParseError.Invoke(this, args);
				}
			}
		}

		private async Task HandleResponseFailed<TResponse>(string tag, string url, HttpResponseMessage responseMessage) where TResponse : TResponseObject
		{
			if (IsDebugLog)
			{
				var message = string.Format("[RequestTag:{0}] [Response Failed]", tag);
				System.Diagnostics.Debug.WriteLine(message);
			}

			try
			{
				if (OnResponseFailed != null)
				{
					if (responseMessage != null)
					{
						string responseString = await responseMessage.Content.ReadAsStringAsync();
						if (IsDebugLog)
						{
							var message = string.Format("[{0}]", responseString);
							System.Diagnostics.Debug.WriteLine(message);
						}

						var responseObject = ParseErrorResponse<TResponse>(tag, responseString);
						var args = new ServiceResponseEventArgs<TResponseObject>(tag);
						args.Url = url;
						args.ResponseString = responseString;
						args.ResponseObject = responseObject;
						args.ResponseMessage = responseMessage;
						OnResponseFailed.Invoke(this, args);
					}
					else
					{
						var args = new ServiceResponseEventArgs<TResponseObject>(tag);
						args.Url = url;
						OnResponseFailed.Invoke(this, args);
					}
				}
			}
			catch (Exception e)
			{
				if (IsDebugLog)
				{
					System.Diagnostics.Debug.WriteLine(e.Message);
					System.Diagnostics.Debug.WriteLine(e.StackTrace);
				}

				if (OnResponseFailed != null)
				{
					var args = new ServiceResponseEventArgs<TResponseObject>(tag);
					args.Url = url;
					args.ResponseMessage = responseMessage;
					OnResponseFailed.Invoke(this, args);
				}
			}
		}

		private void HandleNetworkConnectionFailed(string tag, string url)
		{
			if (IsDebugLog)
			{
				var message = string.Format("[RequestTag:{0}] [Network Connection Failed]", tag);
				System.Diagnostics.Debug.WriteLine(message);
			}

			if (OnNetworkConnectionFailed != null)
			{
				var args = new ServiceResponseEventArgs<TResponseObject>(tag);
				args.Url = url;
				OnNetworkConnectionFailed.Invoke(this, args);
			}
		}

		protected virtual TResponse ParseResponse<TResponse>(string tag, string responseString) where TResponse : TResponseObject
		{
			var responseObject = JsonConvert.DeserializeObject<TResponse>(responseString, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
			responseObject.IsSuccess = true;
			return responseObject;
		}

		protected virtual TResponse ParseErrorResponse<TResponse>(string tag, string responseString) where TResponse : TResponseObject
		{
			var responseObject = JsonConvert.DeserializeObject<TResponse>(responseString, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
			responseObject.IsSuccess = false;
			return responseObject;
		}
	}
}
