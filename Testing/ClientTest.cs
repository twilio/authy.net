using NUnit.Framework;
using Authy.Net;
using System;

namespace Testing {
    /// <remarks>
    ///
    /// Important!!!
    ///
    /// These tests won't pass unless you set the api key below.
    /// You can obtain such a key from the dashboard in your Authy account after you have created an application.
    ///
    /// </remarks>
    [TestFixture]
    public class ClientTest
    {
		/// <summary>
		/// Api Key. This must be set from your own account
		/// </summary>
		//const string validApiKey = "d57d919d11e6b221c9bf6f7c882028f9";
		const string validApiKey = "xzfVSc8CTwU2Mpl959CcMhw3fZBfI5F0";
        /// <summary>

        /// This is intended to be a bad API key. Don't set it to a good value!
        /// </summary>
        const string invalidApiKey = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

        private AuthyClient ValidAuthyClient {
            get { return new AuthyClient(validApiKey, false); }
        }

        [Test]
        public void Registration_Success () {
            var client = this.ValidAuthyClient;
            var result = client.RegisterUser("hans+2@mobileaws.com", "3017872848", 57);

            Assert.AreEqual(AuthyStatus.Success, result.Status);
            Assert.IsNotNull(result.UserId);
            Assert.AreNotEqual(result.UserId, string.Empty);
            Assert.AreEqual(result.Success, true);
            Assert.AreEqual (result.Message, "User created successfully.");
        }

        [Test]
        public void Registration_InvalidEmail() {
            var client = this.ValidAuthyClient;
            var result = client.RegisterUser("test.com", "317-338-9302", 57);

            Assert.AreEqual(AuthyStatus.BadRequest, result.Status);
            Assert.AreEqual(result.Success, false);
            Assert.AreEqual(result.Errors["email"], "is invalid");
            Assert.AreEqual (result.Message, "User was not valid");
        }

        [Test]
        public void Registration_InvalidPhoneNumber() {
            var client = this.ValidAuthyClient;
            var result = client.RegisterUser ("test@test.com", "12345", 57);

            Assert.AreEqual(AuthyStatus.BadRequest, result.Status);
            Assert.AreEqual(result.Success, false);
			Assert.AreEqual(result.Errors["cellphone"], "is invalid");
            Assert.AreEqual (result.Message, "User was not valid");
        }

        [Test]
        public void Registration_InvalidApiKey() {
            var client = new AuthyClient(invalidApiKey, true);
            var result = client.RegisterUser("test@test.com", "317-338-9302", 57);

            Assert.AreEqual(AuthyStatus.Unauthorized, result.Status);
            Assert.AreEqual(result.Success, false);
            Assert.AreEqual(result.Message, "Invalid API key");
        }

        [Test]
        public void Verification_Success() {
            var client = this.ValidAuthyClient;
			var registration = client.RegisterUser("hans+2@mobileaws.com", "3017872848", 57);
			var result = client.VerifyToken(registration.UserId, "0000000");

            Assert.AreEqual(result.Status, AuthyStatus.Success);
            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(result.Message, "Token is valid.");
            Assert.AreEqual(result.Token, "is valid");
        }

        [Test]
        public void Verification_InvalidToken() {
            var client = this.ValidAuthyClient;
            var registrationResult = client.RegisterUser("test@test.com", "317-338-9302", 57);
            var result = client.VerifyToken(registrationResult.UserId, "1234567");

            Assert.AreEqual(result.Status, AuthyStatus.Unauthorized);
            Assert.AreEqual(result.Success, false);
            Assert.AreEqual (result.Message, "Token is invalid");
        }

        [Test]
        public void Verification_InvalidApiKey() {
            var client = new AuthyClient(invalidApiKey, true);
            var result = client.VerifyToken("1", "0000000");

            Assert.AreEqual(result.Status, AuthyStatus.Unauthorized);
            Assert.AreEqual(result.Success, false);
            Assert.AreEqual(result.Message, "Invalid API key");
        }

        [Test]
        public void Verification_InvalidUser() {
            var client = this.ValidAuthyClient;
            var result = client.VerifyToken("99999", "1111111");

            Assert.AreEqual(result.Status, AuthyStatus.Unauthorized);
            Assert.AreEqual(result.Success, false);
            Assert.AreEqual(result.Message, "User doesn't exist");
        }

        [Test]
        public void Verification_BadToken() {
            var client = this.ValidAuthyClient;
            var result = client.VerifyToken("1", "1234");

            Assert.AreEqual(result.Status, AuthyStatus.BadRequest);
            Assert.AreEqual(result.Success, false);
            Assert.AreEqual(result.Message, "Token is invalid.");
            Assert.AreEqual(result.Errors ["token"], "is invalid");
        }

        [Test]
        public void Send_SMS() {
            var client = this.ValidAuthyClient;
            var result = client.SendSms("30144611", true);

			Assert.AreEqual(result.Status,  AuthyStatus.Success);
			Assert.AreEqual(result.Message, "SMS token was sent");
        }

        [Test]
        public void Start_PhoneCall() {
            var client = this.ValidAuthyClient;
            var result = client.StartPhoneCall("30144611", true);

            Assert.AreEqual(result.Message, "Call started...");
        }

		[Test]
		public void Start_OneTouch()
		{
			var client = this.ValidAuthyClient;
			var result = client.OneTouch("30144611");
			Assert.AreEqual(result.Success, true);
			Assert.IsNotNull(result.Approval_Request["uuid"]);
		}
    }
}
