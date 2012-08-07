using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace Authy.Net
{
    /// <summary>
    /// Client for interacting with the Authy API
    /// </summary>
    public class Client
    {
        private readonly string apiKey;
        private readonly bool test;

        /// <summary>
        /// Creates an instance of the Authy client
        /// </summary>
        /// <param name="apiKey">The api key used to access the rest api</param>
        /// <param name="test">indicates that the sandbox should be used</param>
        public Client(string apiKey, bool test = false)
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

                //TODO add error handling and better JSON parsing
                return new RegisterUserResult()
                {
                    Status = AuthyStatus.Success,

                    // TODO use real JSON parsing rather than a hackey regex
                    UserId = Regex.Match(Encoding.ASCII.GetString(response), "id\":([0-9]+)").Groups[1].Value
                };
            });
        }

        /// <summary>
        /// Verify a token with authy
        /// </summary>
        /// <param name="userId">The Authy user id</param>
        /// <param name="token">The token to verify</param>
        public VerifyTokenResult VerifyToken(string userId, string token)
        {
            var url = string.Format("{0}/protected/json/verify/{1}/{2}?api_key={3}", this.baseUrl, token, userId, this.apiKey);
            return this.Execute<VerifyTokenResult>(client =>
            {
                var response = client.DownloadString(url);

                //TODO add error handling and better JSON parsing
                return new VerifyTokenResult()
                {
                    Status = AuthyStatus.Success
                };
            });
        }

        //TODO, add support for the optional SMS API

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
                return new TResult() { Status = AuthyStatus.BadRequest, RawResponse = body };
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
