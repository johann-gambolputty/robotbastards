using System;
using System.Collections;

namespace Rb.Core.Resources
{
	/// <summary>
	/// Standard file system implementation of ResourceProvider. Provides support for IPathDirectory also
	/// </summary>
	public class FileResourceProvider : ResourceProvider, IPathDirectory
	{
		#region	ResourceProvider Methods

		/// <summary>
		/// Opens a stream from a path
		/// </summary>
		/// <param name="path"> Stream identifier (e.g. filesystem path). If the provider resolves the path, the resolved string is stored back in path </param>
		/// <returns> Returns the opened stream, or null if the resource could not be found </returns>
		/// <remarks>
		/// If the path is rooted 
		/// </remarks>
		public override System.IO.Stream Open( ref string path )
		{
			//	If the path is rooted (absolute path), then just try to open the resource, regardless of the base directories
			if ( System.IO.Path.IsPathRooted( path ) )
			{
				return System.IO.File.OpenRead( path );
			}

			foreach ( string baseDir in m_BaseDirs )
			{
				string fullDir = baseDir + path;
				if ( System.IO.File.Exists( fullDir ) )
				{
					path = fullDir;
					return System.IO.File.OpenRead( fullDir );
				}
			}

			return null;
		}

		/// <summary>
		/// Sets up this provider from an XML definition
		/// </summary>
		/// <param name="element"> XML element that created this provider </param>
		public override void Setup( System.Xml.XmlElement element )
		{
			foreach ( System.Xml.XmlElement baseDirNode in element.SelectNodes( "baseDir" ) )
			{
				AddBaseDirectory( baseDirNode.Attributes[ "value" ].Value );
			}
		}

		/// <summary>
		/// Returns true if the specified directory is available using this provider
		/// </summary>
		public override bool DirectoryExists( ref string directory )
		{
			foreach ( string baseDir in m_BaseDirs )
			{
				string fullDir = baseDir + directory;
				if ( System.IO.Directory.Exists( fullDir ) )
				{
					directory = fullDir;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Returns true if the named stream exists in this provider
		/// </summary>
		public override bool StreamExists( string path )
		{
			return System.IO.File.Exists( path );
		}

		#endregion

		#region	IPathDirectory Methods

		/// <summary>
		/// Adds a base directory from which resources can be located
		/// </summary>
		public void AddBaseDirectory( string dir )
		{
			string fullPath = System.IO.Path.GetFullPath( dir ) + '\\';

			ResourcesLog.Info( "Adding base directory \"{0}\"", fullPath );
			m_BaseDirs.Add( fullPath );
		}

		#endregion

		#region	Private stuff

		private ArrayList m_BaseDirs = new ArrayList( );

		#endregion

	}
}
