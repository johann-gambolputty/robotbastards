using System;
using Poc1.Fast.Terrain;
using Poc1.Universe;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Rendering;
using Poc1.Universe.Classes.Rendering;
using Rb.Assets;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Poc1.Universe.Classes.Cameras;
using Rb.Rendering.Interfaces.Objects.Cameras;
using Rb.Rendering.Textures;

namespace Poc1.PlanetBuilder
{
	public class TerrainMesh : IPlanetTerrain, IRenderable
	{
		/// <summary>
		/// Terrain mesh setup constructor
		/// </summary>
		/// <param name="width">Mesh width</param>
		/// <param name="maxHeight">Maximum mesh height</param>
		/// <param name="depth">Mesh depth</param>
		public TerrainMesh( float width, float maxHeight, float depth )
		{
			m_MaxHeight = maxHeight;

			IEffect effect = ( IEffect )AssetManager.Instance.Load( "Effects/Planets/terrestrialPlanetTerrain.cgfx" );
			m_TerrainTechnique = new TechniqueSelector( effect, "DefaultTechnique" );

			TextureLoader.TextureLoadParameters loadParameters = new TextureLoader.TextureLoadParameters( );
			loadParameters.GenerateMipMaps = true;
			m_TerrainPackTexture = ( ITexture2d )AssetManager.Instance.Load( "Terrain/defaultSet0 Pack.jpg", loadParameters );
			m_TerrainTypesTexture = ( ITexture2d )AssetManager.Instance.Load( "Terrain/defaultSet0 Distribution.bmp" );

			m_NoiseTexture = ( ITexture2d )AssetManager.Instance.Load( "Terrain/Noise.jpg" );


			Point3 origin = new Point3( -width / 2, 0, -depth / 2 );
			Vector3 xAxis = new Vector3( width, 0, 0 );
			Vector3 zAxis = new Vector3( 0, 0, depth );

			m_Vertices = new TerrainQuadPatchVertices( );
			m_RootPatch = new TerrainQuadPatch( m_Vertices, origin, xAxis, zAxis, 256.0f );
		}


		#region IPlanetTerrain Members

		/// <summary>
		/// Patches are defined in a local space. This determines the planet-space parameters of a patch
		/// </summary>
		public void SetPatchPlanetParameters( ITerrainPatch patch )
		{
			Point3 edge = patch.LocalOrigin + ( patch.LocalUAxis / 2 );
			Point3 centre = edge + ( patch.LocalVAxis / 2 );

			Point3 plEdge = edge;// ( edge.ToVector3( ).MakeNormal( ) * m_RenderRadius ).ToPoint3( );
			Point3 plCentre = centre;// ( centre.ToVector3( ).MakeNormal( ) * m_RenderRadius ).ToPoint3( );

			patch.SetPlanetParameters( plCentre, plCentre.DistanceTo( plEdge ) );
		}

		/// <summary>
		/// Generates vertices for a patch
		/// </summary>
		public unsafe void GenerateTerrainPatchVertices( ITerrainPatch patch, int res, float uvRes, TerrainVertex* firstVertex )
		{
			SetPatchPlanetParameters( patch );
			m_Gen.GenerateVertices( patch.LocalOrigin, patch.LocalUStep, patch.LocalVStep, res, res, uvRes, firstVertex );
		}

		/// <summary>
		/// Generates vertices for a patch
		/// </summary>
		public unsafe void GenerateTerrainPatchVertices( ITerrainPatch patch, int res, float uvRes, TerrainVertex* firstVertex, out float error )
		{
			SetPatchPlanetParameters( patch );
			m_Gen.GenerateVertices( patch.LocalOrigin, patch.LocalUStep, patch.LocalVStep, res, res, uvRes, firstVertex, out error );
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders the terrain
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			IProjectionCamera camera = ( IProjectionCamera )Graphics.Renderer.Camera;
			Graphics.Draw.Sphere( Graphics.Surfaces.Blue, Point3.Origin, 0.2f );
			Graphics.Draw.Sphere( Graphics.Surfaces.Blue, Point3.Origin + Vector3.XAxis, 0.2f );
			Graphics.Draw.Sphere( Graphics.Surfaces.Blue, Point3.Origin + Vector3.YAxis, 0.2f );
			Graphics.Draw.Sphere( Graphics.Surfaces.Blue, Point3.Origin + Vector3.ZAxis, 0.2f );
			return;
			m_TerrainTechnique.Effect.Parameters[ "TerrainPackTexture" ].Set( m_TerrainPackTexture );
			m_TerrainTechnique.Effect.Parameters[ "TerrainTypeTexture" ].Set( m_TerrainTypesTexture );
			m_TerrainTechnique.Effect.Parameters[ "NoiseTexture" ].Set( m_NoiseTexture );

			m_RootPatch.UpdateLod( ( ( ICamera3 )camera ).Frame.Translation, this, camera );
			m_RootPatch.Update( camera, this );

			m_Vertices.VertexBuffer.Begin( );
			context.ApplyTechnique( m_TerrainTechnique, m_RootPatch );
			m_Vertices.VertexBuffer.End( );
		}

		#endregion

		#region Private Members

		private readonly TechniqueSelector m_TerrainTechnique;
		private readonly ITexture2d m_TerrainPackTexture;
		private readonly ITexture2d m_TerrainTypesTexture;
		private readonly ITexture2d m_NoiseTexture;
		private readonly UniPoint3 m_Centre = new UniPoint3( );
		private readonly float m_MaxHeight;
		private readonly TerrainQuadPatchVertices m_Vertices;
		private readonly TerrainQuadPatch m_RootPatch;
		private SphereTerrainGenerator m_Gen = DefaultTerrainGenerator( );

		private static SphereTerrainGenerator DefaultTerrainGenerator( )
		{
			TerrainFunction heightFunction = new TerrainFunction( TerrainFunctionType.RidgedFractal );
			TerrainFunction groundFunction = new TerrainFunction( TerrainFunctionType.SimpleFractal );

			return new SphereTerrainGenerator( heightFunction, groundFunction );
		}


		/// <summary>
		/// Gets the current time in ticks. Used to seed the PNG in the terrain generator
		/// </summary>
		private static uint TimeSeed
		{
			get
			{
				return ( uint )DateTime.Now.Ticks;
			}
		}

		#endregion

	}
}
