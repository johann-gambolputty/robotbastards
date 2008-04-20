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
	/// Cube map face UV mappings
	/// </summary>
	public static class CubeMapFaceUv
	{
		/// <summary>
		/// Converts a (u,v) point to an (x,y,z) coordinate, depending on the value of face
		/// </summary>
		public static void ToXyz( CubeMapFace face, float u, float v, out float x, out float y, out float z )
		{
			switch ( face )
			{
				default:
				case CubeMapFace.NegativeX:
					x = -1;
					y = v;
					z = u;
					break;
				case CubeMapFace.PositiveX:
					x = 1;
					y = v;
					z = -u;
					break;
				case CubeMapFace.NegativeY:
					x = -u;
					y = -1;
					z = -v;
					break;
				case CubeMapFace.PositiveY:
					x = -u;
					y = 1;
					z = v;
					break;
				case CubeMapFace.NegativeZ:
					x = -u;
					y = v;
					z = -1;
					break;
				case CubeMapFace.PositiveZ:
					x = u;
					y = v;
					z = 1;
					break;
			}
		}
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
