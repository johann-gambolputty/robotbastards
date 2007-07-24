
using System;

namespace Rb.Core.Utils
{
	/// <summary>
	/// Handy string helper methods
	/// </summary>
	public static class StringHelpers
	{
		/// <summary>
		/// Converts a (possibly null or empty) string into an enum value
		/// </summary>
		/// <typeparam name="T">Enum type</typeparam>
		/// <param name="value">String value</param>
		/// <param name="defaultValue">Default value, returned if string is null or empty</param>
		/// <returns>Returns the parsed value, or defaultValue is the string is null or empty</returns>
		public static T StringToEnum< T >( string value, T defaultValue )
		{
			if ( string.IsNullOrEmpty( value ) )
			{
				return defaultValue;
			}
			return ( T )Enum.Parse( typeof( T ), value );
		}

	}
}
