

namespace Rb.World
{
	/// <summary>
	/// Delegate, used by the SceneObjects.OnObjectAdded event
	/// </summary>
	public delegate void OnSceneObjectAddedDelegate( Scene scene, ISceneObject obj );

	/// <summary>
	/// Delegate, used by the SceneObjects.OnObjectRemoved event
	/// </summary>
	public delegate void OnSceneObjectRemovedDelegate( Scene scene, ISceneObject obj );

	/// <summary>
	/// Stores a set of objects for a particular scene
	/// </summary>
	public class SceneObjects
	{

		#region	Public construction

		/// <summary>
		/// Sets the scene (haha)
		/// </summary>
		/// <param name="scene">Scene owner</param>
		public SceneObjects( Scene scene )
		{
			m_Scene = scene;
		}

		#endregion

		#region	Public methods

		/// <summary>
		/// Adds a scene object
		/// </summary>
		/// <param name="object">Object to add</param>
		public void Add( ISceneObject obj )
		{
			if ( OnObjectAdded != null )
			{
				OnObjectAdded( m_Scene, obj );
			}
		}

		/// <summary>
		/// Removes a scene object
		/// </summary>
		/// <param name="object">Object to remove</param>
		public void Remove( ISceneObject obj )
		{
			if ( OnObjectRemoved != null )
			{
				OnObjectRemoved( m_Scene, obj );
			}
		}

		/// <summary>
		/// Finds a given object in the scene
		/// </summary>
		/// <param name="key">Object key</param>
		/// <returns>The object with a matching key, or null if on could not be found</returns>
		public object Find( object key )
		{
			return null;
		}

		#endregion

		#region	Public events

		/// <summary>
		/// Event, invoked by Add() when a new scene object is added
		/// </summary>
		public event OnSceneObjectAddedDelegate OnObjectAdded;

		/// <summary>
		/// Event, invoked by Remove() when a scene object is removed
		/// </summary>
		public event OnSceneObjectRemovedDelegate OnObjectRemoved;

		#endregion

		#region Private stuff

		private Scene m_Scene;

		#endregion
	}
}