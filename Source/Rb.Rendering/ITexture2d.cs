using System.Drawing;

namespace Rb.Rendering
{
	/// <summary>
	/// Interface for 2D textures
	/// </summary>
	/// <remarks>
	/// Use <see cref="TextureUtils"/> for further load/save functionality
	/// </remarks>
	public interface ITexture2d
	{
		/// <summary>
		/// Gets the width of the texture
		/// </summary>
		int Width
		{
			get;
		}

		/// <summary>
		/// Gets the height of the texture
		/// </summary>
		int Height
		{
			get;
		}

		/// <summary>
		/// Gets the format of the texture
		/// </summary>
		TextureFormat Format
		{
			get;
		}

		/// <summary>
		/// Creates an empty texture
		/// </summary>
		/// <param name="width">Width of the texture in pixels</param>
		/// <param name="height">Height of the texture in pixels</param>
		/// <param name="format">Format of the texture</param>
		void Create( int width, int height, TextureFormat format );
		
		/// <summary>
		/// Loads the texture from bitmap data
		/// </summary>
		void Load( Bitmap bmp, bool generateMipMaps );

		/// <summary>
		/// Converts this texture to a bitmap
		/// </summary>
		/// <returns>Bitmap</returns>
		Bitmap ToBitmap( );

		/// <summary>
		/// Binds this texture
		/// </summary>
		/// <param name="unit">Texture unit to bind this texture to</param>
		void Bind( int unit );

		/// <summary>
		/// Unbinds this texture
		/// </summary>
		/// <param name="unit">Texture unit that this texture is bound to</param>
		void Unbind( int unit );
	}
}
