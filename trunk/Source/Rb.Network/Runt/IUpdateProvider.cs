using System.Collections.Generic;

namespace Rb.Network.Runt
{
	/// <summary>
	/// Provides UpdateMessage objects for an UpdateSource
	/// </summary>
	public interface IUpdateProvider
	{
		/// <summary>
		/// Sets the current local sequence
		/// </summary>
		void SetLocalSequence( uint sequence );

		/// <summary>
		/// Sets the oldest target sequence
		/// </summary>
		void SetOldestTargetSequence( uint sequence );

		/// <summary>
		/// Gets all the update messages required for a target with the specified sequence
		/// </summary>
		void GetUpdateMessages( IList< UpdateMessage > messages, uint targetSequence );
	}
}
