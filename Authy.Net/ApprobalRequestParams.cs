using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
namespace Authy.Net
{
	/// <summary>
	/// Approbal request parameters.
	/// </summary>
	public class ApprobalRequestParams
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Authy.Net.ApprobalRequestParams"/> class.
		/// </summary>
		public ApprobalRequestParams()
		{
		}

		/// <summary>
		/// Gets or sets the details.
		/// </summary>
		/// <value>The details.</value>
		public Dictionary<string, string> Details
		{
			get;set;
		} = new Dictionary<string, string>();

		/// <summary>
		/// Gets or sets the hidden details.
		/// </summary>
		/// <value>The hidden details.</value>
		public Dictionary<string, string> HiddenDetails
		{
			get; set;
		} = new Dictionary<string, string>();

		/// <summary>
		/// Gets or sets the logos.
		/// </summary>
		/// <value>The logos.</value>
		public List<Dictionary<string, string>> Logos
		{
			get; set;
		} = new List<Dictionary<string, string>>();

		/// <summary>
		/// Gets or sets the seconds to expire.
		/// </summary>
		/// <value>The seconds to expire.</value>
		public string SecondsToExpire
		{
			get; set;
		} = "86400";

		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		/// <value>The message.</value>
		public string Message 
		{
			get; set;
		}

		/// <summary>
		/// Gets or sets the API key.
		/// </summary>
		/// <value>The API key.</value>
		public string ApiKey
		{
			get; set;
		}

		/// <summary>
		/// Assign the approbalRequestParams's properties to a JObject.
		/// </summary>
		/// <returns>The JObject created.</returns>
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
