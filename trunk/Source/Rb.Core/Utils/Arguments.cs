using System;
using System.Collections;

namespace Rb.Core.Utils
{
	/// <summary>
	/// Argument checks
	/// </summary>
	public static class Arguments
	{
		/// <summary>
		/// Checks if an argument is not null. If it is, an <see cref="ArgumentNullException"/> is thrown
		/// </summary>
		public static void CheckNotNull( object arg, string argName )
		{
			if ( arg == null )
			{
				throw new ArgumentNullException( argName );
			}
		}

		/// <summary>
		/// Checks that arg is not null, and is of type T. Returns arg cast to T
		/// </summary>
		public static T CheckedNonNullCast<T>( object arg, string argName ) where T : class
		{
			CheckNotNull( arg, argName );
			T argT = arg as T;
			if ( argT == null )
			{
				throw new ArgumentException( string.Format( "Argument \"{0}\" was not expected type \"{1}\" (was \"{2}\")", argName, typeof( T ), arg.GetType( ) ) );
			}
			return argT;
		}

		/// <summary>
		/// Checks that a string is not null (<see cref="ArgumentNullException"/> thrown) or empty (<see cref="ArgumentException"/> thrown)
		/// </summary>
		public static void CheckNotNullOrEmpty( string arg, string argName )
		{
			if ( arg == null )
			{
				throw new ArgumentNullException( argName );
			}
			if ( arg == string.Empty )
			{
				throw new ArgumentException( "Argument cannot be an empty string", argName );
			}
		}

		/// <summary>
		/// Checks that a collection is not null (<see cref="ArgumentNullException"/> thrown) or empty (<see cref="ArgumentException"/>)
		/// </summary>
		public static void CheckNotNullOrEmpty( ICollection arg, string argName )
		{
			if ( arg == null )
			{
				throw new ArgumentNullException( argName );
			}
			if ( arg.Count == 0 )
			{
				throw new ArgumentException( "Argument cannot be an empty string", argName );
			}
		}

		/// <summary>
		/// Throws an ArgumentException with a formatted string message
		/// </summary>
		/// <param name="paramName">Parameter name</param>
		/// <param name="format">Exception message format</param>
		/// <param name="args">Exception message argument</param>
		/// <exception cref="ArgumentException"/>
		public static void ThrowArgumentException( string paramName, string format, params object[] args )
		{
			throw new ArgumentException( string.Format( format, args ), paramName );
		}
	}
}
