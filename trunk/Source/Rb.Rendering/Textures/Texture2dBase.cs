using System;
using System.Drawing;
using System.Runtime.Serialization;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.Textures
{
	/// <summary>
	/// 2D texture data
	/// </summary>
	/// <remarks>
	/// To apply a texture, use <see cref="Rb.Rendering.Interfaces.IRenderer.BindTexture"/> or a
	/// <see cref="ITexture2dSampler"/>.
	/// To create textures from bitmaps, use functions from the <see cref="Texture2dUtils"/> class.
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
			Texture2dData[] data = ( Texture2dData[] )info.GetValue( "img", typeof( Texture2dData[] ) );
			Create( data );
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
		/// Creates the texture from a single texture data object
		/// </summary>
		/// <param name="data">Texture data used to create the texture</param>
		/// <param name="generateMipMaps">Mipmap generation flag</param>
		public abstract void Create( Texture2dData data, bool generateMipMaps );

		/// <summary>
		/// Creates the texture from an array of texture data objects, that specify decreasing mipmap levels
		/// </summary>
		/// <param name="data">Texture data used to create the texture and its mipmaps</param>
		public abstract void Create( Texture2dData[] data );

		/// <summary>
		/// Gets texture data from this texture
		/// </summary>
		/// <param name="getMipMaps">If true, texture data for all mipmap levels are retrieved</param>
		/// <returns>
		/// Returns texture data extracted from this texture. If getMipMaps is false, only one <see cref="Texture2dData"/>
		/// object is returned. Otherwise, the array contains a <see cref="Texture2dData"/> object for each mipmap
		/// level.</returns>
		public abstract Texture2dData[] ToTextureData( bool getMipMaps );

		/// <summary>
		/// Creates the texture from a single bitmap
		/// </summary>
		/// <param name="bmp">Source bitmap</param>
		/// <param name="generateMipMaps">Mipmap generation flag</param>
		public abstract void Create( Bitmap bmp, bool generateMipMaps );

		/// <summary>
		/// Creates the texture from an array of bitmaps
		/// </summary>
		/// <param name="bitmaps">Source bitmap data</param>
		public abstract void Create( Bitmap[] bitmaps );

		/// <summary>
		/// Converts this texture to a bitmap
		/// </summary>
		/// <param name="getMipMaps">If true, an array of bitmaps are returned, one for each mipmap level</param>
		/// <returns>
		/// Returns an array of bitmaps. If getMipMaps is false, only one bitmap is returned. If
		/// getMipMaps is true, one bitmap is returned for each mipmap level.
		/// </returns>
		public abstract Bitmap[] ToBitmap( bool getMipMaps );

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
			info.AddValue( "img", ToTextureData( true ) );
		}

		#endregion

	}
}
