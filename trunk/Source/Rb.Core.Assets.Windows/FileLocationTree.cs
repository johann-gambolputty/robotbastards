using System;
using System.Collections.Generic;


namespace Rb.Core.Assets.Windows
{
	public class FileLocationTree : ILocationTree
	{
		public FileLocationTree( ILocationManager locationManager )
		{
			string[] driveNames = Environment.GetLogicalDrives( );
			m_Drives = new Folder[ driveNames.Length ];

			for ( int driveIndex = 0; driveIndex < driveNames.Length; ++driveIndex )
			{
				string driveName = driveNames[ driveIndex ];
				m_Drives[ driveIndex ] = new Folder( null, locationManager, driveName, driveName );
			}
		}

		#region ILocationTree Members

		public LocationTreeFolder[] Roots
		{
			get { return m_Drives; }
		}

		public LocationTreeFolder DefaultFolder
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		#endregion

		private readonly Folder[] m_Drives;

		public class Folder : LocationTreeFolder
		{
			public Folder( LocationTreeFolder parent, ILocationManager manager, string path ) :
				base( parent, new Location( manager, path ) )
			{
			}

			public Folder( LocationTreeFolder parent, ILocationManager manager, string path, string name ) :
				base( parent, new Location( manager, path ), name )
			{

			}

			public override IEnumerable< LocationTreeNode > Children
			{
				get
				{
					if ( m_Children == null )
					{
						PopulateChildren( );
					}
					return m_Children;
				}
			}

			private List< LocationTreeNode > m_Children;

			private void PopulateChildren( )
			{
				m_Children = new List< LocationTreeNode >( );

				string[] directories = System.IO.Directory.GetDirectories( Path );

				string[] files = System.IO.Directory.GetFiles( Path );
				foreach ( string file in files )
				{
					m_Children.Add( new LocationTreeItem( this, ) );
				}
			}
		}
	}
}
