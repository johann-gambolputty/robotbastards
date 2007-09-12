using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Rb.Core.Utils;
using Rb.Log;


namespace Rb.Core.Assets.Windows
{
	/// <summary>
	/// Icon reader
	/// </summary>
	public class IconReader
	{
		public const uint SmallIcon = 0x1;

		[ StructLayout( LayoutKind.Sequential ) ]
		private struct ShellFileInfo
		{
			public IntPtr	hIcon;
			public int		iIndex;
			public uint		dwAttributes; 

			[MarshalAs( UnmanagedType.ByValTStr, SizeConst = 260 ) ]
			public string	szDisplayName; 

			[MarshalAs( UnmanagedType.ByValTStr, SizeConst =80 ) ]
			public string	szTypeName; 
		};

		[Flags]
		private enum IconFlags : uint
		{
			SmallIcon = 0x1,
			OpenIcon = 0x2,
			UseFileAttributes = 0x10,
			AddOverlays = 0x20,
			Icon = 0x100,
			LinkOverlay = 0x8000
		}

		private const uint FileAttributeNormal = 0x80;
		private const uint FileAttributeDirectory = 0x10;


		[DllImport("User32.dll")]
		private static extern int DestroyIcon( IntPtr hIcon );

		[DllImport("Shell32.dll")]
		private static extern IntPtr SHGetFileInfo( string pszPath, uint dwFileAttributes, ref ShellFileInfo psfi, uint cbFileInfo, uint uFlags );

		public static Icon GetFileIcon( string filePath, bool addLinkOverlay, bool small ) 
		{
			IconFlags flags = ( IconFlags.Icon | IconFlags.AddOverlays | IconFlags.UseFileAttributes );
			if ( addLinkOverlay )
			{
				flags |= IconFlags.LinkOverlay;
			}
			if ( small )
			{
				flags |= IconFlags.SmallIcon;
			}

			ShellFileInfo shellFileInfo = new ShellFileInfo( );

			SHGetFileInfo( filePath, FileAttributeNormal, ref shellFileInfo, ( uint )Marshal.SizeOf( shellFileInfo ), ( uint )flags );

			Icon icon = (Icon)Icon.FromHandle( shellFileInfo.hIcon ).Clone( );

			DestroyIcon( shellFileInfo.hIcon );

			return icon;
		}

		public static Icon GetFolderIcon( string path, bool small, bool open )
		{
			IconFlags flags = ( IconFlags.Icon | IconFlags.AddOverlays | IconFlags.UseFileAttributes );
			if ( open )
			{
				flags |= IconFlags.OpenIcon;
			}
			if ( small )
			{
				flags |= IconFlags.SmallIcon;
			}

			ShellFileInfo shellFileInfo = new ShellFileInfo( );

			SHGetFileInfo( path, FileAttributeDirectory, ref shellFileInfo, ( uint )Marshal.SizeOf( shellFileInfo ), ( uint )flags );

			Icon icon = (Icon)Icon.FromHandle( shellFileInfo.hIcon ).Clone( );

			DestroyIcon( shellFileInfo.hIcon );

			return icon;
		}
	}

	public class FileLocationTree : ILocationTree
	{
		private readonly int m_FolderImage;
		private readonly int m_OpenFolderImage;

		private int AddImage( Icon icon )
		{
			int index = m_Images.Images.Count;
			m_Images.Images.Add( icon );
			return index;
		}

		public FileLocationTree( ILocationManager locationManager, string defaultPath )
		{
			m_Images = new ImageList( );
			m_FolderImage = AddImage( Windows.Properties.Resources.Folder );
			m_OpenFolderImage = AddImage( Windows.Properties.Resources.FolderOpen );

			string[] driveNames = Environment.GetLogicalDrives( );
			m_Drives = new Folder[ driveNames.Length ];

			for ( int driveIndex = 0; driveIndex < driveNames.Length; ++driveIndex )
			{
				string driveName = driveNames[ driveIndex ];
				m_Drives[ driveIndex ] = new Folder( null, locationManager, driveName, driveName, m_FolderImage, m_OpenFolderImage );
			}

			m_Properties = new LocationProperty[]
				{
					new LocationProperty( "Name", null, 100 ),
					new LocationProperty( "Size", null, 50 ),
					new LocationProperty( "Type", null, 50 ),
					new LocationProperty( "Modified", null, 60 ),
					new LocationProperty( "Created", null, 60 )
				};

			m_DefaultFolder = m_Drives[ 0 ];
			try
			{
				OpenFolder( defaultPath, out m_DefaultFolder );
				return;
			}
			catch ( Exception ex )
			{
				AssetsLog.Error( "Failed to open default folder \"{0}\"", defaultPath );
				ExceptionUtils.ToLog( AssetsLog.GetSource( Severity.Error ), ex );
			}

		}

