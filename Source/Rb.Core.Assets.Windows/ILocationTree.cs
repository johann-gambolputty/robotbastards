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

	}
}
