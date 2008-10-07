using System;
using Poc1.Fast.Terrain;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Renderers.Patches;
using Poc1.Universe.Interfaces.Rendering;
using Poc1.Universe.Planets.Spherical.Renderers.Patches;
using Rb.Assets;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Interfaces.Objects.Cameras;
using IPlanetTerrainModel=Poc1.Universe.Interfaces.Planets.Models.IPlanetTerrainModel;
using ITerrainPatch=Poc1.Universe.Interfaces.Planets.Renderers.Patches.ITerrainPatch;

namespace Poc1.PlanetBuilder
{
	public class TerrainMesh : IPlanetTerrainModel, IRenderable, ITerrainPatchGenerator
	{
		/// <summary>
		/// Terrain mesh setup constructor
		/// </summary>
		/// <param name="width">Mesh width</param>
		/// <param name="maxHeight">Maximum mesh height</param>
		/// <param name="depth">Mesh depth</param>
		public TerrainMesh( float width, float maxHeight, float depth )
		{
			m_PatchScale = width;
			m_Gen = DefaultTerrainGenerator( width, maxHeight );

			IEffect effect = ( IEffect )AssetManager.Instance.Load( "Effects/Planets/terrestrialPlanetTerrain.cgfx" );
			m_TerrainTechnique = new TechniqueSelector( effect, "DefaultTechnique" );

			//TextureLoader.TextureLoadParameters loadParameters = new TextureLoader.TextureLoadParameters( );
			//loadParameters.GenerateMipMaps = true;
			//m_TerrainPackTexture = ( ITexture2d )AssetManager.Instance.Load( "Terrain/defaultSet0 Pack.jpg", loadParameters );
			//m_TerrainTypesTexture = ( ITexture2d )AssetManager.Instance.Load( "Terrain/defaultSet0 Distribution.bmp" );

			m_NoiseTexture = ( ITexture2d )AssetManager.Instance.Load( "Terrain/Noise.jpg" );

			Point3 origin = new Point3( -width / 2, 0, -depth / 2 );
			Vector3 xAxis = new Vector3( width, 0, 0 );
			Vector3 zAxis = new Vector3( 0, 0, depth );
			
			m_MeshOrigin = origin;
			m_MeshXAxis = xAxis;
			m_MeshZAxis = zAxis;

			RegenerateMesh( );
		}

		/// <summary>
		/// Regenerates this mesh
		/// </summary>
		public void RegenerateMesh( )
		{
			if ( m_RootPatch != null )
			{
				m_RootPatch = null;
				GC.Collect( );
			}
			m_Vertices = new TerrainPatchVertices( );
			m_RootPatch = new TerrainPatch( m_Vertices, m_MeshOrigin, m_MeshXAxis, m_MeshZAxis, new Point2( 0, 0 ), 256.0f );

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

			Point3 plEdge = edge;// ( edge.ToVector3( ).MakeNormal( ) * m_RenderRadius ).ToPoint3( );
			Point3 plCentre = centre;// ( centre.ToVector3( ).MakeNormal( ) * m_RenderRadius ).ToPoint3( );

			patch.SetPlanetParameters( plCentre, plCentre.DistanceTo( plEdge ) );
		}

		public void SetupTerrain( float maxHeight, TerrainFunction heightFunction, TerrainFunction groundFunction )
		{
			TerrainGenerator gen = new TerrainGenerator( TerrainGeometry.Plane, heightFunction, groundFunction );
			gen.SetSmallestStepSize( 0.001f, 0.001f );

			//	TODO: AP: Mesh setup 
			gen.Setup( m_PatchScale, 0, maxHeight );

			m_Gen = gen;
		}

		/// <summary>
		/// Generates vertices for a patch
		/// </summary>
		public unsafe void GenerateTerrainPatchVertices( ITerrainPatch patch, int res, TerrainVertex* firstVertex )
		{
			SetPatchPlanetParameters( patch );
			m_Gen.GenerateVertices( patch.LocalOrigin, patch.LocalUStep, patch.LocalVStep, res, res, patch.Uv, patch.UvResolution, firstVertex );
		}

