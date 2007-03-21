using System;
using System.Collections;

namespace RbEngine.Scene
{
	/// <summary>
	/// Manages lighting in a scene
	/// </summary>
	public class LightingManager : ISceneObject, ILightingManager
	{
		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to a scene
		/// </summary>
		public void			AddedToScene( SceneDb db )
		{
			m_Scene = db;
			m_Scene.Rendering.PreRender	+= new RenderManager.PreRenderDelegate( PreRender );
			m_Scene.AddedToContext += new Components.AddedToContextDelegate( OnObjectAddedToScene );
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
				//	TODO: BODGE - need proper light selection based on camera
				for ( int lightIndex = 0; lightIndex < numLights; ++lightIndex )
				{
					lighting.AddLight( ( Rendering.Light )m_Lights[ lightIndex ] );
				}
			}
		}

		/// <summary>
		/// Looks for lights and lighting data getting added to the scene
		/// </summary>
		private void		OnObjectAddedToScene( Components.IContext sceneContext, Object childObject )
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

		#region ILightingManager Members

		/// <summary>
		/// Fills a light group with lights, based on their distances from the specified camera's frustum, intensities, and other properties
		/// </summary>
		public void GetCameraLightGroup( Cameras.Camera3 camera, Rendering.LightGroup lights )
		{
			//	TODO: bodge
			GetObjectLightGroup( camera.Position, lights );
		}

		/// <summary>
		/// Fills a light group with lights, based on their distances from a specified point, intensities, and other properties
		/// </summary>
		public void GetObjectLightGroup( Maths.Point3 pos, Rendering.LightGroup lights )
		{
			//	TODO: bodge
			lights.Clear( );
			for ( int lightIndex = 0; lightIndex < m_Lights.Count; ++lightIndex )
			{
				lights.Add( ( Rendering.Light )m_Lights[ lightIndex ] );;
			}
		}

		/// <summary>
		/// Gets the array of lights in the scene
		/// </summary>
		public ArrayList Lights
		{
			get
			{
				return m_Lights;
			}
		}

		#endregion
	}
}
