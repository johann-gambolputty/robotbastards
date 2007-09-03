using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Rb.Core.Assets
{
	/// <summary>
	/// Loads assets from the file system
	/// </summary>
	/// <remarks>
	/// The FileAssetProvider can have one base directory (defaults to the working directory when the object is
	/// created). If an asset location is relative, then it's relative to this path. e.g. If the base directory
	/// is "c:\temp\test", and the location is "..\asset.txt", then the resolved path is "c:\temp\asset.txt"
	/// </remarks>
	public class FileLocationManager : ILocationManager
	{
		/// <summary>
		/// Access to the base directory
		/// </summary>
		public string BaseDirectory
		{
			set
			{
				string fullPath = Path.GetFullPath( value ) + '\\';
				AssetsLog.Info( "Setting base directory \"{0}\"", fullPath );
				m_BaseDir = fullPath;
			}
			get
			{
				return m_BaseDir;
			}
		}

		#region ILocationManager Members

		/// <summary>
		/// Gets the full path of a location
		/// </summary>
		/// <param name="location">Location</param>
		/// <returns>Full path of the location (</returns>
		public string GetFullPath( string location )
		{
			if ( Path.IsPathRooted( location ) )
			{
				return location;
			}

			return Path.GetFullPath( m_BaseDir + location );
		}

		/// <summary>
		/// Returns true if the specified string is valid
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public bool IsPathValid( string path )
		{
			return File.Exists( GetFullPath( path ) );
		}

		/// <summary>
		/// Opens a stream at a given location
		/// </summary>
		/// <param name="location">Asset location</param>
		/// <returns>Returns a stream</returns>
		/// <exception cref="AssetNotFoundException">Thrown if no asset exists at the specified location</exception>
		public Stream OpenStream( Location location )
		{
			try
			{
				return File.OpenRead( location.ToString( ) );
			}
			catch ( Exception ex )
			{
				throw new AssetNotFoundException( location, ex );
			}
		}

		#endregion
		
		#region Private stuff

		private string m_BaseDir = Directory.GetCurrentDirectory( );

		#endregion

	}
}
