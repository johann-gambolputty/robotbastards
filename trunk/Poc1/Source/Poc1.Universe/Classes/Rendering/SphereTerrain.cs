using System.Drawing;
using System.Drawing.Imaging;
using Poc1.Universe.Interfaces.Rendering;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Rendering;
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
					GenerateCubeMapFace( CubeMapFace.PositiveX, res, PixelFormat.Format32bppArgb ),
					GenerateCubeMapFace( CubeMapFace.NegativeX, res, PixelFormat.Format32bppArgb ),
					GenerateCubeMapFace( CubeMapFace.PositiveY, res, PixelFormat.Format32bppArgb ),
					GenerateCubeMapFace( CubeMapFace.NegativeY, res, PixelFormat.Format32bppArgb ),
					GenerateCubeMapFace( CubeMapFace.PositiveZ, res, PixelFormat.Format32bppArgb ),
					GenerateCubeMapFace( CubeMapFace.NegativeZ, res, PixelFormat.Format32bppArgb ),
					true
				);

			long end = TinyTime.CurrentTime;

			GraphicsLog.Info("Generated {0}x{0} planet texture using {1} generator type", res, m_Gen.GetType( ) );
			GraphicsLog.Info( "Time taken to generate planet texture: {0:F2} seconds", TinyTime.ToSeconds( start, end ) );

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
			float heightScale = 8.0f;
			
			m_Gen.SetHeightRange( PlanetStandardRadius, PlanetStandardRadius + heightScale );
			m_Gen.GenerateVertices( origin, uStep, vStep, res, res, firstVertex, sizeof( TerrainVertex ), 0, 12 );
		}

		#endregion

		#region Private Members

		private readonly Fast.SphereTerrainGenerator m_Gen = new Fast.SphereTerrainGenerator( Fast.TerrainGeneratorType.Ridged, 0 );

		/// <summary>
		/// Generates cube map face bitmaps
		/// </summary>
		private unsafe Bitmap GenerateCubeMapFace( CubeMapFace face, int res, PixelFormat format )
		{
			Bitmap bmp = new Bitmap( res, res, format );
			BitmapData bmpData = bmp.LockBits( new System.Drawing.Rectangle( 0, 0, res, res ), ImageLockMode.WriteOnly, format );
			m_Gen.GenerateTexture( face, bmp.PixelFormat, bmp.Width, bmp.Height, bmpData.Stride, ( byte* )bmpData.Scan0 );
			bmp.UnlockBits( bmpData );

			return bmp;
		}

		#endregion
	}
}