		public LocationTreeFolder OpenFolder( string path )
		{
			LocationTreeFolder result;
			OpenFolder( path, out result );
			return result;
		}

		public void OpenFolder( string path, out LocationTreeFolder folder )
		{
		    string[] pathParts = path.Split( new char[] { '/', '\\' } );

			folder = FindDriveFolder( pathParts[ 0 ] );

			for ( int partIndex = 1; partIndex < pathParts.Length; ++partIndex )
			{
				LocationTreeFolder nextFolder = folder.FindFolder( pathParts[ partIndex ] );
				if ( nextFolder == null )
				{
					throw new ArgumentException( string.Format( "Path \"{0}\" is invalid - \"{1}\" does not exist in \"{2}\"", path, pathParts[ partIndex ], pathParts[ partIndex - 1 ] ), "path" );
				}
				folder = nextFolder;
			}
		}

		public LocationTreeFolder FindDriveFolder( string drive )
		{
			char driveLetter = char.ToUpper( drive[ 0 ] );
			foreach ( LocationTreeFolder folder in m_Drives )
			{
				if ( driveLetter == folder.Name[ 0 ] )
				{
					return folder;
				}
			}
			throw new ArgumentException( string.Format( "Drive \"{0}\" does not exist", drive ), "drive" );
		}

		#region ILocationTree Members

		/// <summary>
		/// Gets the roots of the tree (file system drives)
		/// </summary>
		public LocationTreeFolder[] Roots
		{
			get { return m_Drives; }
		}

		/// <summary>
		/// Gets the default folder
		/// </summary>
		public LocationTreeFolder DefaultFolder
		{
			get { return m_DefaultFolder; }
		}

		/// <summary>
		/// Gets location properties (e.g. "Name", "Modification Date", ...)
		/// </summary>
		public LocationProperty[] Properties
		{
			get { return m_Properties; }
		}

		/// <summary>
		/// Gets the images associated with items and folders in this tree
		/// </summary>
		public ImageList Images
		{
			get { return m_Images; }
		}

		#endregion

		private readonly LocationTreeFolder[] m_Drives;
		private readonly LocationTreeFolder m_DefaultFolder;
		private readonly LocationProperty[] m_Properties;
		private ImageList m_Images;

		public class Folder : LocationTreeFolder
		{
			public Folder( LocationTreeFolder parent, ILocationManager manager, string path, int image, int selectedImage ) :
				base( parent, new Location( manager, path ), image, selectedImage )
			{
				m_Locations = manager;
			}

			public Folder( LocationTreeFolder parent, ILocationManager manager, string path, string name, int image, int selectedImage ) :
				base( parent, new Location( manager, path ), name, image, selectedImage )
			{
				m_Locations = manager;
			}

			public override IEnumerable< LocationTreeItem > Items
			{
				get
				{
					if ( m_Items== null )
					{
						PopulateItems( );
					}
					return m_Items;
				}
			}

			public override IEnumerable< LocationTreeFolder > Folders
			{
				get
				{
					if ( m_Folders == null )
					{
						PopulateFolders( );
					}
					return m_Folders;
				}
			}

			private List< LocationTreeItem > m_Items;
			private List< LocationTreeFolder > m_Folders;
			private readonly ILocationManager m_Locations;

			private void PopulateItems( )
			{
				m_Items = new List< LocationTreeItem >( );

				string[] files = System.IO.Directory.GetFiles( Path );
				foreach ( string file in files )
				{
					m_Items.Add( new LocationTreeItem( this, new Location( m_Locations, file ), -1, -1 ) );
				}
			}

			private void PopulateFolders( )
			{
				m_Folders = new List< LocationTreeFolder >( );

				string[] directories = System.IO.Directory.GetDirectories( Path );
				foreach ( string directory in directories )
				{
					m_Folders.Add( new Folder( this, m_Locations, directory, Image, SelectedImage ) );
				}
			}
		}
	}
}
