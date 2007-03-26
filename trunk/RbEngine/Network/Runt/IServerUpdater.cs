using System;

namespace RbEngine.Network.Runt
{
	/// <summary>
	/// Interface for server updater objects
	/// </summary>
	public interface IServerUpdater
	{
		/// <summary>
		/// Handles an update message sent from a server
		/// </summary>
		void HandleServerUpdate( UpdateMessage msg );

		/// <summary>
		/// Adds any update messages stored by this updater to send to the server
		/// </summary>
		void GetUpdateMessages( System.Collections.ArrayList messages );
	}
}
