using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
namespace Authy.Net
{
	public class ApprobalRequestParams
	{
		private Dictionary<string, string> details;
		private Dictionary<string, string> hiddenDetails;
		private List<Dictionary<string, string>> logos;
		private string secondsToExpire;
		private string apiKey;
		private string message;

		public ApprobalRequestParams()
		{
		}

		public Dictionary<string, string> Details
		{
			get;set;
		} = new Dictionary<string, string>();

		public Dictionary<string, string> HiddenDetails
		{
			get; set;
		} = new Dictionary<string, string>();

		public List<Dictionary<string, string>> Logos
		{
			get; set;
		} = new List<Dictionary<string, string>>();

		public string SecondsToExpire
		{
			get; set;
		} = "86400";

		public string Message 
		{
			get; set;
		}

		public string ApiKey
		{
			get; set;
		}

		public JObject toJObject()
		{
			JObject obj = new JObject();
			obj.Add("details",JObject.FromObject(Details));
			obj.Add("hidden_details", JObject.FromObject(HiddenDetails));
			obj.Add("logos",JArray.FromObject(Logos.ToArray()));
			obj.Add("seconds_to_expire", SecondsToExpire);
			obj.Add("message", Message);
			obj.Add("api_key", ApiKey);

			return obj;
		}
	}
}
