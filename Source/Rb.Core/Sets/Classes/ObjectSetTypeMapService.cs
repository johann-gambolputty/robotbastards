using System;
using System.Collections;
using Rb.Core.Components;
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
			get { return m_TypeMap.GetObjectsOfType( type ); }
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

		private readonly ObjectTypeMap<object> m_TypeMap = new ObjectTypeMap<object>( );

		/// <summary>
		/// Handles the <see cref="IObjectSet.ObjectAdded"/> event
		/// </summary>
		private void OnObjectAdded( IObjectSet set, object obj )
		{
			m_TypeMap.Add( obj );
		}

		/// <summary>
		/// Handles the <see cref="IObjectSet.ObjectRemoved"/> event
		/// </summary>
		private void OnObjectRemoved( IObjectSet set, object obj )
		{
			m_TypeMap.Remove( obj );
		}

		#endregion
	}
}
