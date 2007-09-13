using System.Windows.Forms;
using Rb.Core.Assets;

namespace Rb.Core.Assets.Windows
{
	/// <summary>
	/// Interface for browsing tree-based location managers (e.g. <see cref="FileLocationManager"/>)
	/// </summary>
	public interface ILocationTree
	{
		/// <summary>
		/// Gets the roots of the tree (e.g. drives in the file system location tree)
		/// </summary>
		LocationTreeFolder[] Roots
		{
			get;
		}

		/// <summary>
		/// Gets the default folder
		/// </summary>
		LocationTreeFolder DefaultFolder
		{
			get;
		}

		/// <summary>
		/// Gets location properties (e.g. "Name", "Modification Date", ...)
		/// </summary>
		LocationProperty[] Properties
		{
			get;
		}

		/// <summary>
		/// Gets the images associated with items and folders in this tree
		/// </summary>
		ImageList Images
		{
		    get;
		}

		/// <summary>
		/// Builds a path from a base path and a new (possibly relative) path
		/// </summary>
		/// <param name="basePath">Base path</param>
		/// <param name="newPath">New path</param>
		/// <returns>Combined path</returns>
		string GetFullPath( string basePath, string newPath );

		/// <summary>
		/// Returns true if the specified path refers to an existing folder
		/// </summary>
		/// <param name="path">Path to check</param>
		/// <returns>true if path refers to an existing folder</returns>
		bool IsFolder( string path );

		/// <summary>
		/// Returns true if the specified path refers to an existing item
		/// </summary>
		/// <param name="path">Path to check</param>
		/// <returns>true if path refers to an existing item</returns>
		bool IsItem( string path );

		/// <summary>
		/// Opens a folder given a specified path
		/// </summary>
		/// <param name="path">Path to folder</param>
		/// <returns>Folder</returns>
		LocationTreeFolder OpenFolder( string path );

	}
}
