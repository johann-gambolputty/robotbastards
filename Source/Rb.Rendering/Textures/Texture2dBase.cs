using System.Drawing;
using System;
using System.Runtime.Serialization;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.Textures
{
	/// <summary>
	/// 2D texture data
	/// </summary>
	/// <remarks>
	/// To apply a texture, use Renderer.BindTexture(), or a TextureSampler2d object
	/// </remarks>
	[Serializable]
	public abstract class Texture2dBase : ISerializable, ITexture2d
	{
		#region	Construction and setup

		/// <summary>
		/// Default constructor
		/// </summary>
		public Texture2dBase( )
		{
		}

		/// <summary>
		/// Serialization constructor
		/// </summary>
		public Texture2dBase( SerializationInfo info, StreamingContext context )
		{
			bool mipMapped = ( bool )info.GetValue( "mm", typeof( bool ) );
			Load( ( Bitmap )info.GetValue( "img", typeof( Bitmap ) ), mipMapped );
		}

		/// <summary>
		/// Loads the texture from a bitmap file
		/// </summary>
		public Texture2dBase( string path, bool generateMipMaps )
		{
			TextureUtils.Load( this, path, generateMipMaps );
		}

		#endregion

		#region ITexture2d members

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
		/// Creates an empty texture
		/// </summary>
		/// <param name="width">Width of the texture in pixels</param>
		/// <param name="height">Height of the texture in pixels</param>
		/// <param name="format">Format of the texture</param>
		public abstract void Create( int width, int height, TextureFormat format );
		
		/// <summary>
		/// Loads the texture from bitmap data
		/// </summary>
		public abstract void Load( Bitmap bmp, bool generateMipMaps );

		/// <summary>
		/// Generates an image from the texture
		/// </summary>
		public abstract Bitmap ToBitmap( );

		/// <summary>
		/// Binds this texture
		/// </summary>
		/// <param name="unit">Texture unit to bind this texture to</param>
		public abstract void Bind( int unit );
		
		/// <summary>
		/// Unbinds this texture
		/// </summary>
		/// <param name="unit">Texture unit that this texture is bound to</param>
		public abstract void Unbind( int unit );

		#endregion

		#region	Protected stuff

		protected int			m_Width;
		protected int			m_Height;
		protected TextureFormat	m_Format;
		protected bool			m_MipMapped;

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Gets rid of this texture
		/// </summary>
		public virtual void Dispose()
		{
		}

		#endregion

		#region ISerializable Members

		/// <summary>
		/// Saves this texture
		/// </summary>
		public virtual void GetObjectData( SerializationInfo info, StreamingContext context )
		{
			info.AddValue( "img", ToBitmap( ) );
			info.AddValue( "mm", m_MipMapped );
		}

		#endregion
	}
}
