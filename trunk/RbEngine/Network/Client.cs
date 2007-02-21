using System;

namespace RbEngine.Network
{
	/*
	 * Local client-server model
	 * 
	 * Client <n:1> Server
	 * 
	 * Remote client-server model
	 * 
	 * Client <n:1> ServerProxy <1:1> Server
	 *
	 * Clients are associated with individual controls
	 */

	/// <summary>
	/// Contains view dependent stuff and interaction. Communicates with server or serverproxy via IServer interface.
	/// </summary>
	public class Client
	{
		/// <summary>
		/// Associates the client with a given control
		/// </summary>
		/// <param name="control">Associated control</param>
		public Client( System.Windows.Forms.Control control )
		{
		}

		/// <summary>
		/// Gets the client camera
		/// </summary>
		public Cameras.CameraBase	Camera
		{
			get;
			set;
		}
	}
}
