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
        Email = 1,
        Cellphone = 2
    }
}
