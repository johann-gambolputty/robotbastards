using System;

namespace RbEngine.Scene
{
	/// <summary>
	/// Implements Query using a user-provided delegate
	/// </summary>
	public class DelegateQuery : Query
	{
		/// <summary>
		/// Delegate for selecting objects in the scene
		/// </summary>
		public delegate bool	SelectionDelegate( Object obj );

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="selection">Delegate to check if an object should be selected</param>
		public DelegateQuery( SelectionDelegate selection )
		{
			m_Selection = selection;
		}

		/// <summary>
		/// Selects the specified object if it is passes the selection delegate
		/// </summary>
		/// <param name="obj">Object to query</param>
		/// <returns>Returns true if the object passes the query</returns>
		public override bool Select( Object obj )
		{
			return m_Selection( obj );
		}

		private SelectionDelegate	m_Selection;
	}
}
