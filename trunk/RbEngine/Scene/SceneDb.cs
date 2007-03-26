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
	public class SceneDb : Components.IParentObject, Components.IXmlLoader, Components.IContext, IDisposable
	{
		#region	Dispose event

		/// <summary>
		/// Delegate, used by the Disposing event
		/// </summary>
		public delegate void			DisposingDelegate( );

		/// <summary>
		/// Event, invoked when the scene is about to be destroyed
		/// </summary>
		public event DisposingDelegate	Disposing;

		#endregion

		~SceneDb( )
		{
			Dispose( );
		}

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
			m_Rendering = new RenderManager( this );
		}

		/// <summary>
		/// Gets a scene object from its identifier
		/// </summary>
		public Object GetSceneObjectById( Components.ObjectId id )
		{
			return m_ChildIdMap[ id ];
		}

		#endregion

		#region IParentObject Members

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
				return m_Children;
			}
		}

		/// <summary>
		/// Adds an object to the scene
		/// </summary>
		/// <param name="childObject">Object to add</param>
		/// <remarks>
		/// Invokes the ChildAdded event
		/// </remarks>
		public void AddChild( Object childObject )
		{
			m_Children.Add( childObject );
			
			if ( ChildAdded != null )
			{
				ChildAdded( this, childObject );
			}
		}

		/// <summary>
		/// Removes an object to the scene
		/// </summary>
		/// <param name="childObject">Object to remove</param>
		/// <remarks>
		/// Invokes the ChildRemoved event
		/// </remarks>
		public void RemoveChild( Object childObject )
		{
			m_Children.Remove( childObject );

			if ( ChildRemoved != null )
			{
				ChildRemoved( this, childObject );
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

		#region	Private stuff

		private ArrayList						m_Children		= new ArrayList( );
		private RenderManager					m_Rendering;
		private ArrayList						m_Clocks		= new ArrayList( );
		private bool							m_Paused		= true;
		private Components.Node					m_Systems;
		private Hashtable						m_ChildIdMap	= new Hashtable( );

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Fires the Disposing event
		/// </summary>
		public void Dispose( )
		{
			if ( Disposing != null )
			{
				Disposing( );
				Disposing = null;
			}
		}

		#endregion

		#region	IContext Members

		/// <summary>
		/// Event, invoked by AddToContext()
		/// </summary>
		public event Components.AddedToContextDelegate		AddedToContext;

		/// <summary>
		/// Event, invoked by RemoveFromContext()
		/// </summary>
		public event Components.RemovedFromContextDelegate	RemovedFromContext;

		/// <summary>
		/// Adds an item to this context
		/// </summary>
		public void AddToContext( Object obj )
		{
			//	Inform ISceneObject objects that they've been added to the scene
			ISceneObject sceneObj = obj as ISceneObject;
			if ( sceneObj != null )
			{
				sceneObj.AddedToScene( this );
			}

			//	Add IUnique objects the child ID map
			Components.IUnique uniqueObj = obj as Components.IUnique;
			if ( uniqueObj != null )
			{
				m_ChildIdMap[ uniqueObj.Id ] = obj;
			}

			//	Invoke the AddedToContext event
			if ( AddedToContext != null )
			{
				AddedToContext( this, obj );
			}
		}

		/// <summary>
		/// Removes an item from this context
		/// </summary>
		/// <param name="obj"></param>
		public void RemoveFromContext( Object obj )
		{
			//	Inform ISceneObject objects that they've been removed from the scene
			ISceneObject sceneObj = obj as ISceneObject;
			if ( sceneObj != null )
			{
				sceneObj.RemovedFromScene( this );
			}

			//	Remove IUnique objects the child ID map
			Components.IUnique uniqueObj = obj as Components.IUnique;
			if ( uniqueObj != null )
			{
				m_ChildIdMap.Remove( uniqueObj.Id );
			}

			//	Invoke the RemovedFromContext event
			if ( RemovedFromContext != null )
			{
				RemovedFromContext( this, obj );
			}
		}

		#endregion
	}
}
