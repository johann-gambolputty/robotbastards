namespace Rb.Assets.Interfaces
{
	/// <summary>
	/// File system interface
	/// </summary>
	public interface ILocationManager
	{
		/// <summary>
		/// Returns true if a specified path refers to an existing location
		/// </summary>
		bool IsPathValid( string path );
		
		/// <summary>
		/// Gets the location (folder or file) at a specified path
		/// </summary>
		/// <param name="path">Location path</param>
		/// <returns>Returns the specified location, or null if nothing exists at the path</returns>
		ILocation GetLocation( string path );

		/// <summary>
		/// Returns the folder at a specified path
		/// </summary>
		/// <param name="path">Folder path</param>
		/// <returns>Returns the specified folder, or null if no folder exists at the path</returns>
		IFolder GetFolder( string path );
		
		/// <summary>
		/// Returns the file at a specified path
		/// </summary>
		/// <param name="path">File path</param>
		/// <returns>Returns the specified file, or null if no files exists at the path</returns>
		IFile GetFile( string path );
		
		/// <summary>
		/// Gets the folder "above" a specified folder or file path
		/// </summary>
		/// <param name="path">Path</param>
		/// <returns>Returns the parent to the specified folder or file, or null if the path specified is a root folder</returns>
		IFolder GetParentFolder( string path );

		/// <summary>
		/// Gets the set of sub-folders at a specified path
		/// </summary>
		/// <param name="path">Folder path</param>
		/// <returns>The array of sub-folders at the specified path</returns>
		IFolder[] GetSubFolders( string path );

		/// <summary>
		/// Returns the set of files at a specified folder path
		/// </summary>
		/// <param name="folderPath">Folder path</param>
		/// <returns>The array of files at the specified folder path</returns>
		IFile[] GetFiles( string folderPath );
	}
}
