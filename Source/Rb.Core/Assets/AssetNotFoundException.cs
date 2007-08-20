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
		/// Location setup constructor
		/// </summary>
		public AssetNotFoundException( Location location ) :
			base( string.Format( "No asset found at {0}", location ) )
		{
		}

		/// <summary>
		/// Location and inner exception setup constructor
		/// </summary>
		public AssetNotFoundException( Location location, Exception innerException ) :
			base( string.Format( "No asset found at {0}", location ), innerException )
		{
		}

	}
}
