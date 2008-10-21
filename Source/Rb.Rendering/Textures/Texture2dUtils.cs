using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Rb.Core.Utils;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.Textures
{
	/// <summary>
	/// Utilities for loading, saving and manipulating textures
	/// </summary>
	public static class Texture2dUtils
	{
		#region Converting bitmaps to and from texture data

		/// <summary>
		/// Creates texture data from a bitmap
		/// </summary>
		public unsafe static Texture2dData CreateTextureDataFromBitmap( Bitmap bmp )
		{
			Texture2dData data = new Texture2dData( );
			TextureFormat format = GetCompatibleTextureFormatFromBitmap( ref bmp );
			BitmapData bmpData = LockEntireBitmap( bmp );
			data.Create( bmp.Width, bmp.Height, format );
			fixed ( byte* dstBytes = data.Bytes )
			{
				MsvCrt.memcpy( dstBytes, ( void* )bmpData.Scan0, data.Bytes.Length );
			}

			return data;
		}

		/// <summary>
		/// Creates a bitmap from texture data
		/// </summary>
		public unsafe static Bitmap CreateBitmapFromTextureData( Texture2dData data )
		{
			PixelFormat format;
			switch ( data.Format )
			{
				case TextureFormat.Depth16			:	format = PixelFormat.Format16bppGrayScale;	break;
			//	case TextureFormat.Depth24			:	break;	//	No mapping
			//	case TextureFormat.Depth32			:	break;	//	No mapping

				case TextureFormat.R8G8B8			:	format = PixelFormat.Format24bppRgb; break;
			//	case TextureFormat.B8G8R8			:	break;	//	No mapping

				case TextureFormat.R8G8B8A8			:	format = PixelFormat.Format32bppRgb; break;
			//	case TextureFormat.B8G8R8A8			:	break;	//	No mapping

				case TextureFormat.A8R8G8B8			:	format = PixelFormat.Format32bppArgb; break;
			//	case TextureFormat.A8B8G8R8			:	break;	//	No mapping

				default :
					throw new NotSupportedException( "Unsupported texture format: " + data.Format );
			}
			fixed ( byte* dataBytes = data.Bytes )
			{
				int stride = data.Width * TextureFormatInfo.GetSizeInBytes( data.Format );
				Bitmap bmp = new Bitmap( data.Width, data.Height, stride, format, new IntPtr( dataBytes ) );
				return bmp;
			}
		}

		#endregion


		#region Converting bitmaps to and from textures

		/// <summary>
		/// Creates a bitmap from a texture
		/// </summary>
		public static Bitmap CreateBitmapFromTexture( ITexture2d texture )
		{
			Bitmap bmp = texture.ToBitmap( false )[ 0 ];
			return bmp;
		}

		/// <summary>
		/// Creates a texture from a bitmap
		/// </summary>
		public static ITexture2d CreateTextureFromBitmap( Bitmap bmp, bool generateMipMaps )
		{
			ITexture2d texture = Graphics.Factory.CreateTexture2d( );
			texture.Create( bmp, generateMipMaps );

			return texture;
		}

		#endregion

		#region Existing 2d texture loads

		/// <summary>
		/// Loads a manifest resource into an existing texture
		/// </summary>
		/// <param name="name">Resource name</param>
		/// <param name="generateMipMaps">If true, then mipmaps are generated for the created texture</param>
		public static ITexture2d CreateTextureFromImageResource( string name, bool generateMipMaps )
		{
			ITexture2d texture = Graphics.Factory.CreateTexture2d( );
			Stream stream = AppDomainUtils.FindManifestResource( name );
			LoadTextureFromImageStream( texture, stream, generateMipMaps );
			return texture;
		}

		/// <summary>
		/// Loads a manifest resource into an existing texture
		/// </summary>
		/// <param name="texture">Texture to load into</param>
		/// <param name="name">Resource name</param>
		/// <param name="generateMipMaps">If true, then mipmaps are generated for the created texture</param>
		public static void CreateTextureFromImageResource( ITexture2d texture, string name, bool generateMipMaps )
		{
			Stream stream = AppDomainUtils.FindManifestResource( name );
			LoadTextureFromImageStream( texture, stream, generateMipMaps );
		}

		/// <summary>
		/// Creates a texture from an image stream
		/// </summary>
		/// <param name="stream">Stream containing image data</param>
		/// <param name="generateMipMaps">If true, then mipmaps are generated for the created texture</param>
		public static ITexture2d LoadTextureFromImageStream( Stream stream, bool generateMipMaps )
		{
			ITexture2d texture = Graphics.Factory.CreateTexture2d( );
			LoadTextureFromImageStream( texture, stream, generateMipMaps );
			return texture;
		}

		/// <summary>
		/// Creates a texture from an image stream
		/// </summary>
		/// <param name="texture">Texture to load into</param>
		/// <param name="stream">Stream containing image data</param>
		/// <param name="generateMipMaps">If true, then mipmaps are generated for the created texture</param>
		public static void LoadTextureFromImageStream( ITexture2d texture, Stream stream, bool generateMipMaps )
		{
			Bitmap bmp = new Bitmap( stream );
			texture.Create( bmp, generateMipMaps );
		}

		/// <summary>
		/// Creates a texture from an image file
		/// </summary>
		/// <param name="path">Path to image file</param>
		/// <param name="generateMipMaps">If true, then mipmaps are generated for the created texture</param>
		public static ITexture2d LoadTextureFromImageFile( string path, bool generateMipMaps )
		{
			ITexture2d texture = Graphics.Factory.CreateTexture2d( );
			LoadTextureFromImageFile( texture, path, generateMipMaps );
			return texture;
		}

		/// <summary>
		/// Creates a texture from an image file
		/// </summary>
		/// <param name="texture">Texture to load into</param>
		/// <param name="path">Path to image file</param>
		/// <param name="generateMipMaps">If true, then mipmaps are generated for the created texture</param>
		public static void LoadTextureFromImageFile( ITexture2d texture, string path, bool generateMipMaps )
		{
			Bitmap bmp = new Bitmap( path );
		//	Texture2dData texData = CreateTextureDataFromBitmap( bmp );
		//	texture.Create( texData, generateMipMaps );
			texture.Create( bmp, generateMipMaps );
		}

		#endregion

		#region 2d texture saves

		/// <summary>
		/// Saves a texture to a file
		/// </summary>
		/// <param name="texture">Texture to save</param>
		/// <param name="path">Save file path</param>
		public static void SaveTextureToImageFile( ITexture2d texture, string path )
		{
			Image img = CreateBitmapFromTexture( texture );
			img.Save( path );
		}

		/// <summary>
		/// Saves a texture to a file
		/// </summary>
		/// <param name="texture">Texture to save</param>
		/// <param name="path">Save file path</param>
		/// <param name="format">Image file format</param>
		public static void SaveTextureToImageFile( ITexture2d texture, string path, ImageFormat format )
		{
			Image img = CreateBitmapFromTexture( texture );
			img.Save( path, format );
		}

		#endregion

		#region Private Members

		/// <summary>
		/// Gets the <see cref="TextureFormat"/> from a bitmap. If the bitmap has no compatible texture format,
		/// its converted to the closest match.
		/// </summary>
		private static TextureFormat GetCompatibleTextureFormatFromBitmap( ref Bitmap bmp )
		{
			//	Handle direct mappings to GL texture image formats
			TextureFormat result = TextureFormat.R8G8B8;
			PixelFormat targetFormat = PixelFormat.Format24bppRgb;
			switch ( bmp.PixelFormat )
			{
				case PixelFormat.Format32bppArgb		:
				{
					targetFormat = PixelFormat.Format32bppArgb;
					result = TextureFormat.A8R8G8B8;
					break;
				}
				case PixelFormat.Format32bppPArgb		:
				{
					targetFormat = PixelFormat.Format32bppArgb;
					result = TextureFormat.A8R8G8B8;
					break;
				}
				case PixelFormat.Format32bppRgb			:
				{
					targetFormat = PixelFormat.Format32bppArgb;
					result = TextureFormat.A8R8G8B8;
					break;
				}
			}

			if ( targetFormat != bmp.PixelFormat )
			{
				bmp = bmp.Clone( new Rectangle( 0, 0, bmp.Width, bmp.Height ), targetFormat );
			}

			return result;
		}

		/// <summary>
		/// Locks an entire bitmap for texture creation
		/// </summary>
		private static BitmapData LockEntireBitmap( Bitmap bmp )
		{
			BitmapData bmpData = bmp.LockBits( new Rectangle( 0, 0, bmp.Width, bmp.Height ), ImageLockMode.ReadOnly, bmp.PixelFormat );

			int expectedStride = bmp.Width * ( Image.GetPixelFormatSize( bmp.PixelFormat ) / 8 );
			if ( bmpData.Stride != expectedStride )
			{
				//	Argh... we don't quite handle bitmaps with loony strides
				throw new ArgumentException( string.Format( "Unexpected stride in bitmap (was {0}, expected {1})", bmpData.Stride, expectedStride ) );
			}

			return bmpData;
		}


		#endregion
	}
}

