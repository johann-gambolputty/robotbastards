namespace Rb.Core.Resources
{
	/// <summary>
	/// Standard file system implementation of ResourceProvider. Provides support for IPathDirectory also
	/// </summary>
	public class FileResourceProvider : ResourceProvider, IPathDirectory
	{
		#region	IPathDirectory Methods

		/// <summary>
		/// Access to the base directory
		/// </summary>
		public string BaseDirectory
		{
			set
			{
				string fullPath = System.IO.Path.GetFullPath( value ) + '\\';
				ResourcesLog.Info( "Setting base directory \"{0}\"", fullPath );
				m_BaseDir = fullPath;
			}
			get
			{
				return m_BaseDir;
			}
		}

		#endregion

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

			string fullDir = BaseDirectory + path;
			if ( System.IO.File.Exists( fullDir ) )
			{
				path = fullDir;
				return System.IO.File.OpenRead( fullDir );
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
				BaseDirectory = baseDirNode.Attributes[ "value" ].Value;
			}
		}

		/// <summary>
		/// Returns true if the specified directory is available using this provider
		/// </summary>
		public override bool DirectoryExists( ref string directory )
		{
			string fullDir = BaseDirectory + directory;
			if ( System.IO.Directory.Exists( fullDir ) )
			{
				directory = fullDir;
				return true;
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

		#region	Private stuff

		private string m_BaseDir = "";

		#endregion

	}
}
