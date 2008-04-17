using System.Drawing;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// Cube map face enumeration
	/// </summary>
	public enum CubeMapFace
	{
		NegativeX,
		PositiveX,
		NegativeY,
		PositiveY,
		NegativeZ,
		PositiveZ
	}

	/// <summary>
	/// Interface for cube mapped textures
	/// </summary>
	public unsafe interface ICubeMapTexture : ITexture
	{
		/// <summary>
		/// Builds this cube map from 6 bitmaps
		/// </summary>
		/// <param name="posX">Positive X axis bitmap</param>
		/// <param name="negX">Negative X axis bitmap</param>
		/// <param name="posY">Positive Y axis bitmap</param>
		/// <param name="negY">Negative Y axis bitmap</param>
		/// <param name="posZ">Positive Z axis bitmap</param>
		/// <param name="negZ">Negative Z axis bitmap</param>
		/// <param name="generateMipMaps">If true, mipmaps are generated for the cube map faces</param>
		void Build( Bitmap posX, Bitmap negX, Bitmap posY, Bitmap negY, Bitmap posZ, Bitmap negZ, bool generateMipMaps );

		/// <summary>
		/// Converts a given cube map face to a bitmap
		/// </summary>
		Bitmap ToBitmap( CubeMapFace face );

		/// <summary>
		/// Renders this cubemap texture to a series of bitmaps
		/// </summary>
		Bitmap[] ToBitmaps( );
	}
}
