using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Core.Assets
{
	/// <summary>
	/// Equivalent to the FileNotFoundException
	/// </summary>
	class AssetNotFoundException : Exception
	{
		/// <summary>
		/// Source setup constructor
		/// </summary>
		public AssetNotFoundException( ISource source ) :
			base( string.Format( "No asset found at {0}", source ) )
		{
		}

		/// <summary>
		/// Location and inner exception setup constructor
		/// </summary>
		public AssetNotFoundException( ISource source, Exception innerException ) :
			base( string.Format( "No asset found at {0}", source ), innerException )
		{
		}

	}
}
