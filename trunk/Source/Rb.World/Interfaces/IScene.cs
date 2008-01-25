using System;
using Rb.Core.Components;

namespace Rb.World.Interfaces
{
	/// <summary>
	/// Delegate, invoked by IScene.AddObject
	/// </summary>
	/// <param name="scene">Scene that object was added to</param>
	/// <param name="id">The ID of the object that was added</param>
	/// <param name="obj">Object that was added</param>
	public delegate void SceneObjectAddedDelegate( IScene scene, Guid id, object obj );


	/// <summary>
	/// Delegate, invoked by IScene.RemoveObject
	/// </summary>
	/// <param name="scene">Scene that object was removed from</param>
	/// <param name="id">The ID of the object that was removed</param>
	/// <param name="obj">Object that was removed</param>
	public delegate void SceneObjectRemovedDelegate( IScene scene, Guid id, object obj );


	public delegate void SceneViewAttachedDelegate( IScene scene, ISceneView view );

	public delegate void SceneViewDetachedDelegate( IScene scene, ISceneView view );

	public interface ISceneView : IDisposable
	{
		event SceneViewAttachedDelegate AttachedToScene;
		event SceneViewDetachedDelegate DetachedFromScene;

		/// <summary>
		///	Gets a typed service from the view
		/// </summary>
		/// <typeparam name="T">Service type</typeparam>
		/// <returns>Returns the service, or null if no service of that type is supported</returns>
		/// <example>GetService{ILightingService}()</example>
		T GetService<T>( );
	}

	public interface IScene : IDisposable
	{
		/// <summary>
		/// Event, invoked when the scene is disposed
		/// </summary>
		event Action<IScene> Disposing;

		#region Adding and removing objects

		/// <summary>
		/// Event, invoked when an object is added to the scene
		/// </summary>
		event SceneObjectAddedDelegate ObjectAdded;

		/// <summary>
		/// Event, invoked when an object is removed from the scene
		/// </summary>
		event SceneObjectRemovedDelegate ObjectRemoved;

		/// <summary>
		/// Adds an object to this scene
		/// </summary>
		/// <param name="id">Object identifier</param>
		/// <param name="obj">Object to add</param>
		void AddObject( Guid id, object obj );

		/// <summary>
		/// Adds an object to this scene
		/// </summary>
		/// <param name="uniqueObject">Uniquely identified object</param>
		void AddObject( IUnique uniqueObject );

		/// <summary>
		/// Removes an object from the scene
		/// </summary>
		/// <param name="obj">Object to remove</param>
		void RemoveObject( object obj );

		/// <summary>
		/// Finds an object with a given ID
		/// </summary>
		/// <param name="id">Object identifier</param>
		/// <returns>The object matched to the id, or null if no such object exists</returns>
		object FindObject( Guid id );

		#endregion

		#region Scene view

		event SceneViewAttachedDelegate ViewAttached;

		event SceneViewDetachedDelegate ViewDetached;

		void AttachView( ISceneView view );

		void DetachView( ISceneView view );

		#endregion
	}
}
