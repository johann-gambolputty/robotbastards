
using System;

namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// Managed data for a 2d texture
	/// </summary>
	public class Texture2dData
	{
		/// <summary>
		/// Gets the width of the texture
		/// </summary>
		public int Width
		{
			get { return m_Width; }
		}

		/// <summary>
		/// Gets the height of the texture
		/// </summary>
		public int Height
		{
			get { return m_Height; }
		}

		/// <summary>
		/// Gets the format of the texture
		/// </summary>
		public TextureFormat Format
		{
			get { return m_Format; }
		}

		/// <summary>
		/// Gets the texture data
		/// </summary>
		public byte[] Bytes
		{
			get { return m_Data; }
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public Texture2dData( )
		{	
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public Texture2dData( int width, int height, TextureFormat format )
		{
			Create( width, height, format );
		}

		/// <summary>
		/// Creates the texture data
		/// </summary>
		public void Create( int width, int height, TextureFormat format )
		{
			m_Width = width;
			m_Height = height;
			m_Format = format;
			m_Data = new byte[ width * height * TextureFormatInfo.GetSizeInBytes( format ) ];
		}

		/// <summary>
		/// Creates the texture data
		/// </summary>
		public void Create( int width, int height, TextureFormat format, byte[] data )
		{
			if ( data != null )
			{
				int expectedDataSize = width * height * TextureFormatInfo.GetSizeInBytes( format );
				if ( data.Length < expectedDataSize )
				{
					throw new ArgumentException( string.Format( "Expected texture data array to contain at least {0} bytes of data - only contained {1}", expectedDataSize, data.Length ) );
				}
			}

			m_Width = width;
			m_Height = height;
			m_Format = format;
			m_Data = data;
		}

		#region Private Members

		private TextureFormat m_Format;
		private int m_Width;
		private int m_Height;
		private byte[] m_Data;

		#endregion
	}
}
