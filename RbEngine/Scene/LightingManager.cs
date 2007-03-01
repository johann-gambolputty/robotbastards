using System;
using System.Collections;

namespace RbEngine.Scene
{
	/// <summary>
	/// Manages lighting in a scene
	/// </summary>
	public class LightingManager : ISceneObject
	{
		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to a scene
		/// </summary>
		public void			AddedToScene( SceneDb db )
		{
			m_Scene = db;
			m_Scene.Rendering.PreRender	+= new RenderManager.PreRenderDelegate( PreRender );
			m_Scene.ObjectAddedToScene	+= new Components.ChildAddedDelegate( OnObjectAddedToScene );
		}

		/// <summary>
		/// Called when this object is added to a scene
		/// </summary>
		public void			RemovedFromScene( SceneDb db )
		{
			m_Scene = null;
		}

		#endregion

		/// <summary>
		/// Sets up lighting information
		/// </summary>
		private void		PreRender( RenderManager renderMan )
		{
			int numLights = Maths.Utils.Min( m_Lights.Count, 3 ); 
			foreach ( LightingData lighting in m_LightingData )
			{
				lighting.ClearLights( );
				//	TODO: BODGE
				for ( int lightIndex = 0; lightIndex < numLights; ++lightIndex )
				{
					lighting.AddLight( ( Rendering.Light )m_Lights[ lightIndex ] );
				}
			}
		}

		/// <summary>
		/// Looks for lights and lighting data getting added to the scene
		/// </summary>
		private void		OnObjectAddedToScene( Object parentObject, Object childObject )
		{
			if ( childObject is Rendering.Light )
			{
				m_Lights.Add( childObject );
			}
			else
			{
				LightingData lighting = childObject as LightingData;
				if ( lighting != null )
				{
					m_LightingData.Add( lighting );
				}
			}
		}

		#region	Private stuff

		private ArrayList	m_Lights		= new ArrayList( );
		private ArrayList	m_LightingData	= new ArrayList( );
		private SceneDb		m_Scene;

		#endregion
	}
}
