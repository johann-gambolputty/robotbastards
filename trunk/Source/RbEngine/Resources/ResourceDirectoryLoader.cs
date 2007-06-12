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
		/// <param name="provider">The resource provider that can open files in the directory</param>
		/// <param name="directory">The path to the directory</param>
		/// <returns>Returns the loaded object</returns>
		public abstract Object Load( ResourceProvider provider, string directory );

		/// <summary>
		/// Loads resources from a directory
		/// </summary>
		/// <param name="provider">The resource provider that can open files in the directory</param>
		/// <param name="directory">The path to the directory</param>
		/// <param name="parameters">Load parameters</param>
		/// <returns>Returns the loaded object</returns>
		public abstract Object Load( ResourceProvider provider, string directory, LoadParameters parameters );

		/// <summary>
		/// Returns true if this loader can load the specified directory
		/// </summary>
		public abstract bool CanLoadDirectory( ResourceProvider provider, string directory );

		#endregion
	}
}
