using System;
using System.Collections;

namespace RbEngine.Scene
{
	/// <summary>
	/// Implementation of Query, that selects all objects of the specified type
	/// </summary>
	public class TypeQuery : Query
	{
		/// <summary>
		/// The type that objects selected by this query must derive from
		/// </summary>
		public Type	SelectType
		{
			get
			{
				return m_BaseType;
			}
		}

		/// <summary>
		/// Selects all objects that are derived from baseType
		/// </summary>
		/// <param name="baseType">Base type</param>
		public TypeQuery( Type baseType )
		{
			m_BaseType = baseType;
		}

		/// <summary>
		/// Selects the specified object if it is derived from the base type supplied to the constructor
		/// </summary>
		/// <param name="obj">Object to query</param>
		/// <returns>Returns true if the object passes the query</returns>
		public override bool Select( Object obj )
		{
			return obj.GetType( ).IsSubclassOf( m_BaseType );
		}

		private Type	m_BaseType;
	}
}
