using System;
using NUnit.Framework;
using Authy.Net;

namespace Testing
{
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
		const string validApiKey = "36cfe745b497f8b55c8377971b75f7b7";
		/// <summary>
		/// This is intended to be a bad API key. Don't set it to a good value!
		/// </summary>
		const string invalidApiKey = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

		private AuthyClient ValidAuthyClient {
			get { return new AuthyClient(validApiKey, true); }
		}

		[Test]
		public void RegistrationSuccess () {
			var client = this.ValidAuthyClient;
			var result = client.RegisterUser("test@test.com", "123-456-7890");

			Assert.AreEqual(AuthyStatus.Success, result.Status);
			Assert.IsNotNull(result.UserId);
			Assert.AreNotEqual(string.Empty, result.UserId);
		}
			
		[Test]
		public void RegistrationInvalidEmail() {
			var client = this.ValidAuthyClient;
			var result = client.RegisterUser("test.com", "123-456-7891");
			Assert.AreEqual(AuthyStatus.BadRequest, result.Status);
		}

		[Test]
		public void RegistrationInvalidPhoneNumber() {
			var client = this.ValidAuthyClient;
			var result = client.RegisterUser ("test@test.com", "12345", 57);
			Assert.AreEqual(AuthyStatus.BadRequest, result.Status);
		}

	}
}