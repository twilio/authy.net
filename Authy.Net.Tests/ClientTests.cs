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
            Assert.Fail("More Specific email failure");
        }

        [TestMethod]
        public void Registration_BadPhoneNumber()
        {
            var client = this.GoodApiKeyClient;
            var result = client.RegisterUser("test@test.com", "aaa-456-7890");
            Assert.AreEqual(AuthyStatus.BadRequest, result.Status);
            Assert.Fail("More Specific phone failure");
        }

        [TestMethod]
        public void Registration_BadEmailAndPhoneNumber()
        {
            var client = this.GoodApiKeyClient;
            var result = client.RegisterUser("test.com", "aaa-456-7890");
            Assert.AreEqual(AuthyStatus.BadRequest, result.Status);
            Assert.Fail("More Specific email and phone failure");
        }

        [TestMethod]
        public void Registration_BadApiKey()
        {
            var client = new AuthyClient(badApiKey, true);
            var result = client.RegisterUser("test@test.com", "123-456-7890");
            Assert.AreEqual(AuthyStatus.InvalidApiKey, result.Status);
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
            var client = this.GoodApiKeyClient;
            var result = client.VerifyToken("1", "1111111");
            Assert.AreEqual(AuthyStatus.InvalidToken, result.Status);
        }

        [TestMethod]
        public void Verification_BadApiKey()
        {
            var client = new AuthyClient(badApiKey, true);
            var result = client.VerifyToken("1", "0000000");
            Assert.AreEqual(AuthyStatus.InvalidApiKey, result.Status);
        }

        [TestMethod]
        public void Verification_InvalidUser()
        {
            var client = this.GoodApiKeyClient;
            var result = client.VerifyToken("99999", "1111111");
            Assert.AreEqual(AuthyStatus.Unauthorized, result.Status);
            Assert.Fail("more specific message indicating the user was bad");
        }

        private AuthyClient GoodApiKeyClient
        {
            get { return new AuthyClient(ApiKey.Value, true); }
        }

        private Lazy<string> ApiKey = new Lazy<string>(() => File.ReadAllText("ApiKey.txt"));
    }
}