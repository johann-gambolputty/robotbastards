using System;
using System.Collections;

namespace RbEngine.Scene
{

	//	TODO: This should store weak references to the scene objects

	/// <summary>
	/// Handles rendering for a scene
	/// </summary>
	public class RenderManager
	{
		/// <summary>
		/// Delegate used by the PreRender event
		/// </summary>
		public delegate void		PreRenderDelegate( RenderManager renderManager );

		/// <summary>
		/// Called by Render() before anything it rendered
		/// </summary>
		public PreRenderDelegate	PreRender;

		/// <summary>
		/// The array of scene renderables (ISceneRenderable interface implementors)
		/// </summary>
		public ArrayList	SceneRenderables
		{
			get
			{
				return m_SceneRenderables;
			}
		}

		/// <summary>
		/// Attaches this manager to a scene
		/// </summary>
		/// <param name="db">Scene to attach to</param>
		public						RenderManager( SceneDb db )
		{
			m_Scene = db;
			m_AddObjectGraphToRenderer = new Components.ChildVisitorDelegate( AddObjectGraphToRenderer );
			db.ObjectAddedToScene += new Components.ChildAddedDelegate( OnObjectAddedToScene );

			//	Add existing scene objects to the renderer
			db.Objects.Visit( m_AddObjectGraphToRenderer );
		}

		/// <summary>
		/// Renders all the stored objects
		/// </summary>
		public void					Render( )
		{
			if ( PreRender != null )
			{
				PreRender( this );
			}

			long curTime = TinyTime.CurrentTime;
			foreach ( Scene.ISceneRenderable renderable in m_SceneRenderables )
			{
				renderable.Render( curTime );
			}

			foreach ( Rendering.IRender renderObj in m_Renderables )
			{
				renderObj.Render( );
			}
		}

		#region	Private stuff

		/// <summary>
		/// If the specified object, or any of its child objects implement the Scene.ISceneRenderable interface, they get added to this manager
		/// </summary>
		private void OnObjectAddedToScene( Object scene, Object obj )
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
				m_SceneRenderables.Add( obj );
			}
			else if ( obj is Rendering.IRender )
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

		private SceneDb							m_Scene;
		private ArrayList						m_SceneRenderables = new ArrayList( );
		private ArrayList						m_Renderables = new ArrayList( );
		private Components.ChildVisitorDelegate	m_AddObjectGraphToRenderer;

		#endregion
	}
}
