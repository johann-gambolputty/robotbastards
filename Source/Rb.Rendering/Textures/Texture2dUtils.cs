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
			TextureFormat format;
			switch ( bmp.PixelFormat )
			{
				case PixelFormat.Format24bppRgb :
					format = TextureFormat.R8G8B8;
					break;
				case PixelFormat.Format32bppArgb :
					format = TextureFormat.R8G8B8A8;
					break;
				default :
					throw new ArgumentException( string.Format( "Bitmap is not in a supported format ({0})", bmp.PixelFormat ), "bmp" );
			}
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
			fixed ( byte* dataBytes = data.Bytes )
			{
				PixelFormat format = TextureFormatInfo.ToPixelFormat( data.Format );
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
			Texture2dData texData = texture.ToTextureData( false )[ 0 ];
			return CreateBitmapFromTextureData( texData );

		}

		/// <summary>
		/// Creates a texture from a bitmap
		/// </summary>
		public static ITexture2d CreateTextureFromBitmap( Bitmap bmp, bool generateMipMaps )
		{
			ITexture2d texture = Graphics.Factory.CreateTexture2d( );
			texture.Create( CreateTextureDataFromBitmap( bmp ), generateMipMaps );

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
		public static ITexture2d LoadTextureFromImageFile( Stream stream, bool generateMipMaps )
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
			Texture2dData texData = CreateTextureDataFromBitmap( bmp );
			texture.Create( texData, generateMipMaps );
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
			Texture2dData texData = CreateTextureDataFromBitmap( bmp );
			texture.Create( texData, generateMipMaps );
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

