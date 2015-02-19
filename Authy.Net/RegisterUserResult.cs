using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Authy.Net
{
    /// <summary>
    /// The result of a request to register a user
    /// </summary>
    public class RegisterUserResult : AuthyResult
    {
        /// <summary>
        /// The user id of a succesful registration event
        /// </summary>
        public string UserId { get; set; }

		/// <summary>
		/// The user information on Authy API
		/// </summary>
		public Dictionary<string, string> User { get; set;}
	}	
}
