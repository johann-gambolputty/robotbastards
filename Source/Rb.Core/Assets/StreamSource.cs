using System;
using System.IO;
using System.Text;

namespace Rb.Core.Assets
{
	/// <summary>
	/// Stream source
	/// </summary>
	public class StreamSource : ISource
	{
		#region Construction

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="stream">Stream source</param>
		/// <param name="path">Path of the stream (should include extension)</param>
		public StreamSource( Stream stream, string path )
		{
			m_Name = System.IO.Path.GetFileName( path );
			m_Path = path;
			m_Stream = new UndyingMemoryStream( stream );
			stream.Close( );
		}

		/// <summary>
		/// Setup constructor. Creates stored stream from a string
		/// </summary>
		/// <param name="streamContents">Stream source string</param>
		/// <param name="path">Name of the stream (should include extension)</param>
		public StreamSource( string streamContents, string path )
		{
			m_Name = System.IO.Path.GetFileName( path );
			m_Path = path;
			m_Stream = new UndyingMemoryStream( Encoding.ASCII.GetBytes( streamContents ) );
		}

		/// <summary>
		/// Setup constructor. Creates stored stream from a byte array
		/// </summary>
		/// <param name="bytes">Stream source bytes</param>
		/// <param name="path">Name of the stream (should include extension)</param>
		public StreamSource( byte[] bytes, string path )
		{
			m_Name = System.IO.Path.GetFileName( path );
			m_Name = path;
			m_Stream = new UndyingMemoryStream( bytes );
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Gets the hash code of this source
		/// </summary>
		/// <returns>Returns the hash code of the source name</returns>
		public override int GetHashCode( )
		{
			return m_Name.GetHashCode( );
		}

		/// <summary>
		/// Returns the name of this source
		/// </summary>
		public override string ToString( )
		{
			return m_Name;
		}

		#endregion

		#region ISource Members

		/// <summary>
		/// Returns true if the source is valid
		/// </summary>
		public bool Exists
		{
			get { return true; }
		}

		/// <summary>
		/// Gets the name of the source
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Gets the path of the source
		/// </summary>
		public string Path
		{
			get { return m_Path; }
		}

		/// <summary>
		/// Gets the source extension
		/// </summary>
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
			throw new InvalidOperationException( string.Format( "Can't call GetSource(\"{0}\") on StreamSource", path ) );
		}

		/// <summary>
		/// Opens a stream
		/// </summary>
		/// <returns>Opened stream</returns>
		public Stream Open( )
		{
			return m_Stream;
		}

		#endregion

		#region Private members

		private readonly string m_Name;
		private readonly string m_Path;
		private readonly Stream m_Stream;

		private class UndyingMemoryStream : MemoryStream
		{
			public UndyingMemoryStream( Stream src ) :
				base( ReadStreamBytes( src ) )
			{
				
			}

			public UndyingMemoryStream( byte[] bytes ) :
				base( bytes )
			{
			}

			public override void Close( )
			{
				Seek( 0, SeekOrigin.Begin );
			}

			private static byte[] ReadStreamBytes( Stream src )
			{
				src.Position = 0;
				byte[] bytes = new byte[ src.Length ];
				src.Read( bytes, 0, bytes.Length ); // TODO: AP: should read long count of bytes
				return bytes;
			}
		}

		#endregion
	}
}
