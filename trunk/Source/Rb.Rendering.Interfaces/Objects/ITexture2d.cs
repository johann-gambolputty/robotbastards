using System.Drawing;

namespace Rb.Rendering.Interfaces.Objects
{
	public enum TextureUsage
	{
		Normal,
		CubeMapNegativeX,
		CubeMapPositiveX,
		CubeMapNegativeY,
		CubeMapPositiveY,
		CubeMapNegativeZ,
		CubeMapPositiveZ
	}

	/// <summary>
	/// 2 dimensional texture interface
	/// </summary>
	public interface ITexture2d : ITexture
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
		/// Gets the texture usage type
		/// </summary>
		TextureUsage Usage
		{
			get;
		}

		/// <summary>
		/// Creates an empty texture
		/// </summary>
		/// <param name="width">Width of the texture in pixels</param>
		/// <param name="height">Height of the texture in pixels</param>
		/// <param name="format">Format of the texture</param>
		/// <param name="usage">How the texture will be used</param>
		void Create( int width, int height, TextureFormat format, TextureUsage usage );
		
		/// <summary>
		/// Loads the texture from bitmap data
		/// </summary>
		void Load( Bitmap bmp, bool generateMipMaps, TextureUsage usage );

		/// <summary>
		/// Converts this texture to a bitmap
		/// </summary>
		Bitmap ToBitmap( );
	}
}
