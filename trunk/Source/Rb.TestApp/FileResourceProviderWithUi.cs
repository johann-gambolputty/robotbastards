
using System.Windows.Forms;
using Rb.Core.Resources;
using Rb.Core.Utils;

namespace Rb.TestApp
{
	/// <summary>
	/// Extends <see cref="FileResourceProvider"/> with a UI
	/// </summary>
	class FileResourceProviderWithUi : FileResourceProvider, IResourceProviderUi
	{
		/// <summary>
		/// Converts this object to a string
		/// </summary>
		public override string ToString( )
		{
			return string.Format( "Files at \"{0}\"", BaseDirectory );
		}

		#region IResourceProviderUi Members

		/// <summary>
		/// Creates an OpenFileDialog to select a single resource
		/// </summary>
		/// <param name="filter">File filter</param>
		/// <returns>Resource path, or null if dialog was cancelled</returns>
		public string GetResourcePath( string filter )
		{
			OpenFileDialog dlg = new OpenFileDialog( );
			dlg.InitialDirectory = BaseDirectory;
			dlg.Filter = filter;

			if ( dlg.ShowDialog( ) == DialogResult.OK )
			{
				return PathHelpers.MakeRelativePath( BaseDirectory, dlg.FileName );
			}
			return null;
		}

		#endregion
	}
}
