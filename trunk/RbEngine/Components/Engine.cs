using System;

namespace RbEngine.Components
{
	/// <summary>
	/// Summary description for Engine.
	/// </summary>
	/// <remarks>
	/// Engine design:
	/// 
	/// ModelSet: Collection of data
	/// Scene: Collection of data instances
	/// Engine: Framework for displaying and interacting with a scene
	/// 
	/// There should be a single engine instance for each control used for rendering a scene.
	/// 
	/// Scene data
	///		- Clocks should be stored in the scene
	///		- (implies that clocks should not run off controls)
	///
	/// Engine data
	///		- 
	/// 
	/// </remarks>
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
