using System;

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
	}
}
