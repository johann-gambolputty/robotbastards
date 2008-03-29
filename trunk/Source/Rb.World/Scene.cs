using System;
using System.Collections;
using System.Collections.Generic;
using Rb.Core.Components;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.World
{
	/// <summary>
	/// Manages all objects in the scene, and the services that tend to them
	/// </summary>
	[Serializable]
    public class Scene : IRenderable, IDisposable
	{
		#region Construction

		/// <summary>
		/// Default constructor
		/// </summary>
		public Scene( )
		{
			m_Objects = new SceneObjectMap( this );
		}

		#endregion

		#region IDisposable members

		/// <summary>
		/// Event, raised by <see cref="Dispose()"/>
		/// </summary>
		public event EventHandler Disposing;

		/// <summary>
		/// Disposes of the scene, raises the <see cref="Disposing"/> event
		/// </summary>
		public void Dispose( )
		{
			if ( !m_Disposed )
			{
				if ( Disposing != null )
				{
					Disposing( this, null );
				}
				m_Disposed = true;
			}
		}

		#endregion

		#region	Services

		/// <summary>
		/// Returns all the services
		/// </summary>
		public IEnumerable Services
		{
			get { return m_AllServices; }
		}

		/// <summary>
		/// Gets a typed service
		/// </summary>
		/// <param name="serviceType">Service type</param>
		/// <returns>Returns the service object. Returns null if the service does not exist</returns>
		public object GetService( Type serviceType )
		{
            object result;
			return m_Services.TryGetValue( serviceType, out result ) ? result : null;
		}

		/// <summary>
		/// Gets a typed service
		/// </summary>
		/// <typeparam name="ServiceType">Service type</typeparam>
		/// <returns>Returns the typed service object. Returns null if the service does not exist</returns>
		public ServiceType GetService<ServiceType>( )
		{
			return ( ServiceType )GetService( typeof( ServiceType ) );
		}

		/// <summary>
		/// Adds a named service
		/// </summary>
		/// <param name="service">Service object</param>
		public void AddService( object service )
		{
			m_AllServices.Add( service );

			AddService( service.GetType( ), service );

            //  Also register the service with all the interfaces that it supports
            foreach ( Type interfaceType in service.GetType( ).GetInterfaces( ) )
            {
                AddService( interfaceType, service );
            }

			ISceneObject sceneService = service as ISceneObject;
			if ( sceneService != null )
			{
				sceneService.AddedToScene( this );
			}
		}

        /// <summary>
        /// Adds a list of services to the scene
        /// </summary>
        /// <param name="services">Scene services</param>
        public void AddServices( ICollection services )
        {
            foreach( object service in services )
            {
                AddService( service );
            }
        }

		/// <summary>
		/// Removes a service
		/// </summary>
		/// <param name="service">Service to remove</param>
		public void RemoveService( object service )
		{
			if ( service == null )
			{
				return;
			}
			m_AllServices.Remove( service );

			Type key = service.GetType( );
			if ( !m_Services.ContainsKey( key ) )
			{
				WorldLog.Warning( "Failed to find service of type \"{0}\"", key );
			}
			else
			{
				m_Services.Remove( key );

				ISceneObject sceneService = service as ISceneObject;
				if ( sceneService != null )
				{
					sceneService.RemovedFromScene( this );
				}
			}
		}

		#endregion

		#region	Public properties

        /// <summary>
        /// Gets the list of renderable objects in the scene
        /// </summary>
        public RenderableList Renderables
        {
            get { return m_Renderables; }
        }

		/// <summary>
		/// Gets the scene object set
		/// </summary>
		public ObjectMap Objects
		{
			get { return m_Objects; }
		}

		#endregion

        #region IRenderable Members

		/// <summary>
		/// Delegate, used by PreRender
		/// </summary>
		public delegate void RenderEventDelegate( Scene scene );

		/// <summary>
		/// Pre-render event, invoked by Render() before anything gets rendered
		/// </summary>
		public event RenderEventDelegate PreRender;

        /// <summary>
        /// Renders the scene
        /// </summary>
        /// <param name="context">Render context</param>
        public void Render( IRenderContext context )
        {
			if ( PreRender != null )
			{
				PreRender( this );
			}
            m_Renderables.Render( context );
        }

        #endregion

		#region	Private stuff

		private readonly ArrayList					m_AllServices	= new ArrayList( );
		private readonly Dictionary< Type, object >	m_Services	    = new Dictionary< Type, object >( );
		private readonly ObjectMap					m_Objects;
        private readonly RenderableList             m_Renderables   = new RenderableList( );

		[NonSerialized]
		private bool m_Disposed = false;

        /// <summary>
        /// Associates a service with a key
        /// </summary>
        /// <param name="key">Service type key</param>
        /// <param name="service">Service object</param>
        private void AddService( Type key, object service )
        {
			WorldLog.Info( "Adding service of type \"{0}\"", key );

			if ( m_Services.ContainsKey( key ) )
			{
				WorldLog.Warning( "Service of type \"{0}\" already existed - overwriting", key );
			}

            m_Services[ key ] = service;
        }

		#endregion

		#region SceneObjectMap class

		/// <summary>
		/// Extends <see cref="ObjectMap"/>. Calls into the <see cref="ISceneObject"/> interface when adding/removing objects
		/// </summary>
		[Serializable]
		private class SceneObjectMap : ObjectMap
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			/// <param name="scene">Associated scene</param>
			public SceneObjectMap( Scene scene )
			{
				m_Scene = scene;
			}

			/// <summary>
			/// Adds an object
			/// </summary>
			/// <param name="key">Object key</param>
			/// <param name="value">Object value</param>
			public override void Add( Guid key, object value )
			{
				AddSceneObject( value );
				base.Add( key, value );
			}

			/// <summary>
			/// Removes an object
			/// </summary>
			/// <param name="key">Object key</param>
			/// <returns>true if object existed in map</returns>
			public override bool Remove( Guid key )
			{
				object obj;
				if ( TryGetValue( key, out obj ) )
				{
					RemoveSceneObject( obj );
					base.Remove( key );
					return true;
				}
				return false;
			}

			private readonly Scene m_Scene;
			
			/// <summary>
			/// Adds an object, calling <see cref="ISceneObject.AddedToScene"/> if appropriate. Recurses into child objects
			/// </summary>
			private void AddSceneObject( object obj )
			{
				ISceneObject sceneObject = obj as ISceneObject;
				if ( sceneObject != null )
				{
					sceneObject.AddedToScene( m_Scene );
				}
				IParent parent = obj as IParent;
				if ( parent != null )
				{
					parent.OnChildAdded += ChildAdded;
					parent.OnChildRemoved += ChildRemoved;
					foreach ( object childObj in parent.Children )
					{
						AddSceneObject( childObj );
					}
				}
			}

			/// <summary>
			/// Removes an object, calling <see cref="ISceneObject.RemovedFromScene"/> if appropriate. Recurses into child objects
			/// </summary>
			private void RemoveSceneObject( object obj )
			{
				ISceneObject sceneObject = obj as ISceneObject;
				if ( sceneObject != null )
				{
					sceneObject.RemovedFromScene( m_Scene );
				}
				IParent parent = obj as IParent;
				if ( parent != null )
				{
					parent.OnChildAdded -= ChildAdded;
					parent.OnChildRemoved -= ChildRemoved;
					foreach ( object childObj in parent.Children )
					{
						RemoveSceneObject( childObj );
					}
				}
			}

			/// <summary>
			/// Called when an object is added as a child to an object in the scene graph
			/// </summary>
			private void ChildAdded( object parent, object child )
			{
				AddSceneObject( child );
			}

			/// <summary>
			/// Called when a child object is removed from an object in the scene graph
			/// </summary>
			private void ChildRemoved( object parent, object child )
			{
				RemoveSceneObject( child );
			}
		}

		#endregion
	}
}