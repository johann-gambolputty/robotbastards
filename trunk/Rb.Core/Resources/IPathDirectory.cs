using System;

namespace Rb.Core.Resources
{
	/// <summary>
	/// Summary description for IPathDirectory.
	/// </summary>
	public interface IPathDirectory
	{
		/// <summary>
		/// Adds a base directory from which resources can be located
		/// </summary>
		void AddBaseDirectory( string dir );
	}
}
