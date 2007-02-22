using System;

namespace RbEngine.Network
{
	/// <summary>
	/// Full-on server
	/// </summary>
	public class Server : IServer
	{
		/// <summary>
		/// Adds a client to the server
		/// </summary>
		/// <param name="client">Client to add</param>
		public void AddClient( Client client )
		{
		}

		/// <summary>
		/// Removes a client from the server
		/// </summary>
		/// <param name="client">Client to remove</param>
		public void RemoveClient( Client client )
		{
		}

		/// <summary>
		/// Gets the scene stored on the server
		/// </summary>
		public Scene.SceneDb	Scene
		{
			set { m_Scene = value; }
			get { return m_Scene; } 
		}

		Scene.SceneDb			m_Scene;
	}
}
