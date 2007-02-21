using System;

namespace RbEngine.Network
{
	/// <summary>
	/// Proxy server
	/// </summary>
	public class ServerProxy
	{
		/// <summary>
		/// Gets the scene stored on the server
		/// </summary>
		public Scene.SceneDb	Scene
		{
			get { return m_Scene; } 
		}

		private Scene.SceneDb	m_Scene;
	}
}
