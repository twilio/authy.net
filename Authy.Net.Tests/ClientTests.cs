using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.IO;

namespace Authy.Net.Tests
{
    /// <remarks>
    /// 
    /// Important!!!
    /// 
    /// These tests won't pass unless you add a file Called ApiKey.txt to the root of the test project.
    /// Once created add a single line containing a sandbox api key.
    /// You can obtain such a key from the dashboard in your Authy account after you have created an application.
    /// 
    /// </remarks>
    [TestClass]
    public class ClientTests
    {
        const string badApiKey = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

        [TestMethod]
        public void Registration_Success()
        {
            var client = this.GoodApiKeyClient;
            var result = client.RegisterUser("test@test.com", "123-456-7890");
            Assert.AreEqual(AuthyStatus.Success, result.Status);
            Assert.IsNotNull(result.UserId);
            Assert.AreNotEqual(string.Empty, result.UserId);
        }

        [TestMethod]
        public void Registration_BadEmail()
        {
            var client = this.GoodApiKeyClient;
            var result = client.RegisterUser("test.com", "123-456-7891");
            Assert.AreEqual(AuthyStatus.BadRequest, result.Status);
            Assert.IsFalse((result.ErrorFields & AuthyErrorFields.Cellphone) == AuthyErrorFields.Cellphone);
            Assert.IsTrue((result.ErrorFields & AuthyErrorFields.Email) == AuthyErrorFields.Email);
        }

        [TestMethod]
        public void Registration_BadPhoneNumber()
        {
            var client = this.GoodApiKeyClient;
            var result = client.RegisterUser("test@test.com", "aaa-456-7890");
            Assert.AreEqual(AuthyStatus.BadRequest, result.Status);
            Assert.IsTrue((result.ErrorFields & AuthyErrorFields.Cellphone) == AuthyErrorFields.Cellphone);
            Assert.IsFalse((result.ErrorFields & AuthyErrorFields.Email) == AuthyErrorFields.Email);
        }

        [TestMethod]
        public void Registration_BadEmailAndPhoneNumber()
        {
            var client = this.GoodApiKeyClient;
            var result = client.RegisterUser("test.com", "aaa-456-7890");
            Assert.AreEqual(AuthyStatus.BadRequest, result.Status);
            Assert.IsTrue((result.ErrorFields & AuthyErrorFields.Cellphone) == AuthyErrorFields.Cellphone);
            Assert.IsTrue((result.ErrorFields & AuthyErrorFields.Email) == AuthyErrorFields.Email);
        }

        [TestMethod]
        public void Registration_BadApiKey()
        {
            var client = new AuthyClient(badApiKey, true);
            var result = client.RegisterUser("test@test.com", "123-456-7890");
            Assert.AreEqual(AuthyStatus.Unauthorized, result.Status);
            Assert.IsTrue((result.ErrorFields & AuthyErrorFields.ApiKey) == AuthyErrorFields.ApiKey);
        }

        [TestMethod]
        public void Verification_Success()
        {
            var client = this.GoodApiKeyClient;
            var result = client.VerifyToken("1", "0000000");
            Assert.AreEqual(AuthyStatus.Success, result.Status);
        }

        [TestMethod]
        public void Verification_BadToken()
        {
            // duplicate a succesful registration to get a valid user id
            var client = this.GoodApiKeyClient;
            var registrationResult = client.RegisterUser("test@test.com", "123-456-7890");
            Assert.AreEqual(AuthyStatus.Success, registrationResult.Status);
            Assert.IsNotNull(registrationResult.UserId);
            Assert.AreNotEqual(string.Empty, registrationResult.UserId);

            // now for the actual bad token test
            var verifyResult = client.VerifyToken(registrationResult.UserId, "1234567");
            Assert.AreEqual(AuthyStatus.Unauthorized, verifyResult.Status);
            Assert.IsTrue((verifyResult.ErrorFields & AuthyErrorFields.Token) == AuthyErrorFields.Token);
        }

        [TestMethod]
        public void Verification_BadApiKey()
        {
            var client = new AuthyClient(badApiKey, true);
            var result = client.VerifyToken("1", "0000000");

            Assert.AreEqual(AuthyStatus.Unauthorized, result.Status);
            Assert.IsTrue((result.ErrorFields & AuthyErrorFields.ApiKey) == AuthyErrorFields.ApiKey);
        }

        [TestMethod]
        public void Verification_InvalidUser()
        {
            var client = this.GoodApiKeyClient;
            var result = client.VerifyToken("99999", "1111111");
            Assert.AreEqual(AuthyStatus.Unauthorized, result.Status);
            Assert.IsTrue((result.ErrorFields & AuthyErrorFields.User) == AuthyErrorFields.User);
        }

        private AuthyClient GoodApiKeyClient
        {
            get { return new AuthyClient(ApiKey.Value, true); }
        }

        private Lazy<string> ApiKey = new Lazy<string>(() => File.ReadAllText("ApiKey.txt"));
    }
}