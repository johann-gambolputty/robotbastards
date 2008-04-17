using Poc1.Universe.Interfaces.Rendering;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Base class for sphere planet terrain generators
	/// </summary>
	public abstract class SphereTerrainGenerator : ISpherePlanetTerrainGenerator
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
		public unsafe void DisplaceTerrainVertices( int vertexCount, TerrainVertex* vertices )
		{
			TerrainVertex* curVertex = vertices;
			for ( int index = 0; index < vertexCount; ++index )
			{
				float height = GetHeight( curVertex->X, curVertex->Y, curVertex->Z );
				curVertex->X = curVertex->NormalX * height;
				curVertex->Y = curVertex->NormalY * height;
				curVertex->Z = curVertex->NormalZ * height;
			}
		}

		/// <summary>
		/// Generates the side of a cube map
		/// </summary>
		public unsafe void GenerateSide( CubeMapFace face, byte* pixels, int width, int height, int stride )
		{
			float incU = 2.0f / ( width - 1 );
			float incV = 2.0f / ( height - 1 );
			float v = -1;

			float res = 2.0f;
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

					float fVal = GetHeight( x, y, z );
					int val = ( int )( fVal * 255.0f );
				//	Color colour = TerrainColourMap[ latitude, val ];
				//	curPixel[ 0 ] = colour.R;
				//	curPixel[ 1 ] = colour.G;
				//	curPixel[ 2 ] = colour.B;
					curPixel[ 0 ] = ( byte )val;
					curPixel[ 1 ] = ( byte )val;
					curPixel[ 2 ] = ( byte )val;

					curPixel += 3;
				}
			}
		}
	}
}
