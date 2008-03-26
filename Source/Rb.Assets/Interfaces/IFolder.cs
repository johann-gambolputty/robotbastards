
namespace Rb.Assets.Interfaces
{
	/// <summary>
	/// A folder is a collection of files and sub-folders
	/// </summary>
	public interface IFolder : ILocation
	{
		/// <summary>
		/// Gets a location relative to this folder
		/// </summary>
		ILocation GetLocation( string path );

		/// <summary>
		/// Gets a file relative to this folder
		/// </summary>
		IFile GetFile( string path );

		/// <summary>
		/// Gets a folder relative to this folder
		/// </summary>
		/// <returns>Returns null if the folder does not exist. Otherwise, returns </returns>
		IFolder GetFolder( string path );

		/// <summary>
		/// Returns true if one or more items in this folder match the specified search string
		/// </summary>
		bool Contains( string search );

		/// <summary>
		/// Gets any files contained in this folder
		/// </summary>
		IFile[] Files
		{
			get;
		}

		/// <summary>
		/// Gets any sub-folders contained in this folder
		/// </summary>
		IFolder[] SubFolders
		{
			get;
		}
	}
}
