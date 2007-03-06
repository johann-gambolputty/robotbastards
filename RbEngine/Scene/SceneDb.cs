using System;
using System.Collections;

namespace RbEngine.Scene
{
	/*
	 * Client-scene separation
	 * 
	 * Use case
	 *	Controlling 2 or more objects at once
	 *		- All controlled objects listen for server command messages
	 *		- Controlled objects add their own movement request messages to internal message queues for processing
	 * 
	 * Use case
	 *	Selecting an object in an editor
	 *		- All editable objects have a basic command listener that look for selection commands
	 *		- If an object is selected, it adds a new command listener to its parent object that listens for context specific editing commands
	 *			- It can either listen to the server, or add itself to some selection set?
	 *			- There needs to be a way for it to inform the client of its editing context?
	 *		- If the basic command listener spots a selection command that does not select its parent, it should destroy the context specific command listener
	 * 
	 * Use case
	 *	For some 3d scenes that contain lights, objects in the scene need to be lit by those lights. Each object can have up to 3 lights affecting them.
	 *	Entity render order could be partially determined by lights (order by light/material/state/texture/depth). Objects should not have to explicitly
	 *  support lighting. There's only support for a limited number of active lights in d3d/opengl (8).
	 *		- Each scene can get a lighting manager (added as a child object? child to rendering manager? child to Systems component?)
	 *		- The lighting manager does a slow update that determines which lights affect which object
	 *			- Each unique combination of lights on an object adds a new LightingGroup to the manager (RenderFactory.NewLightingGroup())
	 *		- The renderer must be aware of this information, and render each LightingGroup, instead of individual objects
	 *		- This breaks the requirement that the renderer be able to apply different orderings to rendered objects
	 *
	 */

	/// <summary>
	/// Stores objects and managers that make up a scene
	/// </summary>
	/// <remarks>
	/// The purpose of the scene is to manage a set of arbitrary objects, for rendering and updating purposes. It is intended to be viewer
	/// independent - any viewer dependent code like cameras must be contained in Network.Client objects.
	/// Rendering the scene is done through the scene's RenderManager (<see cref="Rendering"/> property). Interacting with objects in the
	/// scene is a bit more complicated - it's achieved by sending CommandMessage messages directly to an object in the scene (usually a controller
	/// that is attached to an entity), or to a forwarding object, that scene objects listen to (usually the server).
	/// </remarks>
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

		#region	Scene server

		/// <summary>
		/// Server that this scene belongs to
		/// </summary>
		public Network.ServerBase	Server
		{
			set
			{
				m_Server = value;
			}
			get
			{
				return m_Server;
			}
		}

		#endregion

		#region	Scene systems

		/// <summary>
		/// Returns the rendering manager for this scene
		/// </summary>
		public RenderManager		Rendering
		{
			get
			{
				return m_Rendering;
			}
		}

		/// <summary>
		/// Scene systems
		/// </summary>
		public Components.Node	Systems
		{
			get
			{
				return m_Systems;
			}
			set
			{
				m_Systems = value;

				if ( m_Systems != null )
				{
					OnObjectGraphAdded( this, m_Systems );
				}
			}
		}

		/// <summary>
		/// Helper to get a system of a given type
		/// </summary>
		public Object	GetSystem( Type systemType )
		{
			return Systems.FindChild( systemType );
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
			m_Rendering		= new RenderManager( this );
			m_OnChildAdded	= new Components.ChildAddedDelegate( OnChildObjectAdded );
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

		#endregion


		#region IParentObject Members

		/// <summary>
		/// Event, invoked when a child object is added to the scene (even if that object is not a direct descendant of the scene)
		/// </summary>
		public event Components.ChildAddedDelegate		ObjectAddedToScene;

		/// <summary>
		/// Event, invoked by AddChild() after a child object has been added
		/// </summary>
		public event Components.ChildAddedDelegate		ChildAdded;

		/// <summary>
		/// Event, invoked by AddChild() before a child object has been removed
		/// </summary>
		public event Components.ChildRemovedDelegate	ChildRemoved;

		/// <summary>
		/// Gets the child collection
		/// </summary>
		public ICollection					Children
		{
			get
			{
				return Objects;
			}
		}

		/// <summary>
		/// Adds an object to the scene
		/// </summary>
		/// <param name="childObject">Object to add</param>
		/// <remarks>
		/// Invokes the ChildAdded event, the ObjectAddedToScene event, and ISceneObject.AddedToScene(), if childObject implements that interface
		/// </remarks>
		public void AddChild( Object childObject )
		{
			Objects.Add( childObject );

			Components.IChildObject childInterface = childObject as Components.IChildObject;
			if ( childInterface != null )
			{
				childInterface.AddedToParent( this );
			}

			if ( ChildAdded != null )
			{
				ChildAdded( this, childObject );
			}

			OnObjectGraphAdded( this, childObject );
		}

		/// <summary>
		/// Removes an object to the scene
		/// </summary>
		/// <param name="childObject">Object to remove</param>
		/// <remarks>
		/// Invokes the ChildRemoved event. If obj implements the ISceneObject interface, it gets ISceneObject.RemovedFromScene() called
		/// </remarks>
		public void RemoveChild( Object childObject )
		{
			Objects.Remove( childObject );

			if ( ChildRemoved != null )
			{
				ChildRemoved( this, childObject );
			}

			ISceneObject events = childObject as ISceneObject;
			if ( events != null )
			{
				events.RemovedFromScene( this );
			}
		}

		/// <summary>
		/// Visits all objects in the scene
		/// </summary>
		/// <param name="visitor">Visitor function</param>
		public void VisitChildren( Components.ChildVisitorDelegate visitor )
		{
			Objects.Visit( visitor );
		}

		private void OnObjectGraphAdded( Object parentObject, Object rootObject )
		{
			OnChildObjectAdded( parentObject, rootObject );

			Components.IParentObject rootParentObject = rootObject as Components.IParentObject;
			if ( ( rootParentObject != null ) && ( rootParentObject.Children != null ) )
			{
				foreach ( Object curChildObject in rootParentObject.Children )
				{
					OnObjectGraphAdded( rootObject, curChildObject );
				}
			}
		}

		private void OnChildObjectAdded( Object parentObject, Object childObject )
		{
			if ( childObject is Components.IParentObject )
			{
				( ( Components.IParentObject )childObject ).ChildAdded += m_OnChildAdded;
			}

			if ( childObject is ISceneObject )
			{
				( ( ISceneObject )childObject ).AddedToScene( this );
			}

			if ( ObjectAddedToScene != null )
			{
				ObjectAddedToScene( parentObject, childObject );
			}
		}

		#endregion

		#region IXmlLoader Members

		/// <summary>
		/// Parses the element that generated this scene
		/// </summary>
		public void ParseGeneratingElement( System.Xml.XmlElement element )
		{
		}

		/// <summary>
		/// Parses an element
		/// </summary>
		public bool ParseElement( System.Xml.XmlElement element )
		{
			if ( element.Name == "clock" )
			{
				m_Clocks.Add( new Clock( element.GetAttribute( "name" ), ( int )( 1000.0f / float.Parse( element.GetAttribute( "ticksPerSecond" ) ) ) ) );
				return true;
			}

			return false;
		}

		#endregion

		private ObjectSet						m_Objects	= new TypeGraphObjectSet( );
		private RenderManager					m_Rendering;
		private ArrayList						m_Clocks	= new ArrayList( );
		private bool							m_Paused	= true;
		private Components.Node					m_Systems;
		private Components.ChildAddedDelegate	m_OnChildAdded;
		private Network.ServerBase				m_Server;
	}
}