using System;

namespace RbEngine.Network.Runt
{
	/// <summary>
	/// An update handler handles UpdateMessage messages sent to an UpdateTarget
	/// </summary>
	public interface IUpdateHandler
	{
		/// <summary>
		/// Handles an update message
		/// </summary>
		void Handle( UpdateMessage msg );
	}
}
