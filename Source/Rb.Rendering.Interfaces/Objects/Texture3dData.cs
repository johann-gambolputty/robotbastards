
namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// Managed data for a 3d texture
	/// </summary>
	/// <remarks>
	/// Example:
	/// <code>
	/// Texture3dData data = new Texture3dData( );
	/// data.Create( 64, 64, 64, TextureFormat.R8G8B8 );
	/// </code>
	/// </remarks>
	public class Texture3dData
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public Texture3dData( )
		{	
		}

		/// <summary>
		/// Creates this texture data
		/// </summary>
		public Texture3dData( int width, int height, int depth, TextureFormat format )
		{
			Create( width, height, depth, format );
		}

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
		/// Gets the depth of the texture
		/// </summary>
		public int Depth
		{
			get { return m_Depth; }
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
		/// Creates the texture data
		/// </summary>
		public void Create( int width, int height, int depth, TextureFormat format )
		{
			m_Width = width;
			m_Height = height;
			m_Depth = depth;
			m_Format = format;
			m_Data = new byte[ width * height * depth * TextureFormatInfo.GetSizeInBytes( format ) ];
		}

		#region Private Members

		private TextureFormat m_Format;
		private int m_Width;
		private int m_Height;
		private int m_Depth;
		private byte[] m_Data;
		
		#endregion
	}

}
