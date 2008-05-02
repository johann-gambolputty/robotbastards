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
		/// Sets the planet
		/// </summary>
		public SphereTerrain( SpherePlanet planet )
		{
			m_Planet = planet;
		}

		private readonly SpherePlanet m_Planet;

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
		/// <returns>Centre point of the patch, in render unit space</returns>
		public unsafe Point3 GenerateTerrainPatchVertices( Point3 origin, Vector3 uStep, Vector3 vStep, int res, TerrainVertex* firstVertex )
		{
			float radius = ( float )UniUnits.RenderUnits.FromUniUnits( m_Planet.Radius );
			float height = ( float )UniUnits.RenderUnits.FromUniUnits( UniUnits.Metres.ToUniUnits( 12000 ) );

			m_Gen.SetHeightRange( radius, radius + height );
			m_Gen.GenerateVertices( origin, uStep, vStep, res, res, firstVertex, sizeof( TerrainVertex ), 0, 12 );

			Point3 centre = origin + ( uStep * res / 2 ) + ( vStep * res / 2 );
			return ( centre.ToVector3( ) * radius ).ToPoint3( );
		}

		/// <summary>
		/// Generates vertices for a patch. Calculates maximum error between this patch and next higher detail patch
		/// </summary>
		/// <param name="origin">Patch origin</param>
		/// <param name="uStep">Offset between row vertices</param>
		/// <param name="vStep">Offset between column vertices</param>
		/// <param name="res">Patch resolution</param>
		/// <param name="firstVertex">Patch vertices</param>
		/// <param name="error">Maximum error value between this patch and higher level patch</param>
		/// <returns>Centre point of the patch, in render unit space</returns>
		public unsafe Point3 GenerateTerrainPatchVertices( Point3 origin, Vector3 uStep, Vector3 vStep, int res, TerrainVertex* firstVertex, out float error)
		{
			float radius = ( float )UniUnits.RenderUnits.FromUniUnits( m_Planet.Radius );
			float height = ( float )UniUnits.RenderUnits.FromUniUnits( UniUnits.Metres.ToUniUnits( 12000 ) );

			m_Gen.SetHeightRange( radius, radius + height );
			m_Gen.GenerateVertices( origin, uStep, vStep, res, res, firstVertex, sizeof( TerrainVertex ), 0, 12, out error );

			Point3 centre = origin + ( uStep * res / 2 ) + ( vStep * res / 2 );
			return ( centre.ToVector3( ) * radius ).ToPoint3( );
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
