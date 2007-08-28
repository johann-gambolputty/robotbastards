using System.Drawing;
using Rb.Core.Utils;
using System;
using System.Runtime.Serialization;

namespace Rb.Rendering
{
	/// <summary>
	/// Texture formats
	/// </summary>
	public enum TextureFormat
	{
		Undefined,

		Depth16,
		Depth24,
		Depth32,

		R8G8B8,
		B8G8R8,

		R8G8B8X8,
		B8G8R8X8,

		R8G8B8A8,
		B8G8R8A8,

		A8R8G8B8,
		A8B8G8R8,
	}

	/// <summary>
	/// 2D texture data
	/// </summary>
	/// <remarks>
	/// To apply a texture, use Renderer.ApplyTexture(), or a TextureSampler2d object
	/// </remarks>
	/// <seealso>ApplyTexture2d</seealso>
	[Serializable]
	public abstract class Texture2d : IDisposable, ISerializable
	{
		#region	Texture Format

		/// <summary>
		/// Returns the size in bits of a given texture format
		/// </summary>
		public static int GetTextureFormatSize( TextureFormat format )
		{
			switch ( format )
			{
				case TextureFormat.Depth16	:	return 16;
				case TextureFormat.Depth24	:	return 24;
				case TextureFormat.Depth32	:	return 32;

				case TextureFormat.R8G8B8	:	return 24;
				case TextureFormat.B8G8R8	:	return 24;

				case TextureFormat.R8G8B8X8	:	return 32;
				case TextureFormat.B8G8R8X8	:	return 32;

				case TextureFormat.R8G8B8A8	:	return 32;
				case TextureFormat.B8G8R8A8	:	return 32;

				case TextureFormat.A8R8G8B8	:	return 32;
				case TextureFormat.A8B8G8R8	:	return 32;
			}
			return 0;
		}

		#endregion

		#region	Construction and setup

		/// <summary>
		/// Default constructor
		/// </summary>
		public Texture2d( )
		{
		}

		/// <summary>
		/// Serialization constructor
		/// </summary>
		public Texture2d( SerializationInfo info, StreamingContext context )
		{
			Load( ( Bitmap )info.GetValue( "img", typeof( Bitmap ) ) );
		}

		/// <summary>
		/// Loads the texture from a bitmap file
		/// </summary>
		public Texture2d( string path )
		{
			Load( path );
		}

		/// <summary>
		/// Creates an empty texture
		/// </summary>
		/// <param name="width">Width of the texture in pixels</param>
		/// <param name="height">Height of the texture in pixels</param>
		/// <param name="format">Format of the texture</param>
		public abstract void Create( int width, int height, TextureFormat format );

		#endregion

		#region	Public properties

		/// <summary>
		/// Gets the width of the texture
		/// </summary>
		public int	Width
		{
			get
			{
				return m_Width;
			}
		}

		/// <summary>
		/// Gets the height of the texture
		/// </summary>
		public int Height
		{
			get
			{
				return m_Height;
			}
		}

		/// <summary>
		/// Gets the format of the texture
		/// </summary>
		public TextureFormat	Format
		{
			get
			{
				return m_Format;
			}
		}

		#endregion

		#region	Loading

		/// <summary>
		/// Creates a texture from a resource, using the manifest resource stream
		/// </summary>
		public static Texture2d FromManifestResource( string name )
		{
			Texture2d texture = RenderFactory.Instance.NewTexture2d( );
			texture.LoadManifestResource( name );
			return texture;
		}

		/// <summary>
		/// Loads the texture from a bitmap file
		/// </summary>
		public void Load( string path )
		{
			Image img = Image.FromFile( path, true );
			Bitmap bmp = new Bitmap( img );

			//	Dispose() img immediately - while it remains active, Image objects lock their source files
			img.Dispose( );

			//	Load the bitmap
			Load( bmp );
		}

		/// <summary>
		/// Loads the texture from a resource in this assembly's manifest resources
		/// </summary>
		public void LoadManifestResource( string name )
		{
			System.IO.Stream stream = AppDomainUtils.FindManifestResource( name );
			Load( new Bitmap( Image.FromStream( stream ) ) );
		}

		/// <summary>
		/// Loads the texture from bitmap data
		/// </summary>
		public abstract void Load( Bitmap bmp );

		#endregion

		#region	Saving and conversion

		/// <summary>
		/// Generates an image from the texture
		/// </summary>
		public abstract Bitmap ToBitmap( );

		/// <summary>
		/// Saves this texture to a file
		/// </summary>
		public void Save( string path )
		{
			Image img = ToBitmap( );
			if ( img != null )
			{
				img.Save( path );
			}
		}

		#endregion

		#region	Protected stuff

		protected int			m_Width;
		protected int			m_Height;
		protected TextureFormat	m_Format;

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
		}

		#endregion
	}
}
