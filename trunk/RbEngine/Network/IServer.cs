using System;

namespace RbEngine.Network
{
	/// <summary>
	/// Server interface
	/// </summary>
	public interface IServer
	{
		/// <summary>
		/// Adds a client to the server
		/// </summary>
		/// <param name="client">Client to add</param>
		void AddClient( Client client );

		/// <summary>
		/// Removes a client from the server
		/// </summary>
		/// <param name="client">Client to remove</param>
		void RemoveClient( Client client );

		/// <summary>
		/// Gets the scene stored on the server
		/// </summary>
		Scene.SceneDb	Scene
		{
			set; //TODO: REMOVEME?
			get;
		}
	}
}
