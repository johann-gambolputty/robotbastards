using System;

namespace RbEngine.Scene
{
	/// <summary>
	/// Stores objects and managers that make up a scene
	/// </summary>
	public class SceneDb
	{
		#region	Scene managers and services

		/// <summary>
		/// Returns the rendering manager for this scene
		/// </summary>
		public RenderManager	Rendering
		{
			get
			{
				return m_Rendering;
			}
		}
        
		#endregion

		#region	Scene building

		public SceneDb( )
		{
			m_Rendering = new RenderManager( this );
		}

		/// <summary>
		/// All the objects making up the scene
		/// </summary>
		public ObjectSet	Objects
		{
			get
			{
				return m_Objects;
			}
		}

		/// <summary>
		/// Adds an object to the scene
		/// </summary>
		/// <param name="obj">Object to add</param>
		/// <remarks>
		/// Invokes the ObjectAdded event. If obj implements the ISceneEvents interface, it gets ISceneEvents.AddedToScene() called
		/// </remarks>
		public void Add( Object obj )
		{
			Objects.Add( obj );
			
			if ( ObjectAdded != null )
			{
				ObjectAdded( this, obj );
			}

			ISceneEvents events = obj as ISceneEvents;
			if ( events != null )
			{
				events.AddedToScene( this );
			}
		}

		/// <summary>
		/// Removes an object to the scene
		/// </summary>
		/// <param name="obj">Object to remove</param>
		/// <remarks>
		/// Invokes the ObjectRemoved event. If obj implements the ISceneEvents interface, it gets ISceneEvents.RemovedFromScene() called
		/// </remarks>
		public void Remove( Object obj )
		{
			Objects.Remove( obj );

			if ( ObjectRemoved != null )
			{
				ObjectRemoved( this, obj );
			}

			ISceneEvents events = obj as ISceneEvents;
			if ( events != null )
			{
				events.RemovedFromScene( this );
			}
		}

		#endregion

		#region	Scene building events

		/// <summary>
		/// Delegate type used by ObjectRemoved
		/// </summary>
		public delegate void				ObjectRemovedDelegate( SceneDb scene, Object obj );
		
		/// <summary>
		/// Delegate type used by ObjectAdded
		/// </summary>
		public delegate void				ObjectAddedDelegate( SceneDb scene, Object obj );

		/// <summary>
		/// Event invoked when an object is removed from the scene by the Remove() method
		/// </summary>
		public event ObjectRemovedDelegate	ObjectRemoved;

		/// <summary>
		/// Event invoked when an object is added to the scene by the Add() method
		/// </summary>
		public event ObjectAddedDelegate	ObjectAdded;

		#endregion



		private ObjectSet		m_Objects = new TypeGraphObjectSet( );
		private RenderManager	m_Rendering;
	}
}
