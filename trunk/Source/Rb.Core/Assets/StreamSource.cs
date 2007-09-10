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
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="stream">Stream source</param>
		/// <param name="name">Name of the stream (should include extension)</param>
		public StreamSource( Stream stream, string name )
		{
			m_Name = name;
			m_Stream = stream;
		}

		/// <summary>
		/// Setup constructor. Creates stored stream from a string
		/// </summary>
		/// <param name="streamContents">Stream source string</param>
		/// <param name="name">Name of the stream (should include extension)</param>
		public StreamSource( string streamContents, string name )
		{
			m_Name = name;
			m_Stream = new MemoryStream( Encoding.ASCII.GetBytes( streamContents ) );
		}

		/// <summary>
		/// Setup constructor. Creates stored stream from a byte array
		/// </summary>
		/// <param name="bytes">Stream source bytes</param>
		/// <param name="name">Name of the stream (should include extension)</param>
		public StreamSource( byte[] bytes, string name )
		{
			m_Name = name;
			m_Stream = new MemoryStream( bytes );
		}

		public override int GetHashCode( )
		{
			return m_Name.GetHashCode( );
		}

		public override string ToString( )
		{
			return m_Name;
		}

		#region ISource Members

		/// <summary>
		/// Returns true if the source is valid
		/// </summary>
		public bool IsValid
		{
			get { return true; }
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
		public ISource GetSource( string path )
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

		private readonly string m_Name;
		private readonly Stream m_Stream;
	}
}
