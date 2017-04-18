﻿using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Authy.Net
{
    /// <summary>
    /// Client for interacting with the Authy API
    /// </summary>
    /// <remarks>
    /// This library is threadsafe since the only shared state is stored in private readonly fields.
    ///
    /// Creating a single instance of the client and using it across multiple threads isn't a problem.
    /// </remarks>
    public class AuthyClient
    {
        private readonly string apiKey;
        private readonly bool test;

        /// <summary>
        /// Creates an instance of the Authy client
        /// </summary>
        /// <param name="apiKey">The api key used to access the rest api</param>
        /// <param name="test">indicates that the sandbox should be used</param>
        public AuthyClient(string apiKey, bool test = false)
        {
            this.apiKey = apiKey;
            this.test = test;
        }

		/// <summary>
		/// Registers the user.
		/// </summary>
		/// <returns>The user.</returns>
		/// <param name="email">Email.</param>
		/// <param name="cellPhoneNumber">Cell phone number.</param>
		/// <param name="countryCode">Country code.</param>
		/// <param name="sendInstallLinkViaSms">If set to <c>true</c> send install link via sms.</param>
        public RegisterUserResult RegisterUser(string email, string cellPhoneNumber, int countryCode = 1, bool sendInstallLinkViaSms=false)
        {
            var request = new System.Collections.Specialized.NameValueCollection()
            {
                {"user[email]", email},
                {"user[cellphone]",cellPhoneNumber},
                {"user[country_code]",countryCode.ToString()},
				{"user[send_install_link_via_sms]", sendInstallLinkViaSms.ToString() }
            };

            var url = string.Format("{0}/protected/json/users/new?api_key={1}", this.baseUrl, this.apiKey);
            return this.Execute<RegisterUserResult>(client =>
            {
                var response = client.UploadValues(url, request);
                var textResponse = Encoding.ASCII.GetString(response);

                RegisterUserResult apiResponse = JsonConvert.DeserializeObject<RegisterUserResult>(textResponse);
                apiResponse.RawResponse = textResponse;
                apiResponse.Status = AuthyStatus.Success;
                apiResponse.UserId = apiResponse.User["id"];

                return apiResponse;
            });
        }

        /// <summary>
        /// Verify a token with authy
        /// </summary>
        /// <param name="userId">The Authy user id</param>
        /// <param name="token">The token to verify</param>
        /// <param name="force">Force verification to occur even if the user isn't registered (if the user hasn't finished registering the default is to succesfully validate)</param>
        public VerifyTokenResult VerifyToken(string userId, string token, bool force = false)
        {
            if ( !AuthyHelpers.TokenIsValid(token))
            {
                Dictionary<string, string> errors = new Dictionary<string, string>();
                errors.Add("token", "is invalid");

                return new VerifyTokenResult() {
                    Status = AuthyStatus.BadRequest,
                    Success = false,
                    Message = "Token is invalid.",
                    Errors = errors
                };
            }

            token = AuthyHelpers.SanitizeNumber(token);
            userId = AuthyHelpers.SanitizeNumber(userId);

            var url = string.Format("{0}/protected/json/verify/{1}/{2}?api_key={3}{4}", this.baseUrl, token, userId, this.apiKey, force ? "&force=true" : string.Empty);
            return this.Execute<VerifyTokenResult>(client =>
            {
                var response = client.DownloadString(url);

                VerifyTokenResult apiResponse = JsonConvert.DeserializeObject<VerifyTokenResult>(response);

                if (apiResponse.Token == "is valid")
                {
                    apiResponse.Status = AuthyStatus.Success;
                }
                else
                {
                    apiResponse.Success = false;
                    apiResponse.Status = AuthyStatus.Unauthorized;
                }
                apiResponse.RawResponse = response;

                return apiResponse;
            });
        }

        /// <summary>
        /// Send an SMS message to a user who isn't registered.  If the user is registered with a mobile app then no message will be sent.
        /// </summary>
        /// <param name="userId">The user ID to send the message to</param>
        /// <param name="force">Force a message to be sent even if the user is already registered as an app user. This will incrase your costs</param>
        public SendSmsResult SendSms(string userId, bool force = false)
        {
            userId = AuthyHelpers.SanitizeNumber(userId);

            var url = string.Format("{0}/protected/json/sms/{1}?api_key={2}{3}", this.baseUrl, userId, this.apiKey, force ? "&force=true" : string.Empty);
            return this.Execute<SendSmsResult>(client =>
            {
                var response = client.DownloadString(url);

                SendSmsResult apiResponse = JsonConvert.DeserializeObject<SendSmsResult>(response);
                apiResponse.Status = AuthyStatus.Success;
                apiResponse.RawResponse = response;

                return apiResponse;
            });
        }

        /// <summary>
        /// Send the token via phone call to a user who isn't registered.  If the user is registered with a mobile app then the phone call will be ignored.
        /// </summary>
        /// <param name="userId">The user ID to send the phone call to</param>
        /// <param name="force">Force to the phone call to be sent even if the user is already registered as an app user. This will incrase your costs</param>
        public AuthyResult StartPhoneCall(string userId, bool force = false)
        {
            userId = AuthyHelpers.SanitizeNumber(userId);

            var url = string.Format("{0}/protected/json/call/{1}?api_key={2}{3}", this.baseUrl, userId, this.apiKey, force ? "&force=true" : string.Empty);
            return this.Execute<AuthyResult>(client =>
            {
                var response = client.DownloadString(url);

                AuthyResult apiResponse = JsonConvert.DeserializeObject<AuthyResult>(response);
                apiResponse.Status = AuthyStatus.Success;
                apiResponse.RawResponse = response;

                return apiResponse;
            });
        }

		/// <summary>
		/// Ones the touch.
		/// </summary>
		/// <returns>The touch.</returns>
		/// <param name="userId">User identifier.</param>
		/// <param name="parameters">Parameters.</param>
		public OneTouchResult OneTouch(string userId, ApprobalRequestParams parameters)
		{
			JObject obj;
			userId = AuthyHelpers.SanitizeNumber(userId);

			obj = parameters.toJObject();

			var url = string.Format("{0}/onetouch/json/users/{1}/approval_requests", this.baseUrl, userId);

			var jsonData = JsonConvert.SerializeObject(obj);
			return this.ExecuteWebRequest<OneTouchResult>(client =>
			{
				using (var streamWriter = new StreamWriter(client.GetRequestStream()))
				{
					string json = jsonData;

					streamWriter.Write(json);
					streamWriter.Flush();
				}

				var httpResponse = (HttpWebResponse)client.GetResponse();
				string body = "";
				using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
				{
					body = streamReader.ReadToEnd();
				}

				OneTouchResult apiResponse = JsonConvert.DeserializeObject<OneTouchResult>(body);
				apiResponse.RawResponse = body;

				return apiResponse;
			}, url);

		}

		/// <summary>
		/// Gets the approval status.
		/// </summary>
		/// <returns>The approval status.</returns>
		/// <param name="uuid">UUID.</param>
		/// <param name="force">If set to <c>true</c> force.</param>
		public OneTouchResult GetApprovalStatus(string uuid, bool force = false)
		{
			var url = string.Format("{0}/onetouch/json/approval_requests/{1}?api_key={2}{3}", this.baseUrl, uuid, this.apiKey, force ? "&force=true" : string.Empty);
			return this.Execute<OneTouchResult>(client =>
			{
				var response = client.DownloadString(url);

				OneTouchResult apiResponse = JsonConvert.DeserializeObject<OneTouchResult>(response);
				apiResponse.Status = AuthyStatus.Success;
				apiResponse.RawResponse = response;

				return apiResponse;
			});
		}

		/// <summary>
		/// Execute the specified execute.
		/// </summary>
		/// <param name="execute">Execute.</param>
		/// <typeparam name="TResult">The 1st type parameter.</typeparam>
        private TResult Execute<TResult>(Func<WebClient, TResult> execute)
            where TResult : AuthyResult, new()
        {
            var client = new WebClient();
            var libraryVersion = AuthyHelpers.GetVersion();
            var runtimeVersion = AuthyHelpers.GetSystemInfo();
            var userAgent = string.Format("AuthyNet/{0} ({1})", libraryVersion, runtimeVersion);

            // Set a custom user agent
            client.Headers.Add("user-agent", userAgent);

            try
            {
                return execute(client);
            }
            catch (WebException webex)
            {
                var response = webex.Response.GetResponseStream();

                string body;
                using (var reader = new StreamReader(response))
                {
                    body = reader.ReadToEnd();
                }

                TResult result = JsonConvert.DeserializeObject<TResult>(body);

                switch (((HttpWebResponse)webex.Response).StatusCode)
                {
                    case HttpStatusCode.ServiceUnavailable:
                        result.Status = AuthyStatus.ServiceUnavailable;
                        break;
                    case HttpStatusCode.Unauthorized:
                        result.Status = AuthyStatus.Unauthorized;
                        break;
                    default:
                    case HttpStatusCode.BadRequest:
                        result.Status = AuthyStatus.BadRequest;
                        break;
                }
                return result;
            }
            finally
            {
                client.Dispose();
            }
        }

		/// <summary>
		/// Executes the web request.
		/// </summary>
		/// <returns>The web request.</returns>
		/// <param name="execute">Execute.</param>
		/// <param name="url">URL.</param>
		/// <typeparam name="TResult">The 1st type parameter.</typeparam>
		private TResult ExecuteWebRequest<TResult>(Func<WebRequest, TResult> execute, string url)
			where TResult : AuthyResult, new()
		{
			var client = (HttpWebRequest)WebRequest.Create(url);
			var libraryVersion = AuthyHelpers.GetVersion();
			var runtimeVersion = AuthyHelpers.GetSystemInfo();
			var userAgent = string.Format("AuthyNet/{0} ({1})", libraryVersion, runtimeVersion);

			// Set a custom user agent
			client.UserAgent = userAgent;

			client.ContentType = "application/json; charset=utf-8";
			client.Method = "POST";
			try
			{
				return execute(client);
			}
			catch (WebException webex)
			{
				var response = webex.Response.GetResponseStream();

				string body;
				using (var reader = new StreamReader(response))
				{
					body = reader.ReadToEnd();
				}

				TResult result = JsonConvert.DeserializeObject<TResult>(body);

				switch (((HttpWebResponse)webex.Response).StatusCode)
				{
					case HttpStatusCode.ServiceUnavailable:
						result.Status = AuthyStatus.ServiceUnavailable;
						break;
					case HttpStatusCode.Unauthorized:
						result.Status = AuthyStatus.Unauthorized;
						break;
					default:
					case HttpStatusCode.BadRequest:
						result.Status = AuthyStatus.BadRequest;
						break;
				}
				return result;
			}
			finally
			{
				//
			}

		}

		/// <summary>
		/// Gets the base URL.
		/// </summary>
		/// <value>The base URL.</value>
        private string baseUrl
        {
            get { return this.test ? "http://sandbox-api.authy.com" : "https://api.authy.com"; }
        }
    }
}
