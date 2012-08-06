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
        /// Boolean value indicating a succesful registration
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// The raw response returned from the API
        /// </summary>
        public string RawResponse { get; set; }
    }
}
