using System;
using System.Collections;
using System.Collections.Generic;
using Rb.Core.Utils;
using Rb.Core.Components;

namespace Rb.World
{
	/// <summary>
	/// Manages all objects in the scene, and the services that tend to them
	/// </summary>
	public class Scene
	{
		#region Public construction

		/// <summary>
		/// Construction
		/// </summary>
		public Scene( )
		{
			m_Viewers		= new SceneViewers( this );
			m_Controllers	= new SceneControllers( this );
		}

		#endregion

        #region Clocks

        /// <summary>
        /// Gets a named clock. Throws an exception on failure
        /// </summary>
        /// <param name="name">Clock name</param>
        /// <returns>Returns clock</returns>
        public Clock GetClock( string name )
        {
            return m_Clocks[ name ];
        }

        /// <summary>
        /// Adds a single clock to the scene
        /// </summary>
        /// <param name="clock">Clock to add</param>
        public void AddClock( Clock clock )
        {
            if ( m_Clocks.ContainsKey( clock.Name ) )
            {
                WorldLog.Warning( string.Format( "Clock with name \"{0}\" already exists in the scene - overwriting...", clock.Name ) );
            }
			WorldLog.Info( "Adding clock \"{0}\" (tick interval {1}ms)", clock.Name, clock.TickTime );
            m_Clocks[ clock.Name ] = clock;
        }

        /// <summary>
        /// Adds a set of clocks to the scene
        /// </summary>
        /// <param name="clocks">Clocks to add</param>
        public void AddClocks( IEnumerable clocks )
        {
            foreach ( Clock curClock in clocks )
            {
                AddClock( curClock );
            }
        }

        #endregion

        #region	Services

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
			AddService( service.GetType( ), service );

            //  Also register the service with all the interfaces that it supports
            foreach ( Type interfaceType in service.GetType( ).GetInterfaces( ) )
            {
                AddService( interfaceType, service );
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
			if ( service != null )
			{
				Type key = service.GetType( );
				if ( !m_Services.ContainsKey( key ) )
				{
					WorldLog.Warning( "Failed to find service of type \"{0}\"", key );
				}
				else
				{
					m_Services.Remove( key );
				}
			}
		}

		#endregion

		#region	Public properties

		/// <summary>
		/// Gets the scene viewer set
		/// </summary>
		public SceneViewers Viewers
		{
			get { return m_Viewers; }
		}

		/// <summary>
		/// Gets the scene controller set
		/// </summary>
		public SceneControllers Controllers
		{
			get { return m_Controllers; }
		}

		/// <summary>
		/// Gets the scene object set
		/// </summary>
		public ObjectMap Objects
		{
			get { return m_Objects; }
		}

		#endregion

		#region	Private stuff
        
        private Dictionary< string, Clock >     m_Clocks	= new Dictionary< string, Clock >( );
		private Dictionary< Type, object >		m_Services	= new Dictionary< Type, object >( );
		private ObjectMap						m_Objects	= new ObjectMap( );
		private SceneViewers					m_Viewers;
		private SceneControllers				m_Controllers;


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
	}
}