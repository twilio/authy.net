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
        /// The request was unauthorized.
        /// 
        /// This could mean that an API key is wrong or it could mean that a token is incorrect.
        /// </summary>
        Unauthorized,
        /// <summary>
        /// The service is unavailable.  This usually means that the API call limit has been exceded
        /// </summary>
        ServiceUnavailable,
    }
}
