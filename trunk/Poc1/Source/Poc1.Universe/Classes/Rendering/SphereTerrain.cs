using System.Drawing;
using System.Drawing.Imaging;
using Poc1.Universe.Interfaces.Rendering;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;
using RbGraphics = Rb.Rendering.Graphics;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Planetary terrain for spherical planets
	/// </summary>
	public class SphereTerrain : IPlanetTerrain
	{
		/// <summary>
		/// Radius of the planet - used when placing patch vertices on the planetary sphere
		/// </summary>
		public const float PlanetStandardRadius = 32;

		/// <summary>
		/// Default constructor
		/// </summary>
		public SphereTerrain( )
		{
			m_Generator = new RidgedFractalSphereTerrainGenerator( );
		}

		/// <summary>
		/// Generates the terrain cube map texture for this planet
		/// </summary>
		/// <param name="res">Cube map face resolution</param>
		/// <returns>Returns the texture</returns>
		public ICubeMapTexture CreatePlanetTexture( int res )
		{
			ICubeMapTexture texture = RbGraphics.Factory.CreateCubeMapTexture( );

			texture.Build
				(
					GenerateCubeMapFace( CubeMapFace.PositiveX, res, PixelFormat.Format24bppRgb ),
					GenerateCubeMapFace( CubeMapFace.NegativeX, res, PixelFormat.Format24bppRgb ),
					GenerateCubeMapFace( CubeMapFace.PositiveY, res, PixelFormat.Format24bppRgb ),
					GenerateCubeMapFace( CubeMapFace.NegativeY, res, PixelFormat.Format24bppRgb ),
					GenerateCubeMapFace( CubeMapFace.PositiveZ, res, PixelFormat.Format24bppRgb ),
					GenerateCubeMapFace( CubeMapFace.NegativeZ, res, PixelFormat.Format24bppRgb ),
					true
				);
			return texture;
		}

		#region IPlanetTerrain Members

		/// <summary>
		/// Generates vertices for a patch
		/// </summary>
		/// <param name="origin">Patch origin</param>
		/// <param name="uStep">Offset between row vertices</param>
		/// <param name="vStep">Offset between column vertices</param>
		/// <param name="res">Patch resolution</param>
		/// <param name="firstVertex">Patch vertices</param>
		public unsafe void GenerateTerrainPatchVertices( Point3 origin, Vector3 uStep, Vector3 vStep, int res, TerrainVertex* firstVertex )
		{
			TerrainVertex* curVertex = firstVertex;

			Point3 rowStart = origin;
			for ( int row = 0; row < res; ++row )
			{
				Point3 curPt = rowStart;
				TerrainVertex* rowVertices = curVertex;
				for ( int col = 0; col < res; ++col )
				{
					float invLength = 1.0f / Functions.Sqrt( curPt.X * curPt.X + curPt.Y * curPt.Y + curPt.Z * curPt.Z );
					float toSurface = invLength * PlanetStandardRadius;
					curVertex->SetPosition( curPt.X * toSurface, curPt.Y * toSurface, curPt.Z * toSurface );
					curVertex->SetNormal( curPt.X * invLength, curPt.Y * invLength, curPt.Z * invLength );
					++curVertex;
					curPt += uStep;
				}
				m_Generator.DisplaceTerrainVertices( res, rowVertices );

				rowStart += vStep;
			}

			int right = res - 1;
			int bottom = res - 1;

			for ( int row = 1; row < bottom; ++row )
			{
				curVertex = firstVertex + ( row * res ) + 1;
				for ( int col = 1; col < right; ++col, ++curVertex )
				{
					float curHeight		= curVertex->Y;
					float leftHeight	= ( curVertex - 1 )->Y;
					float rightHeight	= ( curVertex + 1 )->Y;
					float upHeight		= ( curVertex - res )->Y;
					float downHeight	= ( curVertex + res )->Y;

					//	Calculate normal from ((right - cur)x(down - cur))
					float n0x = curHeight - rightHeight;
					float n0y = 1.0f;
					float n0z = curHeight - downHeight;

					//	Calculate normal from ((up - cur)x(right - cur))
					float n1x = curHeight - upHeight;
					float n1y = 1.0f;
					float n1z = curHeight - rightHeight;
					
					//	Calculate normal from ((left - cur)x(up - cur))
					float n2x = curHeight - leftHeight;
					float n2y = 1.0f;
					float n2z = curHeight - upHeight;
					
					//	Calculate normal from ((down - cur)x(left - cur))
					float n3x = curHeight - downHeight;
					float n3y = 1.0f;
					float n3z = curHeight - leftHeight;

					float nX = ( n0x + n1x + n2x + n3x ) / 4;
					float nY = ( n0y + n1y + n2y + n3y ) / 4;
					float nZ = ( n0z + n1z + n2z + n3z ) / 4;

					float invLength = 1.0f / Functions.Sqrt( nX * nX + nY * nY + nZ * nZ );

					curVertex->SetNormal( nX * invLength, nY * invLength, nZ * invLength );
					++curVertex;
				}
			}
		}

		#endregion

		#region Private Members

		private readonly ISpherePlanetTerrainGenerator m_Generator;

		/// <summary>
		/// Generates cube map face bitmaps
		/// </summary>
		private unsafe Bitmap GenerateCubeMapFace( CubeMapFace face, int res, PixelFormat format )
		{
			Bitmap bmp = new Bitmap( res, res, format );
			BitmapData bmpData = bmp.LockBits( new System.Drawing.Rectangle( 0, 0, res, res ), ImageLockMode.WriteOnly, format );
			m_Generator.GenerateSide( face, ( byte* )bmpData.Scan0, res, res, bmpData.Stride );
			bmp.UnlockBits( bmpData );

			return bmp;
		}

		#endregion
	}
}
