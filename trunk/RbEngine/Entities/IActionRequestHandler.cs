using System;

namespace RbEngine.Entities
{
	/// <summary>
	/// Summary description for IActionRequestHandler.
	/// </summary>
	public interface IActionRequestHandler
	{
		/// <summary>
		/// Access to the pending request
		/// </summary>
		ActionRequest PendingRequest
		{
			set;
			get;
		}
	}
}
