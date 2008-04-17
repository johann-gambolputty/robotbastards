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
					GenerateCubeMapFace( CubeMapFace.PositiveX, res, PixelFormat.Format32bppArgb ),
					GenerateCubeMapFace( CubeMapFace.NegativeX, res, PixelFormat.Format32bppArgb ),
					GenerateCubeMapFace( CubeMapFace.PositiveY, res, PixelFormat.Format32bppArgb ),
					GenerateCubeMapFace( CubeMapFace.NegativeY, res, PixelFormat.Format32bppArgb ),
					GenerateCubeMapFace( CubeMapFace.PositiveZ, res, PixelFormat.Format32bppArgb ),
					GenerateCubeMapFace( CubeMapFace.NegativeZ, res, PixelFormat.Format32bppArgb ),
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

			float heightScale = 8.0f;

			Point3 rowStart = origin;
			for ( int row = 0; row < res; ++row )
			{
				Point3 curPt = rowStart;
				TerrainVertex* rowVertices = curVertex;
				for ( int col = 0; col < res; ++col )
				{
					float invLength = 1.0f / Functions.Sqrt( curPt.X * curPt.X + curPt.Y * curPt.Y + curPt.Z * curPt.Z );
					float x = curPt.X * invLength;
					float y = curPt.Y * invLength;
					float z = curPt.Z * invLength;
					curVertex->SetPosition( x, y, z );
					curVertex->SetNormal( x, y, z );
					++curVertex;
					curPt += uStep;
				}
				m_Generator.DisplaceTerrainVertices( res, rowVertices, PlanetStandardRadius, heightScale );

				rowStart += vStep;
			}

			//	Build border regions
			//	TODO: AP: Can be optimised - remove arrays, don't calculate border vertices in main loop, make separate border loop
			Point3[] upPoints = new Point3[ res ];
			Point3[] leftPoints = new Point3[ res ];
			Point3[] rightPoints = new Point3[ res ];
			Point3[] downPoints = new Point3[ res ];

			Point3 upPos = origin - vStep;
			Point3 leftPos = origin - uStep;
			Point3 downPos = origin + ( vStep * res );
			Point3 rightPos = origin + ( uStep * res );
			for ( int row = 0; row < res; ++row, upPos += uStep, downPos += uStep, leftPos += vStep, rightPos += vStep )
			{
				float invLength = 1.0f / Functions.Sqrt( upPos.X * upPos.X + upPos.Y * upPos.Y + upPos.Z * upPos.Z );
				upPoints[ row ] = new Point3( upPos.X * invLength, upPos.Y * invLength, upPos.Z * invLength );
				invLength *= PlanetStandardRadius + heightScale * m_Generator.GetHeight( upPoints[ row ].X, upPoints[ row ].Y, upPoints[ row ].Z );
				upPoints[ row ] += new Vector3( upPos.X * invLength, upPos.Y * invLength, upPos.Z * invLength );

				invLength = 1.0f / Functions.Sqrt( downPos.X * downPos.X + downPos.Y * downPos.Y + downPos.Z * downPos.Z );
				downPoints[ row ] = new Point3( downPos.X * invLength, downPos.Y * invLength, downPos.Z * invLength );
				invLength *= PlanetStandardRadius + heightScale * m_Generator.GetHeight( downPoints[ row ].X, downPoints[ row ].Y, downPoints[ row ].Z );
				downPoints[ row ] += new Vector3( downPos.X * invLength, downPos.Y * invLength, downPos.Z * invLength );

				invLength = 1.0f / Functions.Sqrt( leftPos.X * leftPos.X + leftPos.Y * leftPos.Y + leftPos.Z * leftPos.Z );
				leftPoints[ row ] = new Point3( leftPos.X * invLength, leftPos.Y * invLength, leftPos.Z * invLength );
				invLength *= PlanetStandardRadius + heightScale * m_Generator.GetHeight( leftPoints[ row ].X, leftPoints[ row ].Y, leftPoints[ row ].Z );
				leftPoints[ row ] += new Vector3( leftPos.X * invLength, leftPos.Y * invLength, leftPos.Z * invLength );

				invLength = 1.0f / Functions.Sqrt( rightPos.X * rightPos.X + rightPos.Y * rightPos.Y + rightPos.Z * rightPos.Z );
				rightPoints[ row ] = new Point3( rightPos.X * invLength, rightPos.Y * invLength, rightPos.Z * invLength );
				invLength *= PlanetStandardRadius + heightScale * m_Generator.GetHeight( rightPoints[ row ].X, rightPoints[ row ].Y, rightPoints[ row ].Z );
				rightPoints[ row ] += new Vector3( rightPos.X * invLength, rightPos.Y * invLength, rightPos.Z * invLength );
			}

			int max = res - 1;
			for ( int row = 0; row < res; ++row )
			{
			    curVertex = firstVertex + ( row * res );
			    for ( int col = 0; col < res; ++col, ++curVertex )
			    {
					//	TODO: AP: This is very very slow
					Vector3 left = ( col == 0 ? leftPoints[ row ] : ( curVertex - 1 )->Position ) - curVertex->Position;
					Vector3 up = ( ( row == 0 ) ? upPoints[ col ] : ( curVertex - res )->Position ) - curVertex->Position;
					Vector3 right = ( col == max ? rightPoints[ row ] : ( curVertex + 1 )->Position ) - curVertex->Position;
					Vector3 down = ( ( row == max ) ? downPoints[ col ] : ( curVertex + res )->Position ) - curVertex->Position;

					Vector3 acc = Vector3.Cross( up, left );
					acc.IpAdd( Vector3.Cross( right, up ) );
					acc.IpAdd( Vector3.Cross( down, right ) );
					acc.IpAdd( Vector3.Cross( left, down ) );
					curVertex->Normal = acc.MakeNormal( );
			    }
			}
		}

		#endregion

		#region Private Members

		private readonly ISphereTerrainGenerator m_Generator;

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
