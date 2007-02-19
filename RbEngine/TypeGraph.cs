using System;
using System.Collections;

namespace RbEngine
{
	/// <summary>
	/// Stores a collection of objects. Objects are accessible by type
	/// </summary>
	public class TypeGraph
	{
		/// <summary>
		/// Gets the list of objects that are derived from type t
		/// </summary>
		public ArrayList this[ Type t ]
		{
			get
			{
				return GetListForType( t );
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public TypeGraph( )
		{
			m_Objects = GetListForType( typeof( Object ) );
		}

		/// <summary>
		/// Adds an object to the graph
		/// </summary>
		/// <param name="obj"> Object to add </param>
		public void	Add( Object obj )
		{
			Type curType = obj.GetType( );
			while ( curType != null )
			{
				GetListForType( curType ).Add( obj );
				curType = curType.BaseType;
			}
		}

		/// <summary>
		/// Removes an object from the grahp
		/// </summary>
		/// <param name="obj"> Object to remove </param>
		public void Remove( Object obj )
		{
			Type curType = obj.GetType( );
			while ( curType != null )
			{
				GetListForType( curType ).Remove( obj );
				curType = curType.BaseType;
			}
		}

		/// <summary>
		/// Returns the array of all objects in the graph
		/// </summary>
		public ArrayList	GetAllObjects( )
		{
			return m_Objects;
		}

		/// <summary>
		/// Returns the list of all objects that are stored in the graph, and are derived from the specified type
		/// </summary>
		/// <param name="baseType">Object base type</param>
		/// <returns>Returns the list of objects that are derived from baseType</returns>
		public ArrayList GetListForType( Type baseType )
		{
			Object result = m_Map[ baseType ];
			if ( result != null )
			{
				return ( ArrayList )result;
			}
			ArrayList newList = new ArrayList( );
			m_Map[ baseType ] = newList;
			return newList;
		}

		private ArrayList	m_Objects;
		private Hashtable	m_Map = new Hashtable( );
	}
}
