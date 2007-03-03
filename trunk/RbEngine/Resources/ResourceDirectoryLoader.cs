using System;

namespace RbEngine.Resources
{
	/// <summary>
	/// Loads resources from directories
	/// </summary>
	public abstract class ResourceDirectoryLoader
	{
		#region	Directory loading

		/// <summary>
		/// Loads resources from a directory
		/// </summary>
		public abstract Object Load( ResourceProvider provider, string directory );

		/// <summary>
		/// Returns true if this loader can load the specified directory
		/// </summary>
		public abstract bool CanLoadDirectory( ResourceProvider provider, string directory );

		#endregion
	}
}
