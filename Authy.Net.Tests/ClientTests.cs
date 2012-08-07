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
    /// 
    /// </remarks>
    [TestClass]
    public class ClientTests
    {
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
        }

        [TestMethod]
        public void Registration_BadPhoneNumber()
        {
        }

        [TestMethod]
        public void Registration_BadEmailAndPhoneNumber()
        {
        }

        [TestMethod]
        public void Registration_BadApiKey()
        {
        }

        [TestMethod]
        public void Verification_Success()
        {

        }

        [TestMethod]
        public void Verification_BadToken()
        {
        }

        [TestMethod]
        public void Verification_BadApiKey()
        {
        }

        [TestMethod]
        public void Verification_InvalidUser()
        {
        }

        private Client GoodApiKeyClient
        {
            get { return new Client(ApiKey.Value, true); }
        }

        private Lazy<string> ApiKey = new Lazy<string>(() => File.ReadAllText("ApiKey.txt"));
    }
}
