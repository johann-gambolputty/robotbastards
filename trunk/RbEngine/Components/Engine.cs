using System;

namespace RbEngine.Components
{
	/// <summary>
	/// Summary description for Engine.
	/// </summary>
	public class Engine
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public Engine( )
		{
		}

		/// <summary>
		/// access to the engine scene
		/// </summary>
		public Scene.SceneDb	Scene
		{
			get
			{
				return m_Scene;
			}
			set
			{
				m_Scene = value;
			}
		}

		public static Engine	Main
		{
			get
			{
				return ms_Main;
			}
		}

		Scene.SceneDb	m_Scene;

		private static Engine	ms_Main = new Engine( );
	}
}
