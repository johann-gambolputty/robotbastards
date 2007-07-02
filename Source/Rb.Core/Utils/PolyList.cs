using System.Collections.Generic;

namespace Rb.Core.Utils
{
    /// <summary>
    /// List implementation that allows overrides of certain IList methods
    /// </summary>
    /// <typeparam name="T">List item type</typeparam>
    public class PolyList< T > : IList< T >
	{
		#region Construction

		/// <summary>
        /// Empty list
        /// </summary>
        public PolyList( )
        {
            m_List = new List< T >( );
        }

        /// <summary>
        /// List filled by enumerator
        /// </summary>
        /// <param name="items">Item enumerator</param>
        public PolyList( IEnumerable< T > items )
        {
            m_List = new List< T >( items );
        }

        /// <summary>
        /// List created with specified number of default (null) items
        /// </summary>
        /// <param name="capacity">Number of items</param>
        public PolyList( int capacity )
        {
            m_List = new List< T >( capacity );
		}

		#endregion

		#region Private stuff

		private IList<T> m_List;

		#endregion

		#region IList<T> Members

		public int IndexOf( T item )
		{
			return m_List.IndexOf( item );
		}

		public virtual void Insert( int index, T item )
		{
			m_List.Insert( index, item );
		}

		public virtual void RemoveAt( int index )
		{
			m_List.RemoveAt( index );
		}

		public T this[ int index ]
		{
			get { return m_List[ index ]; }
			set { m_List[ index ] = value; }
		}

		#endregion

		#region ICollection<T> Members

		public virtual void Add( T item )
		{
			m_List.Add( item );
		}

		public virtual void Clear( )
		{
			m_List.Clear( );
		}

		public bool Contains( T item )
		{
			return m_List.Contains( item );
		}

		public void CopyTo( T[] array, int arrayIndex )
		{
			m_List.CopyTo( array, arrayIndex );
		}

		public int Count
		{
			get { return m_List.Count; }
		}

		public bool IsReadOnly
		{
			get { return m_List.IsReadOnly; }
		}

		public virtual bool Remove( T item )
		{
			return m_List.Remove( item );
		}

		#endregion

		#region IEnumerable<T> Members

		public IEnumerator< T > GetEnumerator( )
		{
			return m_List.GetEnumerator( );
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator( )
		{
			return m_List.GetEnumerator( );
		}

		#endregion
	}
}