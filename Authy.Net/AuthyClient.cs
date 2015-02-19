using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

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
        /// Register a user
        /// </summary>
        /// <param name="email">Email address</param>
        /// <param name="cellPhoneNumber">Cell phone number</param>
        /// <param name="countryCode">Country code</param>
        /// <returns>RegisterUserResult object containing the details about the attempted register user request</returns>
        public RegisterUserResult RegisterUser(string email, string cellPhoneNumber, int countryCode = 1)
        {
            var request = new System.Collections.Specialized.NameValueCollection()
            {
	            {"user[email]", email},
	            {"user[cellphone]",cellPhoneNumber},
	            {"user[country_code]",countryCode.ToString()}
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
        /// <param name="force">Force verification to occur even if the user isn't registered (if the user hasn't finished registering the deefault is to succesfully validate)</param>
        public VerifyTokenResult VerifyToken(string userId, string token, bool force = false)
        {
            var url = string.Format("{0}/protected/json/verify/{1}/{2}?api_key={3}{4}", this.baseUrl, token, userId, this.apiKey, force ? "&force=true" : string.Empty);
            return this.Execute<VerifyTokenResult>(client =>
            {
                var response = client.DownloadString(url);
                return new VerifyTokenResult() { Status = AuthyStatus.Success, RawResponse = response };
            });
        }

        /// <summary>
        /// Send an SMS message to a user who isn't registered.  If the user is registered with a mobile app then no message will be sent.
        /// </summary>
        /// <param name="userId">The user ID to send the message to</param>
        /// <param name="force">Force a message to be sent even if the user is already reigistered as an app user.  This will incrase your costs</param>
        public SendSmsResult SendSms(string userId, bool force = false)
        {
            var url = string.Format("{0}/protected/json/sms/{1}?api_key={2}{3}", this.baseUrl, userId, this.apiKey, force ? "&force=true" : string.Empty);
            return this.Execute<SendSmsResult>(client =>
            {
                var response = client.DownloadString(url);
                return new SendSmsResult() { Status = AuthyStatus.Success, RawResponse = response };
            });
        }

        private TResult Execute<TResult>(Func<WebClient, TResult> execute)
            where TResult : AuthyResult, new()
        {
            var client = new WebClient();
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

                //TODO parse out the json response into an error dictionary
                var result = new TResult() { RawResponse = body };

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

        private string baseUrl
        {
            get { return this.test ? "http://sandbox-api.authy.com" : "https://api.authy.com"; }
        }
    }
}
