using System.Drawing;
using System.Drawing.Imaging;
using Poc1.Universe.Interfaces.Rendering;
using Rb.Assets;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Textures;
using RbGraphics = Rb.Rendering.Graphics;
using Poc1.Fast.Terrain;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Planetary terrain for spherical planets
	/// </summary>
	public class SphereTerrainModel : IPlanetTerrainModel
	{
		/// <summary>
		/// Sets the planet
		/// </summary>
		public SphereTerrainModel( SpherePlanet planet )
		{
			m_Planet = planet;
			float radius = ( float )UniUnits.RenderUnits.FromUniUnits( m_Planet.Radius );
			float height = ( float )UniUnits.RenderUnits.FromUniUnits( UniUnits.Metres.ToUniUnits( planet.TerrainHeightRange ) );
			m_RenderRadius = radius;

			float terrainFunctionRadius = ( float )( ( ( double )planet.Radius ) / 20000000.0f );

			//	
			TextureLoader.TextureLoadParameters loadParameters = new TextureLoader.TextureLoadParameters( );
			loadParameters.GenerateMipMaps = true;
			m_TerrainPackTexture = ( ITexture2d )AssetManager.Instance.Load( "Terrain/defaultSet0 Pack.jpg", loadParameters );
			m_TerrainTypesTexture = ( ITexture2d )AssetManager.Instance.Load( "Terrain/defaultSet0 Distribution.bmp" );

			//	Create the default terrain generator
			m_Gen = FlatTerrainGenerator( ); // DefaultTerrainGenerator( terrainFunctionRadius );

			// NOTE: AP: Patch scale is irrelevant, because vertices are projected onto the function sphere anyway
			m_Gen.Setup( 1024, radius, radius + height );
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
		/// Gets/sets the terrain types texture
		/// </summary>
		public ITexture2d TerrainTypesTexture
		{
			get { return m_TerrainTypesTexture; }
			set { m_TerrainTypesTexture = value; }
		}

		/// <summary>
		/// Gets/sets the terrain pack texture
		/// </summary>
		public ITexture2d TerrainPackTexture
		{
			get { return m_TerrainPackTexture; }
			set { m_TerrainPackTexture = value; }
		}

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
		/// Sets up the terrain functions
		/// </summary>
		/// <param name="maxHeight">Maximum height of the terrain</param>
		/// <param name="heightFunction">The terrain height function</param>
		/// <param name="groundFunction">The terrain ground offset function</param>
		public void SetupTerrain( float maxHeight, TerrainFunction heightFunction, TerrainFunction groundFunction )
		{
			float radius = ( float )UniUnits.RenderUnits.FromUniUnits( m_Planet.Radius );
			float height = ( float )UniUnits.RenderUnits.FromUniUnits( UniUnits.Metres.ToUniUnits( m_Planet.TerrainHeightRange ) );

			// NOTE: AP: Patch scale is irrelevant, because vertices are projected onto the function sphere anyway
			m_Gen = new TerrainGenerator( TerrainGeometry.Sphere, heightFunction, groundFunction );
			m_Gen.Setup( 1024, radius, radius + height );
			m_Gen.SetSmallestStepSize( MinimumStepSize, MinimumStepSize );
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

		private ITexture2d				m_TerrainTypesTexture;
		private ITexture2d				m_TerrainPackTexture;
		private TerrainGenerator		m_Gen;
		private readonly SpherePlanet	m_Planet;
		private readonly float			m_RenderRadius;

		private const float				MinimumStepSize = 0.01f;

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

		private static TerrainGenerator FlatTerrainGenerator( )
		{
			TerrainFunction heightFunction = new TerrainFunction( TerrainFunctionType.Flat );
			TerrainGenerator gen = new TerrainGenerator( TerrainGeometry.Sphere, heightFunction );
			gen.SetSmallestStepSize( MinimumStepSize, MinimumStepSize );

			return gen;
		}

		private static TerrainGenerator DefaultTerrainGenerator( float functionScale )
		{
			TerrainFunction heightFunction = new TerrainFunction( TerrainFunctionType.RidgedFractal );
			heightFunction.Parameters.FunctionScale = functionScale;

			TerrainFunction groundFunction = new TerrainFunction( TerrainFunctionType.SimpleFractal );
			heightFunction.Parameters.FunctionScale = functionScale;

			( ( FractalTerrainParameters )heightFunction.Parameters ).Seed = TimeSeed;
			( ( FractalTerrainParameters )groundFunction.Parameters ).Seed = TimeSeed;

			TerrainGenerator gen = new TerrainGenerator( TerrainGeometry.Sphere, heightFunction, groundFunction );
			gen.SetSmallestStepSize( MinimumStepSize, MinimumStepSize );

			return gen;
		}

		/// <summary>
		/// Gets the current time in ticks. Used to seed the PNG in the terrain generator
		/// </summary>
		private static int TimeSeed
		{
			get
			{
				return ( int )System.DateTime.Now.Ticks;
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
