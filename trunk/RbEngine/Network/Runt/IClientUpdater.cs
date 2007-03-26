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
		/// Sets the oldest client sequence value
		/// </summary>
		/// <remarks>
		/// Set by ClientUpdateManager. This is the sequence value of the least up-to-date client connected to the server.
		/// </remarks>
		int OldestClientSequence
		{
			set;
		}

		/// <summary>
		/// Creates a message that updates a network client
		/// </summary>
		/// <param name="currentClientState">The sequence number of the current client state. If this is -1, the client has (apparently) not received states from the server</param>
		/// <returns>New update messages</returns>
		Components.Message[] CreateUpdateMessages( int clientSequence, int serverSequence );
	}
}
