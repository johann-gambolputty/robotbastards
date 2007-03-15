using System;

namespace RbEngine.Scene
{
	//	TODO: This is a bit of a bodge

	/// <summary>
	/// A group of lights that is determined by a SceneView
	/// </summary>
	public class SceneViewLightGroup : Rendering.LightGroup, ISceneObject, Rendering.IRender
	{
		public SceneViewLightGroup( )
		{
			Add( new Rendering.SpotLight( new Maths.Point3( 30, 30, 30 ), Maths.Point3.Origin, 55 ) );
		}


		#region ISceneObject Members

		/// <summary>
		/// Stores the specified scene, and subscribes to the 
		/// </summary>
		/// <param name="db"></param>
		public void AddedToScene( SceneDb db )
		{
			m_Scene = db;
		}

		public void RemovedFromScene( SceneDb db )
		{
			m_Scene = null;
		}

		private SceneDb	m_Scene;

		#endregion

		#region IRender Members

		public void Render( )
		{
			if ( m_Scene != null )
			{
			}
		}

		#endregion
	}
}
