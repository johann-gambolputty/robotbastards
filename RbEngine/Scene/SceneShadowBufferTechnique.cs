
using System;

namespace RbEngine.Scene
{
	/// <summary>
	/// Extends ShadowBufferTechnique, providing the ShadowLights property from the scene lighting manager
	/// </summary>
	public class SceneShadowBufferTechnique : Rendering.ShadowBufferTechnique, ISceneObject
	{
		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to a scene
		/// </summary>
		public void AddedToScene( SceneDb db )
		{
			m_Scene = db;
			m_Scene.Rendering.PreRender += new RenderManager.PreRenderDelegate( PreRender );
			ShadowLights = new Rendering.LightGroup( );
		}

		/// <summary>
		/// Called when this object is removed from a scene
		/// </summary>
		public void RemovedFromScene( SceneDb db )
		{
			m_Scene			= null;
			ShadowLights	= null;
		}

		#endregion

		/// <summary>
		/// Fills the shadow light set from the scene lighting manager
		/// </summary>
		private void	PreRender( RenderManager renderMan )
		{
			if ( m_Scene == null )
			{
				return;
			}

			ILightingManager lighting = ( ILightingManager )m_Scene.Systems.FindChild( typeof( ILightingManager ) );
			if ( lighting != null )
			{
				if ( Rendering.Renderer.Inst.Camera != null )
				{
					//	Get the lights that will affect the camera most
					lighting.GetCameraLightGroup( ( Cameras.Camera3 )Rendering.Renderer.Inst.Camera, ShadowLights );
				}
				else
				{
					//	No point of reference - just get the lights closest to the origin
					lighting.GetObjectLightGroup( Maths.Point3.Origin, ShadowLights );
				}
			}
		}

		private SceneDb	m_Scene;
	}
}
