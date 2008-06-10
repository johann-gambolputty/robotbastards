using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Rb.Assets.Interfaces;

namespace Rb.Assets.Files
{
	public class FileSystem : ILocationManager, IXmlSerializable
	{
		public string BaseDirectory
		{
			get { return m_BaseDirectory; }
			set
			{
				string fullPath = Path.GetFullPath( value ) + Path.DirectorySeparatorChar;
				AssetsLog.Info( "Setting base directory \"{0}\"", fullPath );
				m_BaseDirectory = fullPath;
			}
		}

		public string GetFullPath( string path )
		{
			if ( !Path.IsPathRooted( path ) )
			{
				path = Path.Combine( m_BaseDirectory, path );
			}

			return Path.GetFullPath( path );
		}

		#region ILocationManager Members

		public IFolder GetParentFolder( string path )
		{
			DirectoryInfo parentDirectory = Directory.GetParent( path );
			return new FolderPath( this, parentDirectory.FullName );
		}

		public IFolder[] GetSubFolders( string path )
		{
			string[] folderNames = Directory.GetDirectories( path );
			IFolder[] folders = new IFolder[ folderNames.Length ];

			for ( int folderIndex = 0; folderIndex < folderNames.Length; ++folderIndex )
			{
				folders[ folderIndex ] = new FolderPath( this, folderNames[ folderIndex ] );
			}

			return folders;
		}

		public IFile[] GetFiles( string folderPath )
		{
			string[] fileNames = Directory.GetFiles( folderPath );
			IFile[] files = new IFile[ fileNames.Length ];

			for ( int fileIndex = 0; fileIndex < fileNames.Length; ++fileIndex )
			{
				files[ fileIndex ] = new FilePath( this, fileNames[ fileIndex ] );
			}

			return files;
		}

		#endregion


		public bool IsPathValid( string path )
		{
			string fullPath = GetFullPath( path );
			return File.Exists( fullPath ) || Directory.Exists( fullPath );
		}

		public ILocation GetLocation( string path )
		{
			string fullPath = GetFullPath( path );
			if ( Directory.Exists( fullPath ) )
			{
				return new FolderPath( this, fullPath );
			}
			if ( File.Exists( fullPath ) )
			{
				return new FilePath( this, fullPath );
			}
			return null;
		}

		public IFolder GetFolder( string path )
		{
			string fullPath = GetFullPath( path );
			return Directory.Exists( fullPath ) ? new FolderPath( this, fullPath ) : null;
		}

		public IFile GetFile( string path )
		{
			string fullPath = GetFullPath( path );
			return File.Exists( fullPath ) ? new FilePath( this, fullPath ) : null;
		}
		
		#region Private members

		private string m_BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

		#endregion

		#region IXmlSerializable Members

		/// <summary>
		/// Returns null (no schema required for this type)
		/// </summary>
		public System.Xml.Schema.XmlSchema GetSchema( )
		{
			return null;
		}

		/// <summary>
		/// Reads this object from XML
		/// </summary>
		/// <param name="reader">XML reader</param>
		public void ReadXml( XmlReader reader )
		{
			reader.Read( );
			BaseDirectory = reader.GetAttribute( "value" );
			if ( !Directory.Exists( BaseDirectory ) )
			{
				throw new ApplicationException( string.Format( "Asset base directory \"{0}\" does not exist", BaseDirectory ) );
			}
			reader.Skip( );
		}

		/// <summary>
		/// Writes this object to XML
		/// </summary>
		/// <param name="writer">XML writer</param>
		public void WriteXml( XmlWriter writer )
		{
			throw new Exception("The method or operation is not implemented.");
		}


		#endregion
	}
}
