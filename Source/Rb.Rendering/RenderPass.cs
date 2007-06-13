using System;
using System.Collections;

namespace Rb.Rendering
{

	//	TODO: This is just an ApplianceList

	/// <summary>
	/// A render pass sets up the state of the renderer for rendering
	/// </summary>
	public class RenderPass : System.Collections.IList
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public RenderPass( )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="appliances">Appliances to add to the render pass</param>
		public RenderPass( params IAppliance[] appliances )
		{
			Add( appliances );
		}

		/// <summary>
		/// Adds an object to this render pass (must implement the Rendering.IAppliance interface)
		/// </summary>
		public void						Add( IAppliance obj )
		{
			m_Appliances.Add( obj );
		}

		/// <summary>
		/// Adds an object to this render pass (must implement the Rendering.IAppliance interface)
		/// </summary>
		public void						Add( params IAppliance[] appliances )
		{
			for ( int index = 0; index < appliances.Length; ++index )
			{
				m_Appliances.Add( appliances[ index ] );
			}
		}

		/// <summary>
		/// Applies this render pass
		/// </summary>
		public virtual void				Begin( )
		{
			foreach ( IAppliance appliance in m_Appliances )
			{
				appliance.Begin( );
			}
		}

		/// <summary>
		/// Cleans up the application of this render pass
		/// </summary>
		public virtual void				End( )
		{
			foreach ( IAppliance appliance in m_Appliances )
			{
				appliance.End( );
			}
		}

		#region IList Members

		/// <summary>
		/// Returns true if this list is read only
		/// </summary>
		public bool IsReadOnly
		{
			get
			{
				return m_Appliances.IsReadOnly;
			}
		}

		/// <summary>
		/// List indexer
		/// </summary>
		public object this[ int index ]
		{
			get
			{
				return m_Appliances[ index ];
			}
			set
			{
				m_Appliances[ index ] = value;
			}
		}

		/// <summary>
		/// Removes an object at an index
		/// </summary>
		public void RemoveAt( int index )
		{
			m_Appliances.RemoveAt( index );
		}

		/// <summary>
		/// Inserts an object into the list
		/// </summary>
		public void Insert( int index, object value )
		{
			m_Appliances.Insert( index, ( IAppliance )value );
		}

		/// <summary>
		/// Removes an object from the list
		/// </summary>
		public void Remove( object value )
		{
			m_Appliances.Remove( value );
		}

		/// <summary>
		/// Returns true if this list contains the specified value
		/// </summary>
		public bool Contains( object value )
		{
			return m_Appliances.Contains( value );
		}

		/// <summary>
		/// Clears this list
		/// </summary>
		public void Clear( )
		{
			m_Appliances.Clear( );
		}

		/// <summary>
		/// Returns the index of a specified object
		/// </summary>
		public int IndexOf( object value )
		{
			return m_Appliances.IndexOf( value );
		}

		/// <summary>
		/// Adds an item to this list
		/// </summary>
		public int Add( object value )
		{
			return m_Appliances.Add( value );
		}

		/// <summary>
		/// true if this list has a fixed size
		/// </summary>
		public bool IsFixedSize
		{
			get
			{
				return m_Appliances.IsFixedSize;
			}
		}

		#endregion

		#region ICollection Members

		/// <summary>
		/// true if this list is synchronised
		/// </summary>
		public bool IsSynchronized
		{
			get
			{
				return m_Appliances.IsSynchronized;
			}
		}

		/// <summary>
		/// Returns the number of appliances stored in this list
		/// </summary>
		public int Count
		{
			get
			{
				return m_Appliances.Count;
			}
		}

		/// <summary>
		/// Copies this list to an array
		/// </summary>
		public void CopyTo( Array array, int index )
		{
			m_Appliances.CopyTo( array, index );
		}

		/// <summary>
		/// Gets an object that can synchronise this list
		/// </summary>
		public object SyncRoot
		{
			get
			{
				return m_Appliances.SyncRoot;
			}
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// Returns an enumerator for the list
		/// </summary>
		public IEnumerator GetEnumerator()
		{
			return m_Appliances.GetEnumerator( );
		}

		#endregion

		#region	Private stuff

		private ArrayList	m_Appliances = new ArrayList( );

		#endregion

	}
}
