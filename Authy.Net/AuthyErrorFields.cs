using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Authy.Net
{
    /// <summary>
    /// An enum that indicates fields that have errors
    /// </summary>
    [Flags]
    public enum AuthyErrorFields
    {
        None = 0,
        /// <summary>
        /// The provided email is malformatted
        /// </summary>
        Email = 1,
        /// <summary>
        /// The cellphone number provided is malformatted
        /// </summary>
        Cellphone = 2,
        /// <summary>
        /// The provide API key is invalid
        /// </summary>
        ApiKey = 4,
        /// <summary>
        /// The token for the user is invalid
        /// </summary>
        Token = 8,
        /// <summary>
        /// The user ID provided doesn't exist
        /// </summary>
        User = 16,
    }
}
