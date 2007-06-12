

namespace Rb.World
{

	/// <summary>
	/// Delegate, used by the SceneViewers.OnViewerAdded event
	/// </summary>
	public delegate void OnSceneViewerAddedDelegate( Scene scene, ISceneViewer viewer );

	/// <summary>
	/// Delegate, used by the SceneViewers.OnViewerRemoved event
	/// </summary>
	public delegate void OnSceneViewerRemovedDelegate( Scene scene, ISceneViewer viewer );

	/// <summary>
	/// Stores a set of viewers for a particular scene
	/// </summary>
	public class SceneViewers
	{
		#region	Public construction

		/// <summary>
		/// Sets the scene (haha)
		/// </summary>
		/// <param name="scene">Scene owner</param>
		public SceneViewers( Scene scene )
		{
			m_Scene = scene;
		}

		#endregion

		#region	Public methods

		/// <summary>
		/// Adds a scene viewer
		/// </summary>
		/// <param name="viewer">Viewer to add</param>
		public void Add( ISceneViewer viewer )
		{
			if ( OnViewerAdded != null )
			{
				OnViewerAdded( m_Scene, viewer );
			}
		}

		/// <summary>
		/// Removes a scene viewer
		/// </summary>
		/// <param name="viewer">Viewer to remove</param>
		public void Remove( ISceneViewer viewer )
		{
			if ( OnViewerRemoved != null )
			{
				OnViewerRemoved( m_Scene, viewer );
			}
		}

		#endregion

		#region	Public events

		/// <summary>
		/// Event, invoked by Add() when a new scene viewer is added
		/// </summary>
		public event OnSceneViewerAddedDelegate OnViewerAdded;

		/// <summary>
		/// Event, invoked by Remove() when a scene viewer is removed
		/// </summary>
		public event OnSceneViewerRemovedDelegate OnViewerRemoved;

		#endregion

		#region Private stuff

		private Scene m_Scene;

		#endregion
	}
}