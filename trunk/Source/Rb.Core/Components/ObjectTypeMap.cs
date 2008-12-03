using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rb.Core.Components
{
	/// <summary>
	/// Helper functions for <see cref="ObjectTypeMap{T}"/>
	/// </summary>
	public class ObjectTypeMap
	{
		/// <summary>
		/// Delegate used to determine what types an object should be registered under when inserted into an <see cref="ObjectTypeMap{T}"/>
		/// </summary>
		/// <param name="obj">Object to retrieve types for. Never null</param>
		/// <returns>Returns a list of types to register obj as</returns>
		public delegate IEnumerable<Type> GetObjectTypesDelegate( object obj );

		/// <summary>
		/// Gets all types of an object
		/// </summary>
		public static IEnumerable<Type> GetAllTypes( object obj )
		{
			for ( Type baseType = obj.GetType( ); baseType != null; baseType = baseType.BaseType )
			{
				yield return baseType;
			}

			foreach ( Type interfaceType in obj.GetType( ).GetInterfaces( ) )
			{
				yield return interfaceType;
			}
		}

		/// <summary>
		/// Gets all interfaces types of an object
		/// </summary>
		public static IEnumerable<Type> GetAllInterfaceTypes( object obj )
		{
			foreach ( Type interfaceType in obj.GetType( ).GetInterfaces( ) )
			{
				yield return interfaceType;
			}
		}
	}

	/// <summary>
	/// Stores a bag of objects and allows them to retrieved by type
	/// </summary>
	public class ObjectTypeMap<T> : IEnumerable<T>
		where T : class
	{
		/// <summary>
		/// Default constructor. Retrieves all types associated with an object
		/// </summary>
		public ObjectTypeMap( ) :
			this( ObjectTypeMap.GetAllTypes )
		{
		}

		/// <summary>
		/// Setup constructor. Sets the delegate used to retrieve type keys for objects
		/// </summary>
		/// <param name="getObjectTypes">Object type key delegate</param>
		public ObjectTypeMap( ObjectTypeMap.GetObjectTypesDelegate getObjectTypes )
		{
			m_ObjectTypeKeyGenerator = getObjectTypes;
		}

		/// <summary>
		/// Adds an object to the map
		/// </summary>
		/// <param name="obj">Object to add</param>
		public void Add( T obj )
		{
			if ( obj == null )
			{
				throw new ArgumentNullException( "obj" );
			}
			foreach ( Type type in m_ObjectTypeKeyGenerator( obj ) )
			{
				Get( type ).Add( obj );
			}
			m_AllObjects.Add( obj );
		}

		/// <summary>
		/// Removes an object from the map
		/// </summary>
		/// <param name="obj">Object to remove</param>
		public void Remove( T obj )
		{
			if ( obj == null )
			{
				throw new ArgumentNullException( "obj" );
			}
			foreach ( Type type in m_ObjectTypeKeyGenerator( obj ) )
			{
				Get( type ).Remove( obj );
			}
			m_AllObjects.Remove( obj );
		}

		/// <summary>
		/// Gets the first object that matches the specified key
		/// </summary>
		/// <param name="t">Object type to retrieve</param>
		/// <returns>First object in the map that has type t. Returns null if no object of type t exists</returns>
		public T GetFirstOfType( Type t )
		{
			List<T> objects = Get( t );
			return objects.Count == 0 ? null : objects[ 0 ];
		}

		/// <summary>
		/// Gets a list of all the objects in the map of type t
		/// </summary>
		/// <param name="t">Object type to retrieve</param>
		/// <returns>List of objects of type t</returns>
		public ReadOnlyCollection<T> GetObjectsOfType( Type t )
		{
			return Get( t ).AsReadOnly( );
		}

		/// <summary>
		/// Gets the first object that matches the specified key
		/// </summary>
		/// <typeparam name="X">Object type to retrieve</typeparam>
		/// <returns>First object in the map that has type t. Returns null if no object of type t exists</returns>
		public X GetFirstOfType<X>( )
		{
			return ( X )( object )GetFirstOfType( typeof( X ) );
		}

		/// <summary>
		/// Gets a list of all the objects in the map of type t
		/// </summary>
		/// <returns>List of objects of type t</returns>
		public ReadOnlyCollection<X> GetObjectsOfType<X>( )
		{
			List < X > results = new List<X>( );
			foreach ( T obj in Get( typeof( X ) ) )
			{
				results.Add( ( X )( object )obj );
			}
			return results.AsReadOnly( );
		}

		/// <summary>
		/// Returns all the values in this map
		/// </summary>
		public ReadOnlyCollection<T> Values
		{
			get { return m_AllObjects.AsReadOnly( ); }
		}

		/// <summary>
		/// Removes all objects from the map
		/// </summary>
		public void Clear( )
		{
			m_TypeMap.Clear( );
			m_AllObjects.Clear( );
		}

		#region Private Member

		private readonly ObjectTypeMap.GetObjectTypesDelegate m_ObjectTypeKeyGenerator;
		private readonly Dictionary<Type, List<T>> m_TypeMap = new Dictionary<Type, List<T>>( );
		private readonly List<T> m_AllObjects = new List<T>( );

		/// <summary>
		/// Gets an object list from a type key
		/// </summary>
		private List<T> Get( Type type )
		{
			List<T> objects;
			if ( !m_TypeMap.TryGetValue( type, out objects ) )
			{
				objects = new List<T>( );
				m_TypeMap.Add( type, objects );
			}
			return objects;
		}

		#endregion

		#region IEnumerable<T> Members

		/// <summary>
		/// Returns all objects in this map
		/// </summary>
		public IEnumerator<T> GetEnumerator( )
		{
			return m_AllObjects.GetEnumerator( );
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator( )
		{
			return m_AllObjects.GetEnumerator( );
		}

		#endregion
	}
}
