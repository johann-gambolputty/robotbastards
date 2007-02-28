using System;

namespace RbEngine.Scene
{
	/// <summary>
	/// Stores a set of objects that can be queried
	/// </summary>
	/// <remarks>
	/// ObjectSet provides storage for objects in a scene. The SceneDb is built around a single ObjectSet (usually some spatially organised
	/// set, like a BSP tree). The ObjectSet class provides support for queries that can be used to select one or more objects from the
	/// set. Classes that implement ObjectSet are usually set up to optimise one or more of these query types. For example, a spatially
	/// organised ObjectSet would optimise spatial queries (those derived from SpatialQuery).
	/// </remarks>
	public abstract class ObjectSet : System.Collections.ICollection
	{
		#region	Set building

		/// <summary>
		/// Adds an object to the set
		/// </summary>
		/// <param name="obj">Object to add</param>
		public abstract void	Add( Object obj );

		/// <summary>
		/// Removes an object from the set
		/// </summary>
		/// <param name="obj">Object to remove</param>
		public abstract void	Remove( Object obj );

		#endregion

		#region	Visiting

		/// <summary>
		/// Visits all objects in this set
		/// </summary>
		/// <param name="visitor">Visitor function</param>
		public abstract void Visit( Components.ChildVisitorDelegate visitor );

		#endregion

		#region	Set selection

		/// <summary>
		/// Delegate to call when a Select() or SelectFirst() method passes the query
		/// </summary>
		public delegate void	SelectedDelegate( Object obj );

		/// <summary>
		/// Returns true if this object set is optimised for a given query type
		/// </summary>
		/// <param name="queryType">Type of query</param>
		/// <returns>true if Query objects derived from queryType are optimised by this object set</returns>
		public virtual bool				IsOptimisedForQueryType( Type queryType )
		{
			return false;
		}

		/// <summary>
		/// Returns the subset of objects that pass the specified query
		/// </summary>
		public virtual ArrayObjectSet	GetSelection( Query select )
		{
			ArrayObjectSet result = new ArrayObjectSet( );
			Select( select, new SelectedDelegate( result.Add ) );
			return result;
		}

		/// <summary>
		/// Selects objects in the set that pass the specified selection delegate. Stores selected objects in db
		/// </summary>
		/// <param name="select">Set object selection criteria</param>
		/// <param name="selected">Delegate to call if an object gets selected</param>
		public virtual void				Select( DelegateQuery.SelectionDelegate select, SelectedDelegate selected )
		{
			Select( new DelegateQuery( select ), selected );
		}

		/// <summary>
		/// Selects the first object in the set that passes the specified selection delegate
		/// </summary>
		/// <param name="select">Set object selection criteria</param>
		/// <returns>First object that passes select</returns>
		public virtual Object			SelectFirst( DelegateQuery.SelectionDelegate select )
		{
			return SelectFirst( new DelegateQuery( select ) );
		}


		/// <summary>
		/// Runs a query over the objects in the set
		/// </summary>
		/// <param name="select">Set object selection criteria</param>
		/// <remarks>
		/// This is equivalent to Visit(), except that the object sets can optimise for a given query type
		/// </remarks>
		public abstract void			Select( Query select );

		/// <summary>
		/// Selects objects in the set that pass the specified selection query. Stores selected objects in db
		/// </summary>
		/// <param name="select">Set object selection criteria</param>
		/// <param name="selected">Delegate that is invoked when an object is selected</param>
		public abstract void			Select( Query select, SelectedDelegate selected );

		/// <summary>
		/// Selects the first object in the set that passes the specified selection query
		/// </summary>
		/// <param name="select">Set object selection criteria</param>
		/// <returns>First object that passes select</returns>
		public abstract Object			SelectFirst( Query select );

		#endregion

		#region ICollection Members

		public bool IsSynchronized
		{
			get
			{
				// TODO:  Add ObjectSet.IsSynchronized getter implementation
				return false;
			}
		}

		public int Count
		{
			get
			{
				// TODO:  Add ObjectSet.Count getter implementation
				return 0;
			}
		}

		public void CopyTo(Array array, int index)
		{
			// TODO:  Add ObjectSet.CopyTo implementation
		}

		public object SyncRoot
		{
			get
			{
				// TODO:  Add ObjectSet.SyncRoot getter implementation
				return null;
			}
		}

		#endregion

		#region IEnumerable Members

		public System.Collections.IEnumerator GetEnumerator()
		{
			// TODO:  Add ObjectSet.GetEnumerator implementation
			return null;
		}

		#endregion
	}
}
