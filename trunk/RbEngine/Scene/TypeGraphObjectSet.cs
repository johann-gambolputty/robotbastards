using System;
using System.Collections;

namespace RbEngine.Scene
{
	/// <summary>
	/// Implementation of ObjectSet that uses a table of ArrayObjectSet collections to store objects by type
	/// </summary>
	public class TypeGraphObjectSet : ObjectSet
	{

		#region	Public properties

		/// <summary>
		/// Gets the list of objects that are derived from type t
		/// </summary>
		public ArrayObjectSet this[ Type t ]
		{
			get
			{
				return GetListForType( t );
			}
		}

		#endregion

		#region	Set building

		/// <summary>
		/// Constructor
		/// </summary>
		public TypeGraphObjectSet( )
		{
			m_Objects = GetListForType( typeof( Object ) );
		}

		/// <summary>
		/// Adds an object to the set
		/// </summary>
		/// <param name="obj">Object to add</param>
		public override void	Add( Object obj )
		{
			Type curType = obj.GetType( );
			while ( curType != null )
			{
				GetListForType( curType ).Add( obj );

				foreach ( Type interfaceType in curType.GetInterfaces( ) )
				{
					GetListForType( interfaceType ).Add( obj );
				}

				curType = curType.BaseType;
			}
		}

		/// <summary>
		/// Removes an object from the set
		/// </summary>
		/// <param name="obj">Object to remove</param>
		/// </remarks>
		public override void	Remove( Object obj )
		{
			Type curType = obj.GetType( );
			while ( curType != null )
			{
				GetListForType( curType ).Remove( obj );

				foreach ( Type interfaceType in curType.GetInterfaces( ) )
				{
					GetListForType( interfaceType ).Remove( obj );
				}

				curType = curType.BaseType;
			}
		}

		/// <summary>
		/// Returns the array of all objects in the graph
		/// </summary>
		public ArrayObjectSet	GetAllObjects( )
		{
			return m_Objects;
		}

		/// <summary>
		/// Returns the list of all objects that are stored in the graph, and are derived from the specified type
		/// </summary>
		/// <param name="baseType">Object base type</param>
		/// <returns>Returns the list of objects that are derived from baseType</returns>
		public ArrayObjectSet GetListForType( Type baseType )
		{
			Object result = m_Map[ baseType ];
			if ( result != null )
			{
				return ( ArrayObjectSet )result;
			}
			ArrayObjectSet newList = new ArrayObjectSet( );
			m_Map[ baseType ] = newList;
			return newList;
		}

		#endregion

		#region	Set selection		

		/// <summary>
		/// Returns true if this object set is optimised for a given query type
		/// </summary>
		/// <param name="queryType">Type of query</param>
		/// <returns>true if Query objects derived from queryType are optimised by this object set</returns>
		public override bool		IsOptimisedForQueryType( Type queryType )
		{
			return queryType.IsSubclassOf( typeof( TypeQuery ) );
		}

		/// <summary>
		/// Returns the subset of objects that pass the specified query
		/// </summary>
		public override ArrayObjectSet	Select( Query select )
		{
			TypeQuery selectByType = select as TypeQuery;
			if ( selectByType != null )
			{
				return GetListForType( selectByType.SelectType );
			}
			return base.Select( select );
		}

		/// <summary>
		/// Selects objects in the set that pass the specified selection delegate. Stores selected objects in db
		/// </summary>
		/// <param name="select">Set object selection criteria</param>
		/// <param name="selected">Delegate to call if an object gets selected</param>
		public override void		Select( DelegateQuery.SelectionDelegate select, SelectedDelegate selected )
		{
			ArrayList objects = GetAllObjects( ).Objects;
			for ( int objectIndex = 0; objectIndex < objects.Count; ++objectIndex )
			{
				if ( select( objects[ objectIndex ] ) )
				{
					selected( objects[ objectIndex ] );
				}
			}
		}

		/// <summary>
		/// Selects the first object in the set that passes the specified selection delegate
		/// </summary>
		/// <param name="select">Set object selection criteria</param>
		/// <returns>First object that passes select</returns>
		public override Object		SelectFirst( DelegateQuery.SelectionDelegate select )
		{
			ArrayList objects = GetAllObjects( ).Objects;
			for ( int objectIndex = 0; objectIndex < objects.Count; ++objectIndex )
			{
				if ( select( objects[ objectIndex ] ) )
				{
					return objects[ objectIndex ];
				}
			}

			return null;
		}

		/// <summary>
		/// Selects objects in the set that pass the specified selection query. Stores selected objects in db
		/// </summary>
		/// <param name="select">Scene object selection criteria</param>
		/// <param name="selected">Delegate to call if an object gets selected</param>
		/// <remarks>
		/// Optimised for the TypeQuery query
		/// </remarks>
		public override void	Select( Query select, SelectedDelegate selected )
		{
			TypeQuery selectByType = select as TypeQuery;
			if ( selectByType != null )
			{
				ArrayList objects = GetListForType( selectByType.SelectType ).Objects;
				for ( int objectIndex = 0; objectIndex < objects.Count; ++objectIndex )
				{
					selected( objects[ objectIndex ] );
				}
			}
			else if ( select.RequiredInterface != null )
			{
				ArrayList objects = GetListForType( select.RequiredInterface ).Objects;
				for ( int objectIndex = 0; objectIndex < objects.Count; ++objectIndex )
				{
					selected( objects[ objectIndex ] );
				}
			}
			else
			{
				ArrayList objects = GetAllObjects( ).Objects;
				for ( int objectIndex = 0; objectIndex < objects.Count; ++objectIndex )
				{
					if ( select.Select( objects[ objectIndex ] ) )
					{
						selected( objects[ objectIndex ] );
					}
				}
			}
		}

		/// <summary>
		/// Selects the first object in the set that passes the specified selection query
		/// </summary>
		/// <param name="select">Set object selection criteria</param>
		/// <returns>First object that passes select</returns>
		/// <remarks>
		/// Optimised for the TypeQuery query
		/// </remarks>
		public override Object	SelectFirst( Query select )
		{
			TypeQuery selectByType = select as TypeQuery;
			if ( selectByType != null )
			{
				ArrayObjectSet typeSet = m_Map[ selectByType.SelectType ] as ArrayObjectSet;
				return ( typeSet != null ) && ( typeSet.Objects.Count > 0 ) ? ( typeSet.Objects[ 0 ] ) : null;
			}
			else if ( select.RequiredInterface != null )
			{
				ArrayObjectSet typeSet = m_Map[ select.RequiredInterface ] as ArrayObjectSet;
				return ( typeSet != null ) && ( typeSet.Objects.Count > 0 ) ? ( typeSet.Objects[ 0 ] ) : null;
			}
			else
			{
				ArrayList objects = GetAllObjects( ).Objects;
				for ( int objectIndex = 0; objectIndex < objects.Count; ++objectIndex )
				{
					if ( select.Select( objects[ objectIndex ] ) )
					{
						return objects[ objectIndex ];
					}
				}
			}

			return null;
		}

		#endregion

		private ArrayObjectSet	m_Objects;
		private Hashtable		m_Map = new Hashtable( );
	}
}
