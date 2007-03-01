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
			foreach ( Rendering.LightingGroup group in m_LightingGroups )
			{
				group.ClearLights( );
				//	TODO: BODGE
				for ( int lightIndex = 0; lightIndex < numLights; ++lightIndex )
				{
					group.AddLight( ( Rendering.Light )m_Lights[ lightIndex ] );
				}
			}
		}

		private void		OnObjectAddedToScene( Object parentObject, Object childObject )
		{
			if ( childObject is Rendering.Light )
			{
				m_Lights.Add( childObject );
			}
			else
			{
				Components.IParentObject parent = childObject as Components.IParentObject;
				if ( parent != null )
				{
					Rendering.LightingGroup group = new Rendering.LightingGroup( childObject );
					parent.AddChild( group );
					m_LightingGroups.Add( group );
				}
			}
		}

		#region	Private stuff

		private ArrayList	m_Lights			= new ArrayList( );
		private ArrayList	m_LightingGroups	= new ArrayList( );
		private SceneDb		m_Scene;

		#endregion
	}
}
