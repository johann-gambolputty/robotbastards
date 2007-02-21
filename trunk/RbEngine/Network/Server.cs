using System;

namespace RbEngine.Network
{
	/// <summary>
	/// Full-on server
	/// </summary>
	public class Server : IServer
	{
		/// <summary>
		/// Gets the scene stored on the server
		/// </summary>
		Scene.SceneDb	Scene
		{
			get { return m_Scene; } 
		}

		Scene.SceneDb	m_Scene;
	}
}
