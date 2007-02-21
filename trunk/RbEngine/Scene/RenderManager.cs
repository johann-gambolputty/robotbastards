using System;
using System.Collections;

namespace RbEngine.Scene
{
	/// <summary>
	/// Handles rendering for a scene
	/// </summary>
	public class RenderManager
	{
		/// <summary>
		/// Attaches this manager to a scene
		/// </summary>
		/// <param name="db">Scene to attach to</param>
		public RenderManager( SceneDb db )
		{
			m_AddObjectGraphToRenderer = new Components.ChildVisitorDelegate( AddObjectGraphToRenderer );
			db.ObjectAdded += new SceneDb.ObjectAddedDelegate( OnObjectAddedToScene );
		}

		/// <summary>
		/// Renders all the stored objects
		/// </summary>
		public void Render( )
		{
			float delta = 0.0f;	//	TODO: Fill in delta appropriately
			foreach ( Scene.ISceneRenderable renderable in m_Renderables )
			{
				renderable.Render( delta );
			}
		}

		#region	Private stuff

		/// <summary>
		/// If the specified object, or any of its child objects implement the Scene.ISceneRenderable interface, they get added to this manager
		/// </summary>
		private void OnObjectAddedToScene( SceneDb scene, Object obj )
		{
			AddObjectGraphToRenderer( obj );
		}

		/// <summary>
		/// Adds an object and any child objects to this manager, if they implement the Scene.ISceneRenderable interface
		/// </summary>
		private bool AddObjectGraphToRenderer( Object obj )
		{
			if ( obj is Scene.ISceneRenderable )
			{
				m_Renderables.Add( obj );
			}

			Components.IParentObject parentObj = obj as Components.IParentObject;
			if ( parentObj != null )
			{
				parentObj.VisitChildren( m_AddObjectGraphToRenderer );
			}

			return true;
		}

		private ArrayList						m_Renderables = new ArrayList( );
		private Components.ChildVisitorDelegate	m_AddObjectGraphToRenderer;

		#endregion
	}
}
