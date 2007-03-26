using System;

namespace RbEngine.Network.Runt
{
	/// <summary>
	/// Interface for server updater objects
	/// </summary>
	public interface IServerUpdater
	{
		/// <summary>
		/// Adds any update messages stored by this updater to send to the server
		/// </summary>
		void GetUpdateMessages( System.Collections.ArrayList messages );
	}
}
