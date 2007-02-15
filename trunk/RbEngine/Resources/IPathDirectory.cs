using System;

namespace RbEngine.Resources
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
