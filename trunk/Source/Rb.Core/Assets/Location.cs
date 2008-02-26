using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;

namespace Rb.Core.Assets
{
	/// <summary>
	/// The location of an asset. A path with some information about it cached
	/// </summary>
	[Serializable]
	public class Location : ISource
	{
		#region Construction

		/// <summary>
		/// Sets up the location
		/// </summary>
		/// <param name="path">Location path</param>
		public Location( string path ) :
			this( LocationManagers.Instance, path )
		{
		}

		/// <summary>
		/// Sets up a location, as relative to an existing location
		/// </summary>
		/// <param name="folder">Folder location</param>
		/// <param name="path">Relative path</param>
		public Location( ISource folder, string path ) :
			this( LocationManagers.Instance, folder, path )
		{
		}
		
		/// <summary>
		/// Sets up the location
		/// </summary>
		/// <param name="locationManagers">Location managers</param>
		/// <param name="path">Location path</param>
		public Location( LocationManagers locationManagers, string path )
		{
			m_Provider	= DetermineLocationManager( locationManagers, path );
			m_Path		= ( m_Provider == null ) ? path : m_Provider.GetFullPath( path );
			m_Name		= path;
			m_Key		= GetLocationKey( m_Path );
		}

		/// <summary>
		/// Sets up a location, as relative to an existing location
		/// </summary>
		/// <param name="locationManagers">Location managers</param>
		/// <param name="folder">Folder location</param>
		/// <param name="path">Relative path</param>
		public Location( LocationManagers locationManagers, ISource folder, string path )
		{
			//	TODO: AP: Maybe add ILocationManager.Combine(), or ISource.Combine()?
			string newPath = System.IO.Path.Combine( folder.Path, path );

			m_Provider	= DetermineLocationManager( locationManagers, newPath );
			m_Path		= ( m_Provider == null ) ? newPath : m_Provider.GetFullPath( newPath );
			m_Name		= newPath;
			m_Key		= GetLocationKey( m_Path );
		}

		/// <summary>
		/// Sets up the location
		/// </summary>
		/// <param name="locationManager">Location manager</param>
		/// <param name="path">Location path</param>
		public Location( ILocationManager locationManager, string path )
		{
			m_Provider = locationManager;
			m_Path = locationManager.GetFullPath( path );
			m_Name = path;
			m_Key = GetLocationKey( m_Path );
		}

		#endregion

		#region Serialization

		/// <summary>
		/// Called when this object is deserialized. Resolves the provider from the specified path
		/// </summary>
		/// <param name="context">Deserialization context</param>
		[OnDeserialized]
		public void OnDeserialized( StreamingContext context )
		{
			m_Provider = DetermineLocationManager( m_Name );
			m_Path = m_Provider == null ? m_Name : m_Provider.GetFullPath( m_Name );
		}

		#endregion

		#region ISource Members

		/// <summary>
		/// Event, invoked if the source changes
		/// </summary>
		public event EventHandler SourceChanged
		{
			add
			{
				if ( m_OnChanged == null )
				{
					AddWatcher( );
				}
				m_OnChanged += value;
			}
			remove
			{
				m_OnChanged -= value;
				if ( m_OnChanged == null )
				{
					RemoveWatcher( );
				}
			}
		}

		/// <summary>
		/// Called when the file/directory at the location changes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void OnLocationChanged( object sender, FileSystemEventArgs args )
		{
			if ( m_OnChanged != null )
			{
				m_OnChanged( sender, args );
			}
		}

		/// <summary>
		/// Returns true if the source is valid
		/// </summary>
		[ Browsable( false ) ]
		public bool Exists
		{
			get { return m_Provider != null; }
		}

		/// <summary>
		/// Returns true if the location ends with the specified extension
		/// </summary>
		/// <param name="ext">Extension to look for</param>
		/// <returns>true if the location path ends with the specified extension</returns>
		public bool HasExtension( string ext )
		{
			return m_Name.EndsWith( ext, StringComparison.CurrentCultureIgnoreCase );
		}

