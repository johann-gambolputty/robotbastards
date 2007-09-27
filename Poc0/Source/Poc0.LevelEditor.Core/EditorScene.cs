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
		/// Sets/gets level geometry
		/// </summary>
		public LevelGeometry LevelGeometry
		{
			set
			{
				if ( m_LevelGeometry != null )
				{
					Renderables.Remove( m_LevelGeometry );
				}

				m_LevelGeometry = value;

				if ( m_LevelGeometry != null )
				{
					Renderables.Add( m_LevelGeometry );
				}
			}
			get { return m_LevelGeometry; }
		}

		/// <summary>
		/// Runtime scene
		/// </summary>
		public Scene RuntimeScene
		{
			get { return m_RuntimeScene; }
		}

		private LevelGeometry m_LevelGeometry;
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
