using System;
using System.Collections.Generic;

namespace Authy.Net
{
	/// <summary>
	/// The result of a request of OneTouch
	/// </summary>
	public class OneTouchResult : AuthyResult
	{
		/// <summary>
		/// The approval UUID returned
		/// </summary>
		/// <value>The approval request.</value>
		public Dictionary<string, string> Approval_Request
		{
			get;set;
		}
	}
}
