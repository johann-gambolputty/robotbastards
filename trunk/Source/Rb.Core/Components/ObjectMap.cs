using System;
using System.Collections;
using System.Collections.Generic;

namespace Rb.Core.Components
{
	/// <summary>
	/// Implements the IObjectMap interface using Dictionary objects
	/// </summary>
	[Serializable]
	public class ObjectMap : IObjectMap
	{
		#region IObjectMap Members

		/// <summary>
		/// Adds any object to the map, assigning a new ID
		/// </summary>
		/// <param name="obj">Unique object</param>
		/// <returns>Returns the ID of the added object</returns>
		public Guid Add( object obj )
		{
			Guid newId = Guid.NewGuid( );
			Add( newId, obj );
			return newId;
		}

		/// <summary>
		/// Adds an IUnique object to the map
		/// </summary>
		/// <param name="obj">Unique object</param>
		public void Add( IUnique obj )
		{
			Add( obj.Id, obj );
		}
		
		/// <summary>
		/// Removes an IUnique object from the map
		/// </summary>
		/// <param name="obj">Unique object</param>
		public void Remove( IUnique obj )
		{
			Remove( obj.Id );
		}

		/// <summary>
		/// Gets an object of a given type, and key, from the map
		/// </summary>
		public object Get( Type objectType, Guid key )
		{
			return m_Types[ objectType ][ key ];
		}

        /// <summary>
        /// Gets an object of a given type, and key, from the map
        /// </summary>
		public object Get< T >( Guid key )
		{
			return Get( typeof( T ), key );
		}
        
        /// <summary>
        /// Gets all objects of a given type from the map
        /// </summary>
        public IEnumerable< T > GetAllOfType< T >( )
        {
            if ( m_Types.ContainsKey( typeof( T ) ) )
            {
                foreach ( KeyValuePair< Guid, object > kvp in m_Types[ typeof( T ) ] )
                {
                    yield return ( T )kvp.Value;
                }
            }
        }

		/// <summary>
		/// Gets the first value of a given type
		/// </summary>
		public T GetFirstOfType< T >( ) where T : class
		{
			Dictionary<Guid, object> objectMap;
			if ( !m_Types.TryGetValue( typeof( T ), out objectMap ) )
			{
				return null;
			}
			if ( objectMap.Count == 0 )
			{
				return null;
			}
			IEnumerator pos = objectMap.Values.GetEnumerator( );
			pos.MoveNext( );
			return ( T )pos.Current;
		}

		/// <summary>
		/// Gets an array of all objects in the map that are of type T
		/// </summary>
		public T[] GetArrayOfType< T >( )
		{
			Dictionary< Guid, object > objectMap;
			if ( !m_Types.TryGetValue( typeof( T ), out objectMap ) )
			{
				return new T[ 0 ];
			}
			Dictionary<Guid, object>.ValueCollection values = objectMap.Values;
			T[] objects = new T[ values.Count ];

			int objectIndex = 0;
			foreach ( T value in values )
			{
				objects[ objectIndex++ ] = value;
			}
			return objects;
		}

		#endregion

		#region IDictionary<Guid,object> Members

		public virtual void Add( Guid key, object value )
		{
			m_All[ key ] = value;
			ComponentLog.Verbose( "ObjectMap: Adding object, type \"{0}\", key \"{1}\"", value.GetType( ), key );
			for ( Type baseType = value.GetType( ); baseType != null; baseType = baseType.BaseType )
			{
				GetTypeMap( baseType )[ key ] = value;
			}

			AddInterfaces( key, value, value.GetType( ) );
		}

		public bool ContainsKey( Guid key )
		{
			return m_All.ContainsKey( key );
		}

		public ICollection< Guid > Keys
		{
			get { return m_All.Keys;  }
		}

		public virtual bool Remove( Guid key )
		{
			ComponentLog.Verbose( "ObjectMap: Removing object \"{0}\"", key );
			object obj;
			if ( !m_All.TryGetValue( key, out obj ) )
			{
				return false;
			}

			m_All.Remove( key );

			for ( Type baseType = obj.GetType( ); baseType != null; baseType = baseType.BaseType )
			{
				m_Types[ baseType ].Remove( key );
			}

			RemoveInterfaces( key, obj.GetType( ) );

			return true;
		}

		public bool TryGetValue( Guid key, out object value )
		{
			return m_All.TryGetValue( key, out value );
		}

		public ICollection< object > Values
		{
			get { return m_All.Values; }
		}

		public object this[ Guid key ]
		{
			get
			{
				return m_All[ key ];
			}
			set
			{
				Add( key, value );
			}
		}

		#endregion

		#region ICollection<KeyValuePair<Guid,object>> Members

		public void Add( KeyValuePair<Guid, object> item )
		{
			Add( item.Key, item.Value );
		}

		public virtual void Clear( )
		{
			m_All.Clear( );
			m_Types.Clear( );
		}

		public bool Contains( KeyValuePair<Guid, object> item )
		{
			return ( ( ICollection< KeyValuePair< Guid,object > > )m_All ).Contains( item );
		}

		public void CopyTo( KeyValuePair<Guid, object>[] array, int arrayIndex )
		{
			( ( ICollection<KeyValuePair<Guid, object>> )m_All ).CopyTo( array, arrayIndex );
		}

		public int Count
		{
			get { return m_All.Count; }
		}

		public bool IsReadOnly
		{
			get { return ( ( ICollection<KeyValuePair<Guid, object>> )m_All ).IsReadOnly; }
		}

		public bool Remove( KeyValuePair<Guid, object> item )
		{
			return ( ( ICollection< KeyValuePair< Guid,object > > )m_All ).Remove( item );
		}

		#endregion

		#region IEnumerable<KeyValuePair<Guid,object>> Members

		public IEnumerator<KeyValuePair<Guid, object>> GetEnumerator( )
		{
			return m_All.GetEnumerator( );
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator( )
		{
			return ( ( IEnumerable )m_All ).GetEnumerator( );
		}

		#endregion

		#region Private stuff

		Dictionary<Guid, object> m_All = new Dictionary<Guid, object>( );
		Dictionary<Type, Dictionary<Guid, object>> m_Types = new Dictionary<Type, Dictionary<Guid, object>>( );

		private void AddInterfaces( Guid key, object obj, Type type )
		{
			foreach ( Type interfaceType in type.GetInterfaces( ) )
			{
				GetTypeMap( interfaceType )[ key ] = obj;
			}
		}

		private void RemoveInterfaces( Guid key, Type type )
		{
			foreach ( Type interfaceType in type.GetInterfaces( ) )
			{
				m_Types[ interfaceType ].Remove( key );
			}
		}

		private Dictionary< Guid, object > GetTypeMap( Type type )
		{
			Dictionary<Guid, object> mapForType;
			if ( !m_Types.TryGetValue( type, out mapForType ) )
			{
				mapForType = new Dictionary<Guid, object>( );
				m_Types[ type ] = mapForType;
			}

			return mapForType;
		}

		#endregion
	}
}
