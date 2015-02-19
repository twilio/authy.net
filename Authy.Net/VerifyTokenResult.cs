using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Authy.Net
{
    /// <summary>
    /// The result of a request to verify a token
    /// </summary>
    public class VerifyTokenResult : AuthyResult
    {
        /// <summary>
        /// Confirmation if token is valid or invalid
        /// </summary>
        public string Token { get; set; }
    }
}
