using System;


namespace RbEngine.Network
{
	/// <summary>
	/// Proxy server
	/// </summary>
	public class ServerProxy : ServerBase
	{
		/// <summary>
		/// Sets up a connection to a server. Any messages that get sent to this proxy, get marshalled and forwarded to this server
		/// </summary>
		public void SetupConnection( string connectionString )
		{

		}

		/// <summary>
		/// Adds a client to the server
		/// </summary>
		/// <param name="client">Client to add</param>
		public override void AddClient( Client client )
		{
		}

		/// <summary>
		/// Removes a client from the server
		/// </summary>
		/// <param name="client">Client to remove</param>
		public override void RemoveClient( Client client )
		{
		}
	}
}
