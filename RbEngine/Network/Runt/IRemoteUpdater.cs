using System;

namespace RbEngine.Network.Runt
{
	/// <summary>
	/// Remote update interface
	/// </summary>
	public interface IRemoteUpdater : Components.IUnique
	{
		/// <summary>
		/// Sets the local sequence value. All messages retrived from this updater by GetUpdateMessages() will be tagged with this sequence
		/// </summary>
		void SetLocalSequence( uint sequence );

		/// <summary>
		/// Tells the updater what the sequence value of the most out-of-synch remote object is
		/// </summary>
		void SetOldestRemoteSequence( uint sequence );

		/// <summary>
		/// Handles an update message sent from this updater's remote counterpart
		/// </summary>
		void HandleUpdateMessage( UpdateMessage msg );

		/// <summary>
		/// Gets update messages to send to this updater's remote counterpart, which has a sequence value of remoteSequence
		/// </summary>
		void GetUpdateMessages( System.Collections.ArrayList messages, uint remoteSequence );
	}


}
