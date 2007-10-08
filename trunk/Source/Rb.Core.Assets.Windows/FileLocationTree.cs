using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace Rb.Core.Assets.Windows
{
	public class FileLocationTree : ILocationTree
	{
		public LocationProperty NameProperty
		{
			get { return m_NameProperty;  }
		}

		public LocationProperty SizeProperty
		{
			get { return m_SizeProperty; }
		}

		public LocationProperty TypeProperty
		{
			get { return m_TypeProperty; }
		}

		public LocationProperty ModifiedProperty
		{
			get { return m_ModifiedProperty; }
		}

		public LocationProperty CreatedProperty
		{
			get { return m_CreatedProperty; }
		}


		private readonly LocationProperty m_NameProperty		= new LocationProperty( "Name", CompareStrings, 100 );
		private readonly LocationProperty m_SizeProperty		= new LocationProperty( "Size", CompareLongs, 50 );
		private readonly LocationProperty m_TypeProperty		= new LocationProperty( "Type", CompareStrings, 50 );
		private readonly LocationProperty m_ModifiedProperty	= new LocationProperty( "Modified", CompareDateTimes, 60 );
		private readonly LocationProperty m_CreatedProperty		= new LocationProperty( "Created", CompareDateTimes, 60 );

		private static int CompareLongs( object val0, object val1 )
		{
			long i0 = ( long )val0;
			long i1 = ( long )val1;

			return ( i0 < i1 ) ? -1 : ( ( i0 > i1 ) ? 1 : 0 );
		}
		
		private static int CompareDateTimes( object val0, object val1 )
		{
			DateTime i0 = ( DateTime )val0;
			DateTime i1 = ( DateTime )val1;

			return ( i0 < i1 ) ? -1 : ( ( i0 > i1 ) ? 1 : 0 );
		}

		private static int CompareStrings( object s0, object s1 )
		{
			return string.Compare( ( string )s0, ( string )s1 );
		}

		public FileLocationTree( ILocationManager locationManager, string defaultPath )
		{
			m_Locations = locationManager;

			m_Images = new ImageList( );
			m_FolderImage = AddImage( Windows.Properties.Resources.Folder, true );
			m_FolderOpenImage = AddImage( Windows.Properties.Resources.FolderOpen, true );

			string[] driveNames = Environment.GetLogicalDrives( );
			m_Drives = new Folder[ driveNames.Length ];

			for ( int driveIndex = 0; driveIndex < driveNames.Length; ++driveIndex )
			{
				string driveName = driveNames[ driveIndex ];
				m_Drives[ driveIndex ] = new Folder( null, this, driveName );
			}

			m_Properties = new LocationProperty[]
				{
					m_NameProperty,
					m_SizeProperty,
					m_TypeProperty,
					m_ModifiedProperty,
					m_CreatedProperty
				};

			m_DefaultFolder = m_Drives[ 0 ];
			try
			{
				OpenFolder( defaultPath, out m_DefaultFolder );
				return;
			}
			catch ( Exception ex )
			{
				AssetsLog.Exception( ex, "Failed to open default folder \"{0}\"", defaultPath );
			}

		}

		public void OpenFolder( string path, out LocationTreeFolder folder )
		{
		    string[] pathParts = path.Split( new char[] { '/', '\\' } );

			folder = FindDriveFolder( pathParts[ 0 ] );

			for ( int partIndex = 1; partIndex < pathParts.Length; ++partIndex )
			{
				if ( string.IsNullOrEmpty( pathParts[ partIndex ] ) )
				{
					continue;
				}

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

		/// <summary>
		/// Builds a path from a base path and a new (possibly relative) path
		/// </summary>
		/// <param name="basePath">Base path</param>
		/// <param name="newPath">New path</param>
		/// <returns>Combined path</returns>
		public string GetFullPath( string basePath, string newPath )
		{
			return Path.GetFullPath( Path.Combine( basePath, newPath ) );
		}

		/// <summary>
		/// Returns true if the specified path refers to an existing folder
		/// </summary>
		/// <param name="path">Path to check</param>
		/// <returns>true if path refers to an existing folder</returns>
		public bool IsFolder( string path )
		{
			return Directory.Exists( path );
		}

		/// <summary>
		/// Returns true if the specified path refers to an existing item
		/// </summary>
		/// <param name="path">Path to check</param>
		/// <returns>true if path refers to an existing item</returns>
		public bool IsItem( string path )
		{
			return File.Exists( path );
		}

		/// <summary>
		/// Opens a folder given a specified path
		/// </summary>
		/// <param name="path">Path to folder</param>
		/// <returns>Folder</returns>
		public LocationTreeFolder OpenFolder( string path )
		{
			LocationTreeFolder result;
			OpenFolder( path, out result );
			return result;
		}

		#endregion

		#region Public members

		/// <summary>
		/// Gets the location manager
		/// </summary>
		public ILocationManager Locations
		{
			get { return m_Locations; }
		}
	
		public int GetFolderIcon( bool open )
		{
			return open ? m_FolderOpenImage : m_FolderImage;
		}

		public int GetFileIcon( string path )
		{
			string ext = Path.GetExtension( path );

			int index = m_Images.Images.IndexOfKey( ext );
			if ( index != -1 )
			{
				return index;
			}

			index = m_Images.Images.Count;
			Icon icon = FileIconReader.GetFileIcon( path, false, true );
			m_Images.Images.Add( ext, icon );
			return index;
		}
		#endregion

		private readonly ILocationManager m_Locations;
		private readonly LocationTreeFolder[] m_Drives;
		private readonly LocationTreeFolder m_DefaultFolder;
		private readonly LocationProperty[] m_Properties;
		private readonly int m_FolderImage;
		private readonly int m_FolderOpenImage;
		private readonly ImageList m_Images = new ImageList( );
	
		private int AddImage( Bitmap bmp, bool makeTransparent )
		{
			if ( makeTransparent )
			{
				bmp.MakeTransparent( Color.Magenta );
			}

			int index = m_Images.Images.Count;
			m_Images.Images.Add( bmp );
			return index;
		}

		public class Folder : LocationTreeFolder
		{
			public Folder( LocationTreeFolder parent, FileLocationTree tree, string path ) :
				base( parent, new Location( tree.Locations, path ), tree.GetFolderIcon( false ), tree.GetFolderIcon( true ) )
			{
				m_Context = tree;
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
			private readonly FileLocationTree m_Context;

			private void PopulateItems( )
			{
				m_Items = new List< LocationTreeItem >( );

				string[] files = Directory.GetFiles( Path );
				foreach ( string file in files )
				{
					int img = m_Context.GetFileIcon( file );

					LocationTreeItem item = new LocationTreeItem( this, new Location( m_Context.Locations, file ), img, img );

					item[ m_Context.NameProperty ]		= item.Name;
					item[ m_Context.TypeProperty ]		= System.IO.Path.GetExtension( file );
					item[ m_Context.SizeProperty ]		= new FileInfo( file ).Length;
					item[ m_Context.ModifiedProperty ]	= File.GetLastWriteTime( file );
					item[ m_Context.CreatedProperty ]	= File.GetCreationTime( file );

					m_Items.Add( item );
				}
			}

			private void PopulateFolders( )
			{
				m_Folders = new List< LocationTreeFolder >( );

				string[] directories = Directory.GetDirectories( Path );
				foreach ( string directory in directories )
				{
					Folder folder = new Folder( this, m_Context, directory );
					
					folder[ m_Context.NameProperty ]		= folder.Name;
					folder[ m_Context.TypeProperty ]		= "Folder";
					//folder[ m_Context.SizeProperty ]		= ( long )0;
					folder[ m_Context.ModifiedProperty ]	= Directory.GetLastWriteTime( directory );
					folder[ m_Context.CreatedProperty ]		= Directory.GetCreationTime( directory );

					m_Folders.Add( folder );
				}
			}
		}
	}
}
