
using System.Collections.Generic;
using System.Collections;

namespace Rb.Core.Utils
{
	/// <summary>
	/// Returns items in an enumeration cast to type OutputType
	/// </summary>
	/// <typeparam name="OutputType">Items in enumerations are cast to this type</typeparam>
	public static class EnumerableAdapter<OutputType>
	{
		/// <summary>
		/// Casts a generic enumeration of objects of type InputType to OutputType
		/// </summary>
		public static IEnumerable<OutputType> Cast<InputType>( IEnumerable<InputType> input )
		{
			foreach ( InputType item in input )
			{
				yield return ( OutputType )( object )item;
			}
		}

		/// <summary>
		/// Casts an enumeration of objects of type InputType to OutputType
		/// </summary>
		public static IEnumerable<OutputType> Cast( IEnumerable input )
		{
			foreach ( object item in input )
			{
				yield return ( OutputType )item;
			}
		}

		/// <summary>
		/// Casts a generic enumeration of objects of type InputType to OutputType, and stores
		/// the results in a list 
		/// </summary>
		public static List<OutputType> ToList<InputType>( IEnumerable<InputType> input )
		{
			return new List<OutputType>( Cast( input ) );
		}

		/// <summary>
		/// Casts an enumeration of objects of type InputType to OutputType, and stores
		/// the results in a list 
		/// </summary>
		public static List<OutputType> ToList( IEnumerable input )
		{
			return new List<OutputType>( Cast( input ) );
		}
	}
}
