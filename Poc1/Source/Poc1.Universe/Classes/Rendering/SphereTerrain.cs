using System.Drawing;
using System.Drawing.Imaging;
using Poc1.Universe.Interfaces.Rendering;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using RbGraphics = Rb.Rendering.Graphics;
using Poc1.Fast.Terrain;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Planetary terrain for spherical planets
	/// </summary>
	public class SphereTerrain : IPlanetTerrain
	{
		/// <summary>
		/// Sets the planet
		/// </summary>
		public SphereTerrain( SpherePlanet planet )
		{
			m_Planet = planet;
			float radius = ( float )UniUnits.RenderUnits.FromUniUnits( m_Planet.Radius );
			float height = ( float )UniUnits.RenderUnits.FromUniUnits( UniUnits.Metres.ToUniUnits( planet.TerrainHeightRange ) );
			m_RenderRadius = radius;

			m_Gen.SetSmallestStepSize( 0.05f, 0.05f );

			float terrainFunctionRadius = ( float )( ( ( double )planet.Radius ) / 20000000.0f );
			m_Gen.Setup( radius, radius + height, terrainFunctionRadius );
		}

		/// <summary>
		/// Generates the terrain cube map texture for this planet
		/// </summary>
		/// <param name="res">Cube map face resolution</param>
		/// <returns>Returns the texture</returns>
		public ICubeMapTexture CreatePlanetTexture( int res )
		{
			long start = TinyTime.CurrentTime;
			ICubeMapTexture texture = RbGraphics.Factory.CreateCubeMapTexture( );

			texture.Build
				(
					GenerateCubeMapFace( CubeMapFace.PositiveX, res ),
					GenerateCubeMapFace( CubeMapFace.NegativeX, res ),
					GenerateCubeMapFace( CubeMapFace.PositiveY, res ),
					GenerateCubeMapFace( CubeMapFace.NegativeY, res ),
					GenerateCubeMapFace( CubeMapFace.PositiveZ, res ),
					GenerateCubeMapFace( CubeMapFace.NegativeZ, res ),
					true
				);

			long end = TinyTime.CurrentTime;

			GraphicsLog.Info("Generated {0}x{0} planet texture using {1} generator type", res, m_Gen.GetType( ) );
			GraphicsLog.Info( "Time taken to generate planet texture: {0:F2} seconds", TinyTime.ToSeconds( start, end ) );

			return texture;
		}

		#region IPlanetTerrain Members

		/// <summary>
		/// Patches are defined in a local space. This determines the planet-space parameters of a patch
		/// </summary>
		public void SetPatchPlanetParameters( ITerrainPatch patch )
		{
			Point3 edge = patch.LocalOrigin + ( patch.LocalUAxis / 2 );
			Point3 centre = edge + ( patch.LocalVAxis / 2 );

			Point3 plEdge = ( edge.ToVector3( ).MakeNormal( ) * m_RenderRadius ).ToPoint3( );
			Point3 plCentre = ( centre.ToVector3( ).MakeNormal( ) * m_RenderRadius ).ToPoint3( );

			patch.SetPlanetParameters( plCentre, plCentre.DistanceTo( plEdge ) );
		}

		/// <summary>
		/// Generates vertices for a patch
		/// </summary>
		/// <param name="patch">Patch</param>
		/// <param name="res">Patch resolution</param>
		/// <param name="uvRes">UV coordinate resolution over entire patch</param>
		/// <param name="firstVertex">Patch vertices</param>
		public unsafe void GenerateTerrainPatchVertices( ITerrainPatch patch, int res, float uvRes, TerrainVertex* firstVertex )
		{
			SetPatchPlanetParameters( patch );
			m_Gen.GenerateVertices( patch.LocalOrigin, patch.LocalUStep, patch.LocalVStep, res, res, uvRes, firstVertex );
		}

		/// <summary>
		/// Generates vertices for a patch. Calculates maximum error between this patch and next higher detail patch
		/// </summary>
		/// <param name="patch">Patch</param>
		/// <param name="res">Patch resolution</param>
		/// <param name="uvRes">UV coordinate resolution over entire patch</param>
		/// <param name="firstVertex">Patch vertices</param>
		/// <param name="error">Maximum error value between this patch and higher level patch</param>
		public unsafe void GenerateTerrainPatchVertices( ITerrainPatch patch, int res, float uvRes, TerrainVertex* firstVertex, out float error )
		{
			SetPatchPlanetParameters( patch );

			m_Gen.GenerateVertices( patch.LocalOrigin, patch.LocalUStep, patch.LocalVStep, res, res, uvRes, firstVertex, out error );
			GenerateTextureCoordinates( res, firstVertex, uvRes );
		}

		#endregion

		#region Private Members

		private unsafe static void GenerateTextureCoordinates( int res, TerrainVertex* firstVertex, float uvRes )
		{
			//	TODO: AP: Add to fast version...
			float toUv = uvRes / ( res - 1 );
			TerrainVertex* vert = firstVertex;
			for ( int row = 0; row < res; ++row )
			{
				for ( int col = 0; col < res; ++col, ++vert )
				{
					vert->TerrainUv = new Point2( col * toUv, row * toUv );
				}
			}
		}

		private readonly SphereTerrainGenerator m_Gen = new SphereTerrainGenerator( TerrainGeneratorType.Ridged, true, TimeSeed );
		private readonly SpherePlanet m_Planet;
		private readonly float m_RenderRadius;

		/// <summary>
		/// Gets the current time in ticks. Used to seed the PNG in the terrain generator
		/// </summary>
		private static uint TimeSeed
		{
			get
			{
				return ( uint )System.DateTime.Now.Ticks;
			}
		}

		/// <summary>
		/// Generates cube map face bitmaps
		/// </summary>
		private unsafe Bitmap GenerateCubeMapFace( CubeMapFace face, int res )
		{
			Bitmap bmp = new Bitmap( res, res, PixelFormat.Format32bppArgb );
			BitmapData bmpData = bmp.LockBits( new System.Drawing.Rectangle( 0, 0, res, res ), ImageLockMode.WriteOnly, bmp.PixelFormat );
			m_Gen.GenerateTerrainPropertyCubeMapFace( face, bmp.Width, bmp.Height, bmpData.Stride, ( byte* )bmpData.Scan0 );
			bmp.UnlockBits( bmpData );

			return bmp;
		}

		#endregion
	}
}
