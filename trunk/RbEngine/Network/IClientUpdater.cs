using System;

namespace RbEngine.Network
{
	/// <summary>
	/// Interface for updating a network client
	/// </summary>
	/// <remarks>
	/// updater must subscribe to a ClientUpdateManager
	/// </remarks>
	public interface IClientUpdater
	{
		/// <summary>
		/// Creates a message that updates a network client
		/// </summary>
		/// <returns>New update messages</returns>
		Components.Message[] CreateUpdateMessages( );
	}
}
