using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Authy.Net
{
    /// <summary>
    /// A base authy result
    /// </summary>
    public class AuthyResult
    {
        /// <summary>
        /// The status of a request
        /// </summary>
        public AuthyStatus Status { get; set; }

        /// <summary>
        /// The raw response returned from the API
        /// </summary>
        public string RawResponse { get; set; }

		/// <summary>
		/// The request was success
		/// </summary>
		public bool Success { get; set; }

		/// <summary>
		/// The message from the API
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// The list of erros
		/// <summary>
		public Dictionary<string, string> Errors { get; set; }
	}
}
