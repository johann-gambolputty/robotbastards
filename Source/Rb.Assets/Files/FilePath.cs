using System;
using System.IO;
using Rb.Assets.Interfaces;

namespace Rb.Assets.Files
{
	/// <summary>
	/// Stores a path to a file
	/// </summary>
	[Serializable]
	public class FilePath : LocationPath, IFile
	{
		public FilePath( ILocationManager locations, string path ) :
			base( locations, path )
		{
		}

		/// <summary>
		/// Gets the extension of this location
		/// </summary>
		public override string Extension
		{
			get
			{
				int pos = Name.LastIndexOf( '.' );
				return pos == -1 ? "" : Name.Substring( pos + 1 );
			}
		}

		#region IStreamSource Members

		/// <summary>
		/// Opens a stream from this source
		/// </summary>
		public Stream Open( )
		{
			return new FileStream( Path, FileMode.Open, FileAccess.Read, FileShare.Read );
		}

		#endregion

		#region Protected members

		/// <summary>
		/// Creates a file watcher for this location
		/// </summary>
		protected override FileSystemWatcher CreateWatcher( )
		{
			return new FileSystemWatcher( System.IO.Path.GetDirectoryName( Path ), System.IO.Path.GetFileName( Path ) );
		}

		#endregion
	}
}
