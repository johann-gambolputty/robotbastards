

namespace Rb.Core.World
{

	/// <summary>
	/// Delegate, used by the SceneControllers.OnControllerAdded event
	/// </summary>
	public delegate void OnSceneControllerAddedDelegate( Scene scene, ISceneController controller );

	/// <summary>
	/// Delegate, used by the SceneControllers.OnControllerRemoved event
	/// </summary>
	public delegate void OnSceneControllerRemovedDelegate( Scene scene, ISceneController controller );

	/// <summary>
	/// Stores a set of controllers for a particular scene
	/// </summary>
	public class SceneControllers
	{

		#region	Public construction

		/// <summary>
		/// Sets the scene (haha)
		/// </summary>
		/// <param name="scene">Scene owner</param>
		public SceneControllers( Scene scene )
		{
			m_Scene = scene;
		}

		#endregion

		#region	Public methods

		/// <summary>
		/// Adds a scene controller
		/// </summary>
		/// <param name="controller">Controller to add</param>
		public void Add( ISceneController controller )
		{
			if ( OnControllerAdded != null )
			{
				OnControllerAdded( m_Scene, controller );
			}
		}

		/// <summary>
		/// Removes a scene controller
		/// </summary>
		/// <param name="controller">Controller to remove</param>
		public void Remove( ISceneController controller )
		{
			if ( OnControllerRemoved != null )
			{
				OnControllerRemoved( m_Scene, controller );
			}
		}

		#endregion

		#region	Public events

		/// <summary>
		/// Event, invoked by Add() when a new scene controller is added
		/// </summary>
		public event OnSceneControllerAddedDelegate OnControllerAdded;

		/// <summary>
		/// Event, invoked by Remove() when a scene controller is removed
		/// </summary>
		public event OnSceneControllerRemovedDelegate OnControllerRemoved;

		#endregion


		#region Private stuff

		private Scene m_Scene;

		#endregion
	}
}