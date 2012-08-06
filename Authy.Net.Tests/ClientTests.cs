using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace Authy.Net.Tests
{
    /// <remarks>
    /// In order for these tests to pass, the configuration must be updated with a valid API key
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
            get { return new Client(ConfigurationManager.AppSettings["ApiKey"], true); }
        }
    }
}
