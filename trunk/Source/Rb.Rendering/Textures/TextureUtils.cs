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
	public static class TextureUtils
	{
		#region Existing 2d texture loads

		/// <summary>
		/// Loads a manifest resource into an existing texture
		/// </summary>
		/// <param name="texture">Texture to load into</param>
		/// <param name="name">Resource name</param>
		/// <param name="generateMipMaps">If true, then mipmaps are generated for the created texture</param>
		public static void LoadResource( ITexture2d texture, string name, bool generateMipMaps )
		{
			Stream stream = AppDomainUtils.FindManifestResource( name );
			Load( texture, stream, generateMipMaps );
		}
		
		/// <summary>
		/// Creates a texture from an image stream
		/// </summary>
		/// <param name="texture">Texture to load into</param>
		/// <param name="stream">Stream containing an image</param>
		/// <param name="generateMipMaps">If true, then mipmaps are generated for the created texture</param>
		public static void Load( ITexture2d texture, Stream stream, bool generateMipMaps )
		{
			Bitmap bmp = new Bitmap( stream );
			texture.Load( bmp, generateMipMaps );
		}

		/// <summary>
		/// Creates a texture from an image file
		/// </summary>
		/// <param name="texture">Texture to load into</param>
		/// <param name="path">Path to image file</param>
		/// <param name="generateMipMaps">If true, then mipmaps are generated for the created texture</param>
		public static void Load( ITexture2d texture, string path, bool generateMipMaps )
		{
			Bitmap bmp = new Bitmap( path );
			texture.Load( bmp, generateMipMaps );
		}

		#endregion

		#region 2d texture loads

		/// <summary>
		/// Creates a texture from a manifest resource
		/// </summary>
		/// <param name="name">Resource name</param>
		/// <param name="generateMipMaps">If true, then mipmaps are generated for the created texture</param>
		/// <returns>Returns the created texture</returns>
		public static ITexture2d LoadResource( string name, bool generateMipMaps )
		{
			Stream stream = AppDomainUtils.FindManifestResource( name );
			return Load( stream, generateMipMaps );
		}
		

		/// <summary>
		/// Creates a texture from an image stream
		/// </summary>
		/// <param name="stream">Stream containing an image</param>
		/// <param name="generateMipMaps">If true, then mipmaps are generated for the created texture</param>
		/// <returns>Returns the created texture</returns>
		public static ITexture2d Load( Stream stream, bool generateMipMaps )
		{
			Bitmap bmp = new Bitmap( stream );
			ITexture2d texture = Graphics.Factory.CreateTexture2d( );
			texture.Load( bmp, generateMipMaps );
			return texture;
		}

		/// <summary>
		/// Creates a texture from an image file
		/// </summary>
		/// <param name="path">Path to image file</param>
		/// <param name="generateMipMaps">If true, then mipmaps are generated for the created texture</param>
		/// <returns>Returns the created texture</returns>
		public static ITexture2d Load( string path, bool generateMipMaps )
		{
			Bitmap bmp = new Bitmap( path );
			ITexture2d texture = Graphics.Factory.CreateTexture2d( );
			texture.Load( bmp, generateMipMaps );
			return texture;
		}

		#endregion

		#region 2d texture saves

		/// <summary>
		/// Saves a texture to a file
		/// </summary>
		/// <param name="texture">Texture to save</param>
		/// <param name="path">Save file path</param>
		public static void Save( ITexture2d texture, string path )
		{
			Image img = texture.ToBitmap( );
			if ( img != null )
			{
				img.Save( path );
			}
		}

		/// <summary>
		/// Saves a texture to a file
		/// </summary>
		/// <param name="texture">Texture to save</param>
		/// <param name="path">Save file path</param>
		/// <param name="format">Image file format</param>
		public static void Save( ITexture2d texture, string path, ImageFormat format )
		{
			Image img = texture.ToBitmap( );
			if ( img != null )
			{
				img.Save( path, format );
			}
		}

		#endregion
	}
}

