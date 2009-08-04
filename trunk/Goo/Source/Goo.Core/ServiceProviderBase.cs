using System;
using System.Collections.Generic;
using log4net;
using Rb.Core.Utils;

namespace Goo.Core
{
	/// <summary>
	/// Base class for implementors of <see cref="IServiceProvider"/>
	/// </summary>
	public class ServiceProviderBase : IServiceProvider
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public ServiceProviderBase( )
		{
			m_Log = LogManager.GetLogger( GetType( ) );
		}

		/// <summary>
		/// Gets the logger for this object
		/// </summary>
		public ILog Log
		{
			get { return m_Log; }
		}

		#region IServiceProvider Members

		/// <summary>
		/// Gets all registered services
		/// </summary>
		public object[] Services
		{
			get { return m_Services.ToArray( ); }
		}

		/// <summary>
		/// Adds a service object
		/// </summary>
		/// <param name="service">Service object to add</param>
		public TService AddService<TService>( TService service )
		{
			Arguments.CheckNotNull( service, "service" );
			if ( m_Services.Contains( service ) )
			{
				m_Log.WarnFormat( "{0} already contains the service object {1} - skipping remove operation", this, service );
				return service;
			}
			m_Services.Add( service );
			return service;
		}

		/// <summary>
		/// Removes a service object
		/// </summary>
		/// <param name="service">Service object to remove</param>
		public void RemoveService( object service )
		{
			Arguments.CheckNotNull( service, "service" );
			if ( !m_Services.Contains( service ) )
			{
				m_Log.WarnFormat( "{0} does not contain the service object {1} - skipping remove operation", this, service );
				return;
			}
			m_Services.Remove( service );
		}

		/// <summary>
		/// Gets a service belonging to this object by its type
		/// </summary>
		/// <typeparam name="TService">Service type</typeparam>
		/// <returns>Returns a service of the specified type. Returns default of TService if no such service exists.</returns>
		public TService GetService<TService>( )
		{
			foreach ( object service in m_Services )
			{
				if ( service is TService )
				{
					return ( TService )service;
				}
			}
			return default( TService );
		}

		/// <summary>
		/// Gets a service belonging to this object by its type
		/// </summary>
		/// <typeparam name="TService">Service type</typeparam>
		/// <returns>Returns a service of the specified type. Throws a KeyNotFoundException if no such service exists.</returns>
		public TService EnsureGetService<TService>( )
		{
			foreach ( object service in m_Services )
			{
				if ( service is TService )
				{
					return ( TService )service;
				}
			}
			throw new ArgumentException( "Could not find service of type " + typeof( TService ), "TService" );
		}

		#endregion

		#region Private Members

		private readonly List<object> m_Services = new List<object>( );
		private readonly ILog m_Log;

		#endregion
	}
}
