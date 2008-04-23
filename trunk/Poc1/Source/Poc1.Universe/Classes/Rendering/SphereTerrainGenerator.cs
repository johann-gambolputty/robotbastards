using System.Drawing;
using System.Drawing.Imaging;
using Poc1.Universe.Interfaces.Rendering;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Classes.Rendering
{
	//	http://www.gamedev.net/community/forums/topic.asp?topic_id=263350

	/// <summary>
	/// Base class for sphere planet terrain generators
	/// </summary>
	public abstract class SphereTerrainGenerator : ISphereTerrainGenerator
	{
		/// <summary>
		/// Converts the (u,v) coordinate on a cube map face to an x,y,z position
		/// </summary>
		public static void UvToXyz( float u, float v, CubeMapFace face, out float x, out float y, out float z )
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
		/// <summary>
		/// Gets the height of a point on the surface of the surface
		/// </summary>
		public abstract float GetHeight( float x, float y, float z );

		/// <summary>
		/// Displaces a set of terrain vertices
		/// </summary>
		/// <remarks>
		/// Vertices should already be initialised with their position on the sphere (position property), and
		/// their initial normal (sphere normal)
		/// </remarks>
		public unsafe void DisplaceTerrainVertices( int vertexCount, TerrainVertex* vertices, float radius, float scale )
		{
			TerrainVertex* curVertex = vertices;
			for ( int index = 0; index < vertexCount; ++index )
			{
				float height = radius + GetHeight( curVertex->X, curVertex->Y, curVertex->Z ) * scale;
				curVertex->X += curVertex->NormalX * height;
				curVertex->Y += curVertex->NormalY * height;
				curVertex->Z += curVertex->NormalZ * height;
				++curVertex;
			}
		}
		private readonly Fast.SphereTerrainGenerator m_Gen = new Fast.SphereTerrainGenerator( Fast.TerrainGeneratorType.Ridged, 0 );

		/// <summary>
		/// Generates the side of a cube map
		/// </summary>
		public unsafe void GenerateSide( ITerrainTypeManager terrainTypes, CubeMapFace face, byte* pixels, int width, int height, int stride )
		{
			//	ReSharper LIES about face being unassignable to CubeMapFace... ignore
			m_Gen.GenerateTexture( face, PixelFormat.Format32bppArgb, width, height, stride, pixels );
			/*
			float incU = 2.0f / ( width - 1 );
			float incV = 2.0f / ( height - 1 );
			float v = -1;

			float[] weights = new float[ terrainTypes.TerrainTypes.Length ];

			float res = 1.0f;
			for ( int row = 0; row < height; ++row, v += incV )
			{
				float u = -1;
				byte* curPixel = pixels + row * stride;
				for ( int col = 0; col < width; ++col, u += incU )
				{
					float x, y, z;
					UvToXyz( u, v, face, out x, out y, out z );

					float invLength = res / Functions.Sqrt( u * u + v * v + 1 );
					x *= invLength;
					y *= invLength;
					z *= invLength;

					float latitude = ( y + 1 ) / 2;
					float terrainHeight = GetHeight( x, y, z );
					float total = 0.0f;
					float offset = 0;
					for ( int i = 0; i < weights.Length; ++i )
					{
					//	weights[ i ] = Fractals.RidgedFractal( x + offset, y + offset, z + offset, 1.8f, 8, 0.9f, Fractals.Noise3dBasis );
						weights[ i ] = ( Fractals.Noise3dBasis( x * 3.0f + offset, y * 3.0f + offset, z * 3.0f + offset ) + 1 ) / 2;
						weights[ i ] *= weights[ i ] * weights[ i ];
						weights[ i ] *= terrainTypes.TerrainTypes[ i ].Distribution.GetSample( terrainHeight, latitude, 0 );

						offset += 2.3f;
						total += weights[ i ];
					}
					float r = 0;
					float g = 0;
					float b = 0;
					for ( int i = 0; i < weights.Length; ++i )
					{
						Color c = terrainTypes.TerrainTypes[ i ].Colour;
						float f = weights[ i ] / total;
						r += c.R * f;
						g += c.G * f;
						b += c.B * f;
					}

					byte bTerrainHeight = ( byte )( terrainHeight * 255.0f );
					curPixel[ 0 ] = ( byte )r;
					curPixel[ 1 ] = ( byte )g;
					curPixel[ 2 ] = ( byte )b;
					curPixel[ 3 ] = bTerrainHeight;
					curPixel += 4;
				}
			}
			 */
		}
	}
}
