using System;
using System.Collections;
using System.Collections.Generic;
using Rb.Core.Sets.Interfaces;

namespace Rb.Core.Sets.Classes
{
	/// <summary>
	/// Object set service that maps objects by type. Used by the <see cref="ObjectSetTypeFilter"/>
	/// </summary>
	public class ObjectSetTypeMapService : IObjectSetService
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="set">Set that this object is associated with</param>
		public ObjectSetTypeMapService( IObjectSet set )
		{
			if ( set == null )
			{
				throw new ArgumentNullException( "set" );
			}
			set.ObjectAdded += OnObjectAdded;
			set.ObjectRemoved += OnObjectRemoved;
		}

		/// <summary>
		/// Gets the objects stored in this map by type
		/// </summary>
		public IEnumerable this[ Type type ]
		{
			get
			{
				ArrayList objects;
				if ( m_ObjectMap.TryGetValue( type, out objects ) )
				{
					return objects;
				}
				return new object[ 0 ];
			}
		}

		#region Protected Members

		/// <summary>
		/// Returns true if a type should be registered in the map. By default, all types are
		/// </summary>
		protected virtual bool IsRegisteredType( Type type )
		{
			return true;
		}

		#endregion

		#region Private Members

		private readonly Dictionary<Type, ArrayList> m_ObjectMap = new Dictionary<Type, ArrayList>( );

		/// <summary>
		/// Iterates over all the specific types in an object, and registers the object for each of them
		/// </summary>
		private void Add( object obj )
		{
			for ( Type baseType = obj.GetType( ); baseType != null; baseType = baseType.BaseType )
			{
				Add( baseType, obj );
			}
			foreach ( Type interfaceType in obj.GetType( ).GetInterfaces( ) )
			{
				Add( interfaceType, obj );
			}
		}

		/// <summary>
		/// Iterates over all the specific types in an object, and unregisters the object for each of them
		/// </summary>
		private void Remove( object obj )
		{
			for ( Type baseType = obj.GetType( ); baseType != null; baseType = baseType.BaseType )
			{
				Remove( baseType, obj );
			}
			foreach ( Type interfaceType in obj.GetType( ).GetInterfaces( ) )
			{
				Remove( interfaceType, obj );
			}
		}

		/// <summary>
		/// Adds an object to the map
		/// </summary>
		/// <param name="type">Specific type to register obj with</param>
		/// <param name="obj">Object to add</param>
		private void Add( Type type, object obj )
		{
			ArrayList objects;
			if ( !m_ObjectMap.TryGetValue( type, out objects ) )
			{
				objects = new ArrayList( );
				m_ObjectMap[ type ] = objects;
			}
			objects.Add( obj );
		}

		/// <summary>
		/// Removes an object to the map
		/// </summary>
		/// <param name="type">Specific type that the object is registered with</param>
		/// <param name="obj">Object being removed</param>
		private void Remove( Type type, object obj )
		{
			ArrayList objects;
			if ( m_ObjectMap.TryGetValue( type, out objects ) )
			{
				objects.Remove( obj );
			}
		}

		/// <summary>
		/// Handles the <see cref="IObjectSet.ObjectAdded"/> event
		/// </summary>
		private void OnObjectAdded( IObjectSet set, object obj )
		{
			Add( obj );
		}

		/// <summary>
		/// Handles the <see cref="IObjectSet.ObjectRemoved"/> event
		/// </summary>
		private void OnObjectRemoved( IObjectSet set, object obj )
		{
			Remove( obj );
		}

		#endregion
	}
}
