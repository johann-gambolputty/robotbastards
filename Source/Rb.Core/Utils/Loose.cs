using System;
using System.Reflection;

namespace Rb.Core.Utils
{
	/// <summary>
	/// Functions for loosely related types (types that have the same fields).
	/// </summary>
	public static class Loose
	{
		/// <summary>
		/// Returns true if all the two objects are equal, and all their fields are equal
		/// </summary>
		/// <remarks>
		/// Handy for unit testing, to provide a deep equality check when a class only supports reference equality.
		/// </remarks>
		public static bool DeepEquality( object lhs, object rhs )
		{
			return DeepEquality( lhs, rhs, new System.Collections.Hashtable( ) );
		}

		#region Private Members

		/// <summary>
		/// Same type equality
		/// </summary>
		private static bool SameTypeEquality( object lhs, object rhs, System.Collections.Hashtable objectMap )
		{
			if ( objectMap.ContainsKey( lhs ) )
			{
				//	Lhs has already been visited. If the rhs is the same as the last visit, then we know that they
				//	are equal
				return ( objectMap[ lhs ] == rhs );
			}
			objectMap[ lhs ] = rhs;
			for ( Type type = lhs.GetType( ); type != null; type = type.BaseType )
			{
				foreach ( FieldInfo field in type.GetFields( BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic ) )
				{
					if ( !DeepEquality( field.GetValue( lhs ), field.GetValue( rhs ) ) )
					{
						return false;
					}
				}
			}
			return true;
		}

		/// <summary>
		/// Returns true if all the two objects are equal, and all their fields are equal. Maintains a map of objects to stop circular references
		/// </summary>
		private static bool DeepEquality( object lhs, object rhs, System.Collections.Hashtable objectMap )
		{
			if ( lhs == null )
			{
				return rhs == null;
			}
			if ( rhs == null )
			{
				return false;
			}
			if ( lhs.GetType( ).IsValueType )
			{
				return lhs.Equals( rhs );
			}

			if ( lhs.GetType( ) == rhs.GetType( ) )
			{
				return SameTypeEquality( lhs, rhs, objectMap );
			}

			return false;
		}

		#endregion
	}

}
