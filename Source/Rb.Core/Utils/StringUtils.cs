using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace Rb.Core.Utils
{
	/// <summary>
	/// Handy string helper methods
	/// </summary>
	public static class StringUtils
	{
		public delegate string ToStringDelegate( object obj );
		public delegate string ToStringDelegate<T>( T obj );

		/// <summary>
		/// Creates a regular expression from a wildcard string (matches '?' and '*' characters)
		/// </summary>
		public static Regex CreateWildcardRegex( string wildcardString )
		{
			string pattern = "^" + Regex.Escape( wildcardString ).Replace( "\\*", ".*" ).Replace( "\\?", "." ) + "$";
			return new Regex( pattern );
		}

		/// <summary>
		/// Returns a string containing all the elements in an enumerable object
		/// </summary>
		/// <param name="enumerable">Collection to stringify</param>
		/// <param name="seperator">The elements are separated by this string</param>
		public static string StringifyEnumerable( IEnumerable enumerable, string seperator )
		{
			return StringifyEnumerable( enumerable, seperator, delegate( object obj ) { return obj.ToString( ); } );
		}

		/// <summary>
		/// Returns a string containing all the elements in an enumerable object
		/// </summary>
		/// <param name="enumerable">Collection to stringify</param>
		/// <param name="seperator">The elements are separated by this string</param>
		/// <param name="toString"></param>
		public static string StringifyEnumerable( IEnumerable enumerable, string seperator, ToStringDelegate toString )
		{
			StringBuilder sb = new StringBuilder( );
			foreach ( object element in enumerable )
			{
				if ( sb.Length > 0 )
				{
					sb.Append( seperator );
				}
				sb.Append( toString( element ) );
			}
			return sb.ToString( );
		}

		/// <summary>
		/// Returns a string containing all the elements in an enumerable object
		/// </summary>
		/// <param name="enumerable">Collection to stringify</param>
		/// <param name="seperator">The elements are separated by this string</param>
		public static string StringifyEnumerable<T>( IEnumerable<T> enumerable, string seperator )
		{
			return StringifyEnumerable( enumerable, seperator, delegate( T obj ) { return obj.ToString( ); } );
		}

		/// <summary>
		/// Returns a string containing all the elements in an enumerable object
		/// </summary>
		/// <param name="enumerable">Collection to stringify</param>
		/// <param name="seperator">The elements are separated by this string</param>
		/// <param name="toString"></param>
		public static string StringifyEnumerable<T>( IEnumerable<T> enumerable, string seperator, ToStringDelegate<T> toString )
		{
			StringBuilder sb = new StringBuilder( );
			foreach ( T element in enumerable )
			{
				if ( sb.Length > 0 )
				{
					sb.Append( seperator );
				}
				sb.Append( toString( element ) );
			}
			return sb.ToString( );
		}

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
