using System;

namespace RbEngine.Network.Runt
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
		/// Informs this updater that it must be able to create update messages for a new client with the specified index
		/// </summary>
		/// <param name="clientIndex">New client's index</param>
		void AddNewClient( int clientIndex );

		/// <summary>
		/// Creates a message that updates a network client
		/// </summary>
		/// <param name="clientIndex">The index of the client</param>
		/// <param name="currentClientState">The sequence number of the current client state. If this is -1, the client has (apparently) not received states from the server</param>
		/// <returns>New update messages</returns>
		Components.Message[] CreateUpdateMessages( int clientIndex, int clientSequence, int serverSequence );
	}
}
