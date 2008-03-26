using System;
using System.IO;
using System.Text;
using Rb.Assets.Interfaces;

namespace Rb.Assets.Base
{
	/// <summary>
	/// Stream source
	/// </summary>
	[Serializable]
	public class StreamSource : IStreamSource
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

			if ( stream.CanSeek )
			{
				stream.Seek( 0, SeekOrigin.Begin );
			}
			m_Memory = new byte[ stream.Length ];
			stream.Read( m_Memory, 0, m_Memory.Length );
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
			m_Memory = Encoding.ASCII.GetBytes( streamContents );
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
			m_Memory = bytes;
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

		#region Private members

		private readonly string m_Name;
		private readonly string m_Path;
		private readonly byte[] m_Memory;


		#endregion

		#region ISource Members

		public event Action<ISource> SourceChanged
		{
			add { }
			remove { }
		}

		public string Path
		{
			get { return m_Path; }
		}

		public string Name
		{
			get { return m_Name; }
		}

		public string Extension
		{
			get { return System.IO.Path.GetExtension( m_Name ); }
		}

		public bool HasExtension( string extension )
		{
			return string.Compare( Extension, extension, StringComparison.InvariantCultureIgnoreCase ) == 0;
		}

		public ILocation Location
		{
			get { throw new InvalidOperationException( "Stream has no associated location" ); }
		}

		#endregion

		#region IStreamSource Members

		public Stream Open( )
		{
			return new MemoryStream( m_Memory );
		}

		#endregion
	}
}
