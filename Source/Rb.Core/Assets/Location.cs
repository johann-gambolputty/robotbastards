using System;
using System.IO;

namespace Rb.Core.Assets
{
	/// <summary>
	/// The location of an asset. A path with some cached information about it
	/// </summary>
	public class Location
	{
		#region Construction

		/// <summary>
		/// Sets up the location
		/// </summary>
		/// <param name="path">Location path</param>
		public Location( string path )
		{
			m_Provider	= DetermineProvider( path );
			m_Path		= ( m_Provider == null ) ? path : m_Provider.GetFullPath( path );
			m_Key		= GetLocationKey( m_Path );
		}

		/// <summary>
		/// Sets up a location, as relative to an existing location
		/// </summary>
		/// <param name="folder">Folder location</param>
		/// <param name="path">Relative path</param>
		public Location( Location folder, string path )
		{
			m_Provider	= DetermineProvider( folder + path );
			m_Path		= ( m_Provider == null ) ? path : m_Provider.GetFullPath( path );
			m_Key		= GetLocationKey( m_Path );
		}

		#endregion

		#region Public methods

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
		/// Helper method that uses the associated asset provider to create a stream from this location
		/// </summary>
		/// <returns>Opened stream</returns>
		/// <exception cref="InvalidOperationException">Thrown if no provider exists that can open the location</exception>
		/// <exception cref="AssetNotFoundException">Thrown if no asset exists at the specified location</exception>
		public Stream OpenStream( )
		{
			if ( m_Provider == null )
			{
				throw new InvalidOperationException( string.Format( "No provider exists to open stream at {0}", this ) );
			}
			return m_Provider.OpenStream( this );
		}

		/// <summary>
		/// Helper method, that creates a location relative to this one, and opens a stream from it
		/// </summary>
		/// <param name="path">Path relative to this location</param>
		/// <returns>Returns the opened stream, or null</returns>
		/// <exception cref="InvalidOperationException">Thrown if no provider exists that can open the location</exception>
		/// <exception cref="AssetNotFoundException">Thrown if no asset exists at the specified location</exception>
		public Stream OpenStream( string path )
		{
			if ( m_Provider == null )
			{
				throw new InvalidOperationException( string.Format( "No provider exists to open stream at {0}", this ) );
			}
			return new Location( this, path ).OpenStream( );
		}

		/// <summary>
		/// Determines the provider that can load a given location
		/// </summary>
		/// <param name="location">Asset location</param>
		/// <returns>Returns the provider that </returns>
		public static IAssetProvider DetermineProvider( string location )
		{
			foreach ( IAssetProvider provider in AssetProviders.Instance )
			{
				if ( provider.IsPathValid( location ) )
				{
					return provider;
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
		public IAssetProvider Provider
		{
			get { return m_Provider; }
		}

		/// <summary>
		/// Returns true if the location is valid
		/// </summary>
		public bool Valid
		{
			get { return m_Provider != null; }
		}

		#endregion

		#region Private stuff

		private readonly string			m_Path;
		private readonly int			m_Key;
		private readonly IAssetProvider	m_Provider;

		#endregion
	}
}
