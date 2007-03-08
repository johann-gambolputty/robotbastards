using System;
using System.Collections;

namespace RbEngine.Scene
{
	/// <summary>
	/// Implements ObjectSet using an ArrayList
	/// </summary>
	public class ArrayObjectSet : ObjectSet
	{
		/// <summary>
		/// Gets the array
		/// </summary>
		public ArrayList	Objects
		{
			get
			{
				return m_Objects;
			}
		}
		

		#region	Set building

		/// <summary>
		/// Adds an object to the set
		/// </summary>
		/// <param name="obj">Object to add</param>
		public override void	Add( Object obj )
		{
			m_Objects.Add( obj );
		}

		/// <summary>
		/// Removes an object from the set
		/// </summary>
		/// <param name="obj">Object to remove</param>
		public override void	Remove( Object obj )
		{
			m_Objects.Remove( obj );
		}

		#endregion

		#region	Visiting

		/// <summary>
		/// Visits all objects in this set
		/// </summary>
		/// <param name="visitor">Visitor function</param>
		public override void Visit( Components.ChildVisitorDelegate visitor )
		{
			ArrayList objects = m_Objects;
			for ( int objectIndex = 0; objectIndex < objects.Count; ++objectIndex )
			{
				if ( !visitor( objects[ objectIndex ] ) )
				{
					return;
				}
			}
		}

		#endregion

		#region	Set selection

		/// <summary>
		/// Selects objects in the set that pass the specified selection delegate. Stores selected objects in db
		/// </summary>
		/// <param name="select">Set object selection criteria</param>
		/// <param name="selected">Delegate to call if an object gets selected</param>
		public override void	Select( DelegateQuery.SelectionDelegate select, SelectedDelegate selected )
		{
			for ( int objectIndex = 0; objectIndex < m_Objects.Count; ++objectIndex )
			{
				if ( select( m_Objects[ objectIndex ] ) )
				{
					selected( m_Objects[ objectIndex ] );
				}
			}
		}

		/// <summary>
		/// Selects the first object in the set that passes the specified selection delegate
		/// </summary>
		/// <param name="select">Set object selection criteria</param>
		/// <returns>First object that passes select</returns>
		public override Object	SelectFirst( DelegateQuery.SelectionDelegate select )
		{
			for ( int objectIndex = 0; objectIndex < m_Objects.Count; ++objectIndex )
			{
				if ( select( m_Objects[ objectIndex ] ) )
				{
					return m_Objects[ objectIndex ];
				}
			}
			return null;
		}


		/// <summary>
		/// Runs a query over the objects in the set
		/// </summary>
		/// <param name="select">Set object selection criteria</param>
		/// <remarks>
		/// This is equivalent to Visit(), except that the object sets can optimise for a given query type
		/// </remarks>
		public override void	Select( Query select )
		{
			for ( int objectIndex = 0; objectIndex < m_Objects.Count; ++objectIndex )
			{
				select.Select( m_Objects[ objectIndex ] );
			}
		}

		/// <summary>
		/// Selects objects in the set that pass the specified selection query. Stores selected objects in db
		/// </summary>
		/// <param name="select">Set object selection criteria</param>
		/// <param name="selected">Delegate that is invoked when an object is selected</param>
		public override void	Select( Query select, SelectedDelegate selected )
		{
			for ( int objectIndex = 0; objectIndex < m_Objects.Count; ++objectIndex )
			{
				if ( select.Select( m_Objects[ objectIndex ] ) )
				{
					selected( m_Objects[ objectIndex ] );
				}
			}
		}

		/// <summary>
		/// Selects the first object in the set that passes the specified selection query
		/// </summary>
		/// <param name="select">Set object selection criteria</param>
		/// <returns>First object that passes select</returns>
		public override Object	SelectFirst( Query select )
		{
			for ( int objectIndex = 0; objectIndex < m_Objects.Count; ++objectIndex )
			{
				if ( select.Select( m_Objects[ objectIndex ] ) )
				{
					return m_Objects[ objectIndex ];
				}
			}
			return null;
		}

		#endregion

		private ArrayList	m_Objects = new ArrayList( );
	}
}