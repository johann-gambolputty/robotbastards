using System;
using System.Collections.Generic;
using System.Text;
using Rb.Core.Sets.Interfaces;

namespace Rb.Core.Sets.Classes
{
	/// <summary>
	/// Object set service map implementation
	/// </summary>
	public class ObjectSetServiceMap : IObjectSetServiceMap
	{
		#region IObjectSetServiceMap Members

		/// <summary>
		/// Adds a service to this set
		/// </summary>
		/// <param name="service">Service to add</param>
		/// <exception cref="System.ArgumentNullException">Thrown if service is null</exception>
		public void AddService( IObjectSetService service )
		{
			if ( service == null )
			{
				throw new ArgumentNullException( "service" );
			}
			Type[] serviceTypes = GetServiceRegistrationTypes( service.GetType( ) );
			if ( serviceTypes.Length == 0 )
			{
				throw new ArgumentException( string.Format( "Service of type {0} contained no registered service types", service.GetType( ) ), "service" );
			}
			StringBuilder sb = new StringBuilder( );
			foreach ( Type serviceType in serviceTypes )
			{
				object existingService = Service( serviceType );
				if ( existingService != null )
				{
					throw new ArgumentException( string.Format( "Can't register service object of type {0} - a service of type {1} is already registered under type {2}", service.GetType( ), existingService.GetType( ), serviceType ), "service" );
				}
				m_Services.Add( serviceType, service );
				if ( sb.Length > 0 )
				{
					sb.Append( ", " );
				}
				sb.Append( serviceType );
			}

			ObjectSetLog.Info( "Registering service type \"{0}\" with types ({1})", service.GetType( ), sb.ToString( ) );
		}

		/// <summary>
		/// Removes a service from this set
		/// </summary>
		/// <param name="service">Service to remove</param>
		/// <exception cref="System.ArgumentNullException">Thrown if service is null</exception>
		public void RemoveService( IObjectSetService service )
		{
			if ( service == null )
			{
				throw new ArgumentNullException( "service" );
			}
			Type[] serviceTypes = GetServiceRegistrationTypes( service.GetType( ) );
			if ( serviceTypes.Length == 0 )
			{
				throw new ArgumentException( string.Format( "Service of type {0} contained no registered service types", service.GetType( ) ), "service" );
			}
			foreach ( Type serviceType in serviceTypes )
			{
				m_Services.Remove( serviceType );
			}
		}

		/// <summary>
		/// Gets a service attached to this set, by its type. Throws an ArgumentException if the service does not exist
		/// </summary>
		/// <typeparam name="T">Type of service to retrieve</typeparam>
		/// <returns>Returns the typed service</returns>
		/// <exception cref="System.ArgumentException">Thrown if T is not a valid service</exception>
		public T SafeService<T>( ) where T : class
		{
			T service = ( T )Service( typeof( T ) );
			if ( service == null )
			{
				throw new ArgumentException( string.Format( "No service type \"{0}\" existed in the service map", typeof( T ) ), "T" );
			}
			return service;
		}

		/// <summary>
		/// Gets a service attached to this set, by its type
		/// </summary>
		/// <typeparam name="T">Type of service to retrieve</typeparam>
		/// <returns>Returns the typed service, or null if no such service exists</returns>
		public T Service<T>( ) where T : class
		{
			return ( T )Service( typeof( T ) );
		} 

		#endregion

		#region Private Members

		private readonly Dictionary<Type, IObjectSetService> m_Services = new Dictionary<Type, IObjectSetService>( );

		/// <summary>
		/// Non-generic version of service retrieval
		/// </summary>
		private object Service( Type type )
		{
			IObjectSetService service;
			return m_Services.TryGetValue( type, out service ) ? service : null;
		}
		
		/// <summary>
		/// Returns the type to register a service under
		/// </summary>
		private static Type[] GetServiceRegistrationTypes( Type type )
		{
			List<Type> registrationTypes = new List<Type>( );
			for ( Type baseType = type; baseType != typeof( object ); baseType = type.BaseType )
			{
				if ( IsObjectSetServiceType( baseType ) )
				{
					registrationTypes.Add( baseType );
				}
			}
			foreach ( Type interfaceType in type.GetInterfaces( ) )
			{
				if ( IsObjectSetServiceType( interfaceType ) )
				{
					registrationTypes.Add( interfaceType );
				}
			}
			if ( registrationTypes.Count == 0 )
			{
				registrationTypes.Add( type );
			}
			return registrationTypes.ToArray( );
		}

		/// <summary>
		/// Returns true if a type is marked with the ObjectSetServiceAttribute
		/// </summary>
		private static bool IsObjectSetServiceType( Type type )
		{
			return type.GetCustomAttributes( typeof( ObjectSetServiceAttribute ), false ).Length > 0;
		}

		#endregion
	}
}
