using System;
using System.Reflection;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text;
using PCLCrypto;
using static PCLCrypto.WinRTCrypto;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;

namespace Authy.Net
{
	/// <summary>
	/// Authy helpers.
	/// </summary>
	public class AuthyHelpers
	{
		///<summary>
		/// Remove all non-digits from the string
		/// </summary>
		///<param name="value">The string to sanitize</param>
		public static string SanitizeNumber(string value)
		{
			return Regex.Replace(value, @"\D", string.Empty);
		}

		///<summary>
		/// Validate the token entered by the user
		/// </summary>
		/// <param name="token">The token to validate</param>
		public static bool TokenIsValid(string token)
		{
			token = SanitizeNumber(token);

			if (token.Length < 6 || token.Length > 12)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Gets the version of the Assembly.
		/// </summary>
		/// <returns>The version.</returns>
		public static string GetVersion()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
			string version = fvi.ProductVersion;

			return version;
		}

		/// <summary>
		/// Get the system information
		/// </summary>
		/// <returns>The .NET version</returns>
		public static string GetSystemInfo()
		{
			var runtimeVersion = Environment.Version;
			return string.Format("Runtime v{0}", runtimeVersion);
		}

		/// <summary>
		/// Hmacsha256 the specified key and value.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		public static string GenerateHash(String key, string value)
		{
			var mac = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithm.HmacSha256);
			var keyMaterial = CryptographicBuffer.ConvertStringToBinary(key, Encoding.UTF8);
			var cryptoKey = mac.CreateKey(keyMaterial);
			var hash = CryptographicEngine.Sign(cryptoKey, CryptographicBuffer.ConvertStringToBinary(value, Encoding.UTF8));
			return CryptographicBuffer.EncodeToBase64String(hash);
		}


		/// <summary>
		/// Validates the signature.
		/// </summary>
		/// <returns><c>true</c>, if signature was validated, <c>false</c> otherwise.</returns>
		/// <param name="parameters">Parameters.</param>
		/// <param name="headers">Headers.</param>
		/// <param name="method">Method.</param>
		/// <param name="url">URL.</param>
		/// <param name="authyToken">Authy token.</param>
		public static bool ValidateSignature(Dictionary<string, string> parameters, NameValueCollection headers, string method, string url, string authyToken)
		{
			StringBuilder sb = new StringBuilder(headers.Get("X-Authy-Signature-Nonce"))
							.Append("|")
							.Append(method)
							.Append("|")
							.Append(url)
							.Append("|")
							.Append(MapToQuery(parameters));
			string signature = GenerateHash(authyToken, sb.ToString());

			return signature.Equals(headers.Get("X-Authy-Signature"));
		}

		/// <summary>
		/// Maps to query.
		/// </summary>
		/// <returns>The to query.</returns>
		/// <param name="map">Map.</param>
		public static string MapToQuery(Dictionary<string, string> map)
		{

			StringBuilder sb = new StringBuilder();

			Dictionary<string, string>.KeyCollection keys = map.Keys;

			bool first = true;

			foreach (string key in keys)
			{

				if (first)
				{
					first = false;
				}
				else {
					sb.Append("&");
				}

				string value = map[key];

				// don't encode null values
				if (value == null)
				{
					continue;
				}
				sb.Append(WebUtility.UrlEncode(key)).Append("=").Append(WebUtility.UrlEncode(value));
			}

			return sb.ToString();

		}

		/// <summary>
		/// Extract the specified properties of obj and subobj.
		/// </summary>
		/// <param name="pre">Pre.</param>
		/// <param name="obj">Object.</param>
		/// <param name="map">Map.</param>
		public static void Extract(string pre, object obj, Dictionary<string, string> map)
		{
			Dictionary<string, object> json = (Dictionary<string, object>)obj;
			foreach (KeyValuePair<string, object> entry in json)
			{
				string key = pre.Length == 0 ? entry.Key : pre + "[" + entry.Key + "]";
				try
				{
					var type = entry.Value.GetType();
					if (type.IsPrimitive || type == typeof(String) || type == typeof(bool) || type == typeof(int) || type == typeof(float) || type == typeof(double) || type == typeof(decimal))
					{
						map.Add(key, entry.Value.ToString());
					}
					else {
						Extract(key, entry.Value, map);
					}
				}
				catch (Exception e)
				{
					throw e;
				}

			}
		}
	}
}

