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
		/// Gets the unique ID of this updater (usually the ID of the parent object)
		/// </summary>
		Components.ObjectId Id
		{
			get;
		}

		/// <summary>
		/// Handles an update message sent from a client
		/// </summary>
		void HandleClientUpdate( UpdateMessage msg );

		/// <summary>
		/// Sets the oldest client sequence value
		/// </summary>
		/// <remarks>
		/// Set by ClientUpdateManager. This is the sequence value of the least up-to-date client connected to the server.
		/// </remarks>
		void SetOldestClientSequence( uint sequence );

		/// <summary>
		/// Creates a series of messages that updates a network client
		/// </summary>
		void GetUpdateMessages( System.Collections.ArrayList messages, uint clientSequence, uint serverSequence );

	}
}
