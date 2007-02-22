using System;
using System.Collections;

namespace RbEngine.Scene
{
	/// <summary>
	/// Stores objects and managers that make up a scene
	/// </summary>
	public class SceneDb : Components.IParentObject, Components.IXmlLoader
	{
		#region	Scene clocks

		/// <summary>
		/// Pauses or unpauses the scene clocks
		/// </summary>
		public bool				Pause
		{
			get
			{
				return m_Paused;
			}
			set
			{
				m_Paused = value;
				foreach ( Clock curClock in m_Clocks )
				{
					curClock.Pause = value;
				}
			}
		}

		/// <summary>
		/// Finds a scene clock by name
		/// </summary>
		/// <param name="clockName">Name of the clock</param>
		/// <returns></returns>
		public Clock			GetNamedClock( string clockName )
		{
			foreach ( Clock curClock in m_Clocks )
			{
				if ( curClock.Name == clockName )
				{
					return curClock;
				}
			}
			return null;
		}

		#endregion

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

		/// <summary>
		/// Default constructor. Adds a rendering manager and default clocks
		/// </summary>
		/// <remarks>
		/// The scene clocks are initially paused
		/// </remarks>
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

		#region IParentObject Members

		/// <summary>
		/// Adds an object to the scene
		/// </summary>
		/// <param name="childObject">Object to add</param>
		public void AddChild( Object childObject )
		{
			Add( childObject );
		}

		/// <summary>
		/// Visits all objects in the scene
		/// </summary>
		/// <param name="visitor">Visitor function</param>
		public void VisitChildren( Components.ChildVisitorDelegate visitor )
		{
			Objects.Visit( visitor );
		}

		#endregion

		private ObjectSet		m_Objects = new TypeGraphObjectSet( );
		private RenderManager	m_Rendering;
		private ArrayList		m_Clocks	= new ArrayList( );
		private bool			m_Paused	= true;

		#region IXmlLoader Members

		/// <summary>
		/// Parses the element that generated this scene
		/// </summary>
		/// <param name="reader">XML reader pointing to the element</param>
		public void ParseGeneratingElement( System.Xml.XmlReader reader )
		{
		}

		/// <summary>
		/// Parses an element
		/// </summary>
		/// <param name="reader">XML reader pointing to the element</param>
		public void ParseElement( System.Xml.XmlReader reader )
		{
			if ( reader.Name == "clock" )
			{
				m_Clocks.Add( new Clock( reader.GetAttribute( "name" ), ( int )( 100.0f / float.Parse( reader.GetAttribute( "ticksPerSecond" ) ) ) ) );
			}
			else
			{
				throw new ApplicationException( "Unknown element in scene" );
			}
		}

		#endregion
	}
}
