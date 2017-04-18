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
		/// Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
        public AuthyStatus Status { get; set; }

		/// <summary>
		/// Gets or sets the raw response.
		/// </summary>
		/// <value>The raw response.</value>
        public string RawResponse { get; set; }


		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Authy.Net.AuthyResult"/> is success.
		/// </summary>
		/// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success { get; set; }

		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		/// <value>The message.</value>
        public string Message { get; set; }

		/// <summary>
		/// Gets or sets the errors.
		/// </summary>
		/// <value>The errors.</value>
        public Dictionary<string, string> Errors { get; set; }
    }
}
