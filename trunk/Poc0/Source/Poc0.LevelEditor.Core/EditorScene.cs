using System;
using Rb.World;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// The EditorScene also stores a child scene that is serialised for the game only
	/// </summary>
	[Serializable]
	public class EditorScene : Scene
	{
		/// <summary>
		/// Scene builder
		/// </summary>
		public EditorScene( )
		{
			Renderables.Add( m_LevelGeometry );
		}

		public LevelGeometry LevelGeometry
		{
			get { return m_LevelGeometry; }
		}

		/// <summary>
		/// Runtime scene
		/// </summary>
		public Scene RuntimeScene
		{
			get { return m_RuntimeScene; }
		}

		private readonly LevelGeometry m_LevelGeometry = new LevelGeometry( );
		private readonly Scene m_RuntimeScene = CreateRuntimeScene( );

		/// <summary>
		/// Creates the runtime scene
		/// </summary>
		private static Scene CreateRuntimeScene( )
		{
			Scene newScene = new Scene( );

			//	TODO: AP: Bodge - load default scene from component file
			newScene.AddService( newScene.Builder.CreateInstance( typeof( LightingManager ) ) );

			return newScene;
		}
	}
}