		/// <summary>
		/// Generates vertices for a patch
		/// </summary>
		public unsafe void GenerateTerrainPatchVertices( ITerrainPatch patch, int res, TerrainVertex* firstVertex, out float error )
		{
			SetPatchPlanetParameters( patch );
			m_Gen.GenerateVertices( patch.LocalOrigin, patch.LocalUStep, patch.LocalVStep, res, res, patch.Uv, patch.UvResolution, firstVertex, out error );
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders the terrain
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			if ( m_RootPatch == null )
			{
				return;
			}
			IProjectionCamera camera = ( IProjectionCamera )Graphics.Renderer.Camera;

			m_RootPatch.UpdateLod( ( ( ICamera3 )camera ).Frame.Translation, this, camera );
			m_RootPatch.Update( camera, this );

			ITexture2d lookupTexture = TerrainTypesTexture; // TerrainTypeTextureBuilder.Instance.LookupTexture;
			ITexture2d packTexture = TerrainPackTexture; // TerrainTypeTextureBuilder.Instance.PackTexture;

			m_TerrainTechnique.Effect.Parameters[ "TerrainPackTexture" ].Set( packTexture );
			m_TerrainTechnique.Effect.Parameters[ "TerrainTypeTexture" ].Set( lookupTexture );
			m_TerrainTechnique.Effect.Parameters[ "NoiseTexture" ].Set( m_NoiseTexture );

			m_Vertices.VertexBuffer.Begin( );
			context.ApplyTechnique( m_TerrainTechnique, m_RootPatch );
		//	m_RState.Begin( );
		//	m_RootPatch.Render( context );
		//	m_RState.End( );
			m_Vertices.VertexBuffer.End( );
		}

		#endregion

		#region Private Members

		private ITexture2d m_TerrainTypesTexture;
		private ITexture2d m_TerrainPackTexture;
		private readonly float m_PatchScale;
		private readonly TechniqueSelector m_TerrainTechnique;
		private readonly ITexture2d m_NoiseTexture;
		private TerrainPatchVertices m_Vertices;
		private TerrainPatch m_RootPatch;
		private TerrainGenerator m_Gen;
		private readonly Point3 m_MeshOrigin;
		private readonly Vector3 m_MeshXAxis;
		private readonly Vector3 m_MeshZAxis;


		private static TerrainGenerator DefaultTerrainGenerator( float patchScale, float maxHeight )
		{
			TerrainFunction heightFunction = new TerrainFunction( TerrainFunctionType.SimpleFractal );
			heightFunction.Parameters.OutputScale = 1.0f;
			TerrainFunction groundFunction = new TerrainFunction( TerrainFunctionType.SimpleFractal );
			heightFunction.Parameters.OutputScale = 0.5f;

			TerrainGenerator gen = new TerrainGenerator( TerrainGeometry.Plane, heightFunction, groundFunction );

			gen.SetSmallestStepSize( 0.05f, 0.05f );
			gen.Setup( patchScale, 0, maxHeight );

			//	TODO: AP: Remove hack
		//	DebugInfo.DisableTerainSkirts = true;

			return gen;
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


		#region IPlanetTerrainModel Members


		public Units.Metres MaximumHeight
		{
			get
			{
				throw new Exception( "The method or operation is not implemented." );
			}
			set
			{
				throw new Exception( "The method or operation is not implemented." );
			}
		}

		#endregion

		#region IPlanetEnvironmentModel Members

		public event EventHandler ModelChanged;

		public IPlanet Planet
		{
			get
			{
				throw new Exception( "The method or operation is not implemented." );
			}
			set
			{
				throw new Exception( "The method or operation is not implemented." );
			}
		}

		#endregion
	}
}
