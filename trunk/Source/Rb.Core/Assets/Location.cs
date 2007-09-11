using System;
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
		public Location( Location folder, string path ) :
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
			m_Key		= GetLocationKey( m_Path );
		}

		/// <summary>
		/// Sets up a location, as relative to an existing location
		/// </summary>
		/// <param name="locationManagers">Location managers</param>
		/// <param name="folder">Folder location</param>
		/// <param name="path">Relative path</param>
		public Location( LocationManagers locationManagers, Location folder, string path )
		{
			string newPath = folder + path;

			m_Provider	= DetermineLocationManager( locationManagers, newPath );
			m_Path		= ( m_Provider == null ) ? newPath : m_Provider.GetFullPath( newPath );
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
			m_Path = path;
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
			m_Provider = DetermineLocationManager( m_Path );
		}

		#endregion

		#region ISource implementation

		/// <summary>
		/// Returns true if the source is valid
		/// </summary>
		public bool IsValid
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
			return m_Path.EndsWith( ext, StringComparison.CurrentCultureIgnoreCase );
		}

		/// <summary>
		/// Gets a source relative to this one
		/// </summary>
		/// <param name="path">Relative path</param>
		/// <returns>New source</returns>
		public ISource GetSource( string path )
		{
			return new Location( this, path );
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
			return m_Path;
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

		#endregion

		#region Private stuff

		private readonly string m_Path;
		private readonly int m_Key;

		[NonSerialized]
		private ILocationManager m_Provider;

		#endregion
	}
}