		/// <summary>
		/// Gets a source relative to this one
		/// </summary>
		/// <param name="path">Relative path</param>
		/// <returns>New source</returns>
		public ISource GetRelativeSource( string path )
		{
			string baseDir = Directory ? Path : System.IO.Path.GetDirectoryName( Path );
			return new Location( System.IO.Path.Combine( baseDir, path ) );
		}

		/// <summary>
		/// Opens a stream at this location
		/// </summary>
		/// <returns>Opened stream</returns>
		/// <exception cref="InvalidOperationException">Thrown if no provider exists that can open the location</exception>
		/// <exception cref="AssetNotFoundException">Thrown if no asset exists at the specified location</exception>
		public Stream Open( )
		{
			if ( m_Provider == null )
			{
				throw new InvalidOperationException( string.Format( "No location manager exists to open stream at {0}", this ) );
			}
			return m_Provider.OpenStream( this );
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Determines the provider that can load a given location
		/// </summary>
		/// <param name="location">Asset location</param>
		/// <returns>Returns the provider that </returns>
		public static ILocationManager DetermineLocationManager( string location )
		{
			return DetermineLocationManager( LocationManagers.Instance, location );
		}
		
		/// <summary>
		/// Determines the provider that can load a given location
		/// </summary>
		/// <param name="locationManagers">Location managers</param>
		/// <param name="location">Asset location</param>
		/// <returns>Returns the provider that </returns>
		public static ILocationManager DetermineLocationManager( LocationManagers locationManagers, string location )
		{
			foreach ( ILocationManager manager in locationManagers )
			{
				if ( manager.IsPathValid( location ) )
				{
					return manager;
				}
			}
			return null;
		}

		/// <summary>
		/// Creates a key from an asset location string
		/// </summary>
		/// <param name="location">Location string</param>
		/// <returns>Location key</returns>
		public static int GetLocationKey( string location )
		{
			return location.ToLower( ).GetHashCode( );
		}

		/// <summary>
		/// Gets the hash code for this locaiton
		/// </summary>
		public override int GetHashCode( )
		{
			return m_Key;
		}

		/// <summary>
		/// Returns the location string
		/// </summary>
		public override string ToString( )
		{
			return m_Name;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// Gets the location key
		/// </summary>
		public int Key
		{
			get { return m_Key; }
		}

		/// <summary>
		/// Gets the provider for this location. If this location is not valid, then Provider will return null
		/// </summary>
		public ILocationManager Provider
		{
			get { return m_Provider; }
		}

		/// <summary>
		/// Gets the location name
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Gets the full location path
		/// </summary>
		public string Path
		{
			get { return m_Path; }
		}

		/// <summary>
		/// Returns true if this source is a directory
		/// </summary>
		public bool Directory
		{
			get
			{
				return m_Provider == null ? false : m_Provider.IsDirectoryPath( m_Path );
			}
		}

		#endregion

		#region Private stuff

		private readonly string m_Name;
		private readonly int m_Key;

		[NonSerialized]
		private string m_Path;

		[NonSerialized]
		private ILocationManager m_Provider;

		[NonSerialized]
		private EventHandler m_OnChanged;

		[NonSerialized]
		private FileSystemWatcher m_Watcher;

		/// <summary>
		/// Enables a file system watcher to look for changes at the location
		/// </summary>
		private void AddWatcher( )
		{
			if ( m_Watcher == null )
			{
				if ( Directory )
				{
					m_Watcher = new FileSystemWatcher(Path);
				}
				else
				{
					m_Watcher = new FileSystemWatcher( System.IO.Path.GetDirectoryName( Path ), System.IO.Path.GetFileName( Path ) );
				}
				m_Watcher.Changed += OnLocationChanged;
				m_Watcher.Deleted += OnLocationChanged;
				m_Watcher.Created += OnLocationChanged;
				m_Watcher.EnableRaisingEvents = true;
			}
		}
		
		/// <summary>
		/// Disables the file system watcher
		/// </summary>
		private void RemoveWatcher( )
		{
			if ( m_Watcher != null )
			{
				m_Watcher.Dispose( );
				m_Watcher = null;
			}
		}

		#endregion
	}
}
