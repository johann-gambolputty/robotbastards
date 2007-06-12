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
		}

		/// <summary>
		/// Adds a renderable object (either implementing IRender or ISceneRenderable) to the render manager
		/// </summary>
		public void					AddObject( Object obj )
		{
			if ( obj is Scene.ISceneRenderable )
			{
				m_SceneRenderables.Add( obj );
			}
			else if ( obj is Rendering.IRender )
			{
				m_Renderables.Add( obj );
			}
		}

		/// <summary>
		/// Removes a renderable object from the render manager
		/// </summary>
		public void					RemoveObject( Object obj )
		{
			if ( obj is Scene.ISceneRenderable )
			{
				m_SceneRenderables.Remove( obj );
			}
			else if ( obj is Rendering.IRender )
			{
				m_Renderables.Remove( obj );
			}
		}

		/// <summary>
		/// The PreRender update step. This should be called prior to calling Render()
		/// </summary>
		public void					RenderUpdate( )
		{
			if ( PreRender != null )
			{
				PreRender( this );
			}
		}

		/// <summary>
		/// Renders all the stored objects
		/// </summary>
		public void					Render( )
		{
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

		private SceneDb		m_Scene;
		private ArrayList	m_SceneRenderables = new ArrayList( );
		private ArrayList	m_Renderables = new ArrayList( );

		#endregion
	}
}
