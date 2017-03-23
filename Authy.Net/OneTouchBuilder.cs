using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Authy.Net
{
	/// <summary>
	/// One touch builder, create the object step by step,
	/// included validation.
	/// </summary>
	public class OneTouchBuilder
	{
		public static int MAX_STRING_SIZE = 200;
		public static string MESSAGE_ERROR = "Param message cannot be null or empty  and it's length needs to be less than 200 max characters.";
		public static string AUTHYID_ERROR = "Param authyId cannot be null and should be the id of the user that will be authorized using OneTouch.";
		public static string DETAIL_ERROR = "Each entry(key,value) for a detail needs to have not null or empty values and keys and it's lengths cannot exceed 200 max characters.";
		public static string HIDDEN_DETAIL_ERROR = "Each entry(key,value) for a hidden detail needs to have not null or empty values and keys and it's lengths cannot exceed 200 max characters.";
		public static string LOGO_ERROR_RES = "The 'Resolution' for a logo cannot be null.";
		public static string LOGO_ERROR_URL = "The 'url' for a logo cannot be null or empty  and it's length needs to be less than 500 max characters.";
		public static string LOGO_ERROR_DEFAULT = "If you provide logos you should always provide the default Resolution.";
		public static string LOGO_ERROR_KEYS = "Invalid logos dict keys. Expected \'res\' or \'url\'.";
		public static string LOGO_ERROR_NO_KEYS = " 'res' and 'url' keys are not present.";
		public static int MAXSIZEURL = 500;
		ApprobalRequestParams parameters;


		/// <summary>
		/// Initializer 
		/// </summary>
		public OneTouchBuilder Begin()
		{
			parameters = new ApprobalRequestParams();
			return this;
		}

		/// <summary>
		/// APIs the key.
		/// </summary>
		/// <returns>The key.</returns>
		/// <param name="apiKey">API key.</param>
		public OneTouchBuilder ApiKey(String apiKey)
		{
			parameters.ApiKey = apiKey;
			return this;
		}

		/// <summary>
		/// Message sended to the server
		/// </summary>
		/// <param name="message">Message.</param>
		public OneTouchBuilder Message(String message=null)
		{
			if (String.IsNullOrEmpty(message) || message.Length > MAX_STRING_SIZE)
			{
				throw new OneTouchBuilderException(MESSAGE_ERROR);
			}

			parameters.Message = message;
			return this;
		}

		/// <summary>
		/// Seconds to expire.
		/// </summary>
		/// <param name="seconds">Seconds.</param>
		public OneTouchBuilder SecondsToExpire(String seconds)
		{
			parameters.SecondsToExpire = seconds;
			return this;
		}

		/// <summary>
		/// Hiddens the details.
		/// </summary>
		/// <param name="hiddenDetails">Hidden details.</param>
		public OneTouchBuilder HiddenDetails(Dictionary<string, string> hiddenDetails)
		{
			foreach (var entry in hiddenDetails)
			{
				if (String.IsNullOrEmpty(entry.Value) || entry.Value.Length > MAX_STRING_SIZE)
				{
					throw new OneTouchBuilderException(HIDDEN_DETAIL_ERROR);
				}

				if (String.IsNullOrEmpty(entry.Key) || entry.Key.Length > MAX_STRING_SIZE)
				{
					throw new OneTouchBuilderException(HIDDEN_DETAIL_ERROR);
				}
			}
			parameters.HiddenDetails = hiddenDetails;
			return this;
		}

		/// <summary>
		/// Details the specified details.
		/// </summary>
		/// <param name="details">Details.</param>
		public OneTouchBuilder Details(Dictionary<string, string> details)
		{
			foreach (var entry in details)
			{
				if (String.IsNullOrEmpty(entry.Value) || entry.Value.Length > MAX_STRING_SIZE)
				{
					throw new OneTouchBuilderException(DETAIL_ERROR);
				}

				if (String.IsNullOrEmpty(entry.Key) || entry.Key.Length > MAX_STRING_SIZE)
				{
					throw new OneTouchBuilderException(DETAIL_ERROR);
				}
			}
			parameters.Details = details;
			return this;
		}

		/// <summary>
		/// Logos the specified logos.
		/// </summary>
		/// <param name="logos">Logos.</param>
		public OneTouchBuilder Logos(List<Dictionary<string, string>> logos)
		{
			Boolean resolutionDefault = false;
			if (logos != null)
			{
				foreach (var logo in logos)
				{

					if (!logo.ContainsKey("res") || !logo.ContainsKey("url"))
					{
						throw new OneTouchBuilderException(LOGO_ERROR_NO_KEYS);
					}

					foreach (var dic in logo)
					{
						if (dic.Key.Equals("res"))
						{
							if (!resolutionDefault)
							{
								resolutionDefault = dic.Value.Equals("default") ? true : false;
							}
						}
						else if (dic.Key.Equals("url"))
						{
							if (dic.Value.Length > MAXSIZEURL)
							{
								throw new OneTouchBuilderException(LOGO_ERROR_URL);
							}
						}
						else {
							throw new OneTouchBuilderException(LOGO_ERROR_KEYS);
						}
					}
					if (!resolutionDefault)
					{
						throw new OneTouchBuilderException(LOGO_ERROR_DEFAULT);
					}
				}
			}

			parameters.Logos = logos;
			return this;
		}

		/// <summary>
		/// Build this instance.
		/// </summary>
		public ApprobalRequestParams Build()
		{
			//return parameters;
			if (String.IsNullOrEmpty(parameters.Message))
			{
				throw new OneTouchBuilderException(MESSAGE_ERROR);
			}
			return parameters;
		}
	}

	/// <summary>
	/// One touch builder exception.
	/// </summary>
	public class OneTouchBuilderException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Authy.Net.OneTouchBuilderException"/> class.
		/// </summary>
		public OneTouchBuilderException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Authy.Net.OneTouchBuilderException"/> class.
		/// </summary>
		/// <param name="message">Message.</param>
		public OneTouchBuilderException(string message): base(message)
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Authy.Net.OneTouchBuilderException"/> class.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="inner">Inner.</param>
		public OneTouchBuilderException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
