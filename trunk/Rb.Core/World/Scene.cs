using System;
using System.Collections.Generic;

namespace Rb.Core.World
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
			m_Objects		= new SceneObjects( this );
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
			return m_Services[ serviceType ];
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
		/// <param name="serviceName">Service name</param>
		/// <param name="service">Service object</param>
		public void AddService( object service )
		{
			Type key = service.GetType( );
			if ( m_Services.ContainsKey( key ) )
			{
				WorldLog.Warning( "Overwriting service of type \"{0}\"", key );
			}

			m_Services[ service.GetType( ) ] = service;
			WorldLog.Info( "Added service of type \"{0}\"", key );
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
		public SceneObjects Objects
		{
			get { return m_Objects; }
		}

		#endregion

		#region	Private stuff

        private Dictionary< Type, object >		m_Services = new Dictionary< Type, object >( );
		private SceneViewers					m_Viewers;
		private SceneControllers				m_Controllers;
		private SceneObjects					m_Objects;

		#endregion
	}
}