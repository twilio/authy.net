using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Authy.Net
{
    /// <summary>
    /// The status of an authy call 
    /// </summary>
    public enum AuthyStatus
    {
        /// <summary>
        /// The call was sucessful and everything is OK
        /// </summary>
        Success,
        /// <summary>
        /// The request was invalid.  Check the fields property for more information
        /// </summary>
        BadRequest,
        /// <summary>
        /// The provide API key is invalid
        /// </summary>
        InvalidApiKey,
        /// <summary>
        /// The token for the user is invalid
        /// </summary>
        InvalidToken,
        /// <summary>
        /// The user ID provided doesn't exist
        /// </summary>
        InvalidUser,
        /// <summary>
        /// The service is unavailable.  This usually means that the API call limit has been exceded
        /// </summary>
        ServiceUnavailable,
    }
}
