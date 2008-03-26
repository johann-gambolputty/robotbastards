using System;
using System.IO;
using Rb.Assets.Interfaces;

namespace Rb.Assets.Files
{
	[Serializable]
	public class FolderPath : LocationPath, IFolder
	{
		public FolderPath( ILocationManager locations, string path ) :
			base( locations, path )
		{
		}

		#region IFolder Members

		public ILocation GetLocation( string path )
		{
			string fullPath = System.IO.Path.Combine( Path, path );
			return Locations.Instance.GetFile( fullPath ) ?? ( ILocation )Locations.Instance.GetFolder( fullPath );
		}

		public IFile GetFile( string path )
		{
			string fullPath = System.IO.Path.Combine( Path, path );
			return Locations.Instance.GetFile( fullPath );
		}

		public IFolder GetFolder( string path )
		{
			string fullPath = System.IO.Path.Combine( Path, path );
			return Locations.Instance.GetFolder( fullPath );
		}

		/// <summary>
		/// Returns true if one or more items in this folder match the specified search string
		/// </summary>
		public bool Contains( string search )
		{
			return Directory.GetFiles( Path, search ).Length > 0;
		}

		/// <summary>
		/// Gets the set of files in this folder
		/// </summary>
		public IFile[] Files
		{
			get
			{
				if ( m_Files == null )
				{
					m_Files = LocationManager.GetFiles( Path );
				}
				return m_Files;
			}
		}

		/// <summary>
		/// Gets the set of sub-folders in this folder
		/// </summary>
		public IFolder[] SubFolders
		{
			get
			{
				if ( m_Folders == null )
				{
					m_Folders = LocationManager.GetSubFolders( Path );
				} 
				return m_Folders;
			}
		}

		#endregion
		
		#region Protected members

		/// <summary>
		/// Creates a file watcher to observe changes to the folder
		/// </summary>
		protected override FileSystemWatcher CreateWatcher( )
		{
			return new FileSystemWatcher( Path );
		}

		#endregion

		#region Private members

		[NonSerialized]
		private IFile[] m_Files;

		[NonSerialized]
		private IFolder[] m_Folders;

		#endregion
	}
}
