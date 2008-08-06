using System.Drawing;

namespace Rb.Rendering.Interfaces.Objects
{
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
		/// Creates an empty texture
		/// </summary>
		/// <param name="width">Width of the texture in pixels</param>
		/// <param name="height">Height of the texture in pixels</param>
		/// <param name="format">Format of the texture</param>
		void Create( int width, int height, TextureFormat format );

		/// <summary>
		/// Creates from a <see cref="Texture2dData"/> object
		/// </summary>
		/// <param name="data">Texture data</param>
		void Create( Texture2dData data );
		
		/// <summary>
		/// Loads the texture from bitmap data
		/// </summary>
		void Load( Bitmap bmp, bool generateMipMaps );

		/// <summary>
		/// Converts this texture to a bitmap
		/// </summary>
		Bitmap ToBitmap( );
	}
}
