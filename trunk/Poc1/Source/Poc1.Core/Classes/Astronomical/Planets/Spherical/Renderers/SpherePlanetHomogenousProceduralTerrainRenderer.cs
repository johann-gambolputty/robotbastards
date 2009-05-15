
using System;
using System.Drawing;
using System.Drawing.Imaging;
using Poc1.Core.Classes.Astronomical.Planets.Spherical.Renderers.Terrain.PackTextures;
using Poc1.Core.Classes.Astronomical.Planets.Spherical.Renderers.Terrain.Patches;
using Poc1.Core.Classes.Profiling;
using Poc1.Core.Interfaces.Astronomical.Planets;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Renderers;
using Poc1.Core.Interfaces.Astronomical.Planets.Renderers.Terrain.PackTextures;
using Poc1.Core.Interfaces.Astronomical.Planets.Renderers.Terrain.Patches;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical.Renderers;
using Poc1.Core.Interfaces.Rendering;
using Poc1.Fast.Terrain;
using Rb.Assets;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Textures;
using RbGraphics = Rb.Rendering.Graphics;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical.Renderers
{
	/// <summary>
	/// Renders homogenous procedural terrain for spherical planets
	/// </summary>
	public class SpherePlanetHomogenousProceduralTerrainRenderer : SpherePlanetEnvironmentRenderer, IPlanetHomogenousProceduralTerrainRenderer, ITerrainPatchGenerator, ITerrainPackTextureProvider, ISpherePlanetTerrainRenderer
	{
		/// <summary>
		/// Default construcor
		/// </summary>
		public SpherePlanetHomogenousProceduralTerrainRenderer( )
		{
			m_Renderer = new SpherePlanetTerrainPatchRenderer( this );
			m_Technique = new SpherePlanetPackTextureTechnique( this );

			m_PackTexture = ( ITexture2d )AssetManager.Instance.Load( "Terrain\\dirt0.jpg", new TextureLoadParameters( true ) );
			m_LookupTexture = RbGraphics.Factory.CreateTexture2d( );
			Texture2dData lookupData = new Texture2dData( 1, 1, TextureFormat.R8G8B8 );
			m_LookupTexture.Create( lookupData, true );
		}

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public override void Render( IUniRenderContext context )
		{
			if ( context.CurrentPass == UniRenderPass.FarObjects )
			{
				return;
			}
			GameProfiles.Game.Rendering.PlanetRendering.TerrainRendering.Begin( );

			//	Reflected objects require flipped face windings
			bool flipWinding = ( context.CurrentPass == UniRenderPass.CloseReflectedObjects );

			m_Technique.Planet = Planet;
			if ( m_FrameCountAtLastUpdate != context.RenderFrameCounter )
			{
				m_Renderer.Update( context.Camera, Planet.Transform );
				m_FrameCountAtLastUpdate = context.RenderFrameCounter;
			}
			m_Renderer.Render( context, context.Camera, m_Technique, flipWinding );

			GameProfiles.Game.Rendering.PlanetRendering.TerrainRendering.End( );

		}

		#region ISpherePlanetTerrainRenderer Members

		/// <summary>
		/// Creates a face for the marble texture cube map
		/// </summary>
		public unsafe Bitmap CreateMarbleTextureFace( CubeMapFace face, int width, int height )
		{
			Bitmap bmp = new Bitmap( width, height, PixelFormat.Format24bppRgb );
			BitmapData bmpData = bmp.LockBits( new System.Drawing.Rectangle( 0, 0, width, height ), ImageLockMode.WriteOnly, bmp.PixelFormat );
			byte* pixels = ( byte* )bmpData.Scan0;
			SafeTerrainGenerator.GenerateTerrainPropertyCubeMapFace( face, width, height, bmpData.Stride, pixels );
			bmp.UnlockBits( bmpData );
			return bmp;
		}

		#endregion

		#region IPlanetHomogenousProceduralTerrainRenderer Members

		/// <summary>
		/// Refreshes this renderer
		/// </summary>
		public void Refresh( )
		{
			m_TerrainGenerator = null;	//	This forces the generator to be recreated by the SafeTerrainGenerator property
			m_Renderer.Refresh( Planet );
		}

		#endregion

		#region ITerrainPatchGenerator Members

		/// <summary>
		/// Gets patch build parameters
		/// </summary>
		public TerrainPatchBuildParameters BuildParameters
		{
			get { return m_PatchBuildParameters; }
		}

		/// <summary>
		/// Generates vertices for a patch
		/// </summary>
		/// <param name="patch">Patch</param>
		/// <param name="res">Patch resolution</param>
		/// <param name="firstVertex">Patch vertices</param>
		public unsafe void GenerateTerrainPatchVertices( ITerrainPatch patch, int res, TerrainPatchVertex* firstVertex )
		{
			SetPatchPlanetParameters( patch );
			SafeTerrainGenerator.GenerateVertices( patch.LocalOrigin, patch.LocalUStep, patch.LocalVStep, res, res, patch.Uv, patch.UvResolution, firstVertex );
		}

		/// <summary>
		/// Generates vertices for a patch. Calculates maximum error between this patch and next higher detail patch
		/// </summary>
		/// <param name="patch">Patch</param>
		/// <param name="res">Patch resolution</param>
		/// <param name="firstVertex">Patch vertices</param>
		/// <param name="error">Maximum error value between this patch and higher level patch</param>
		public unsafe void GenerateTerrainPatchVertices( ITerrainPatch patch, int res, TerrainPatchVertex* firstVertex, out float error )
		{
			SetPatchPlanetParameters( patch );
			SafeTerrainGenerator.GenerateVertices( patch.LocalOrigin, patch.LocalUStep, patch.LocalVStep, res, res, patch.Uv, patch.UvResolution, firstVertex, out error );
		}

		#endregion

		#region ITerrainPackTextureProvider Members

		/// <summary>
		/// Gets the terrain pack texture
		/// </summary>
		public ITexture2d PackTexture
		{
			get { return m_PackTexture; }
			set { m_PackTexture = value; }
		}

		/// <summary>
		/// Gets the pack lookup texture
		/// </summary>
		public ITexture2d LookupTexture
		{
			get { return m_LookupTexture; }
			set { m_LookupTexture = value; }
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Called after this environment renderer has been added to the specified planet renderer
		/// </summary>
		/// <param name="renderer">Planet renderer that this environment renderer was added to</param>
		protected override void OnAddedToPlanetRenderer( IPlanetRenderer renderer )
		{
			base.OnAddedToPlanetRenderer( renderer );
			Refresh( );
		}

		#endregion

		#region Private Members

		/// <summary>
		/// Terrain generator minimum step size. 4 samples are taken from the terrain geometry model, using
		/// this step size, around a central sample, to calculate the terrain normal
		/// </summary>
		private const float MinimumStepSize = 0.01f;

		private readonly TerrainPatchBuildParameters m_PatchBuildParameters = new TerrainPatchBuildParameters( );
		private readonly SpherePlanetPackTextureTechnique m_Technique;
		private readonly SpherePlanetTerrainPatchRenderer m_Renderer;
		private TerrainGenerator m_TerrainGenerator;
		private ITexture2d m_PackTexture;
		private ITexture2d m_LookupTexture;
		private ulong m_FrameCountAtLastUpdate = unchecked( ( ulong )-1 );

		/// <summary>
		/// Creates a terrain generator from a model definition
		/// </summary>
		private static TerrainGenerator CreateTerrainGenerator( ISpherePlanet planet, IPlanetHomogenousProceduralTerrainModel model )
		{
			float radius = planet.Model.Radius.ToRenderUnits;
			float height = model.MaximumHeight.ToRenderUnits;

			TerrainGenerator generator = new TerrainGenerator( TerrainGeometry.Sphere, model.HeightFunction, model.GroundOffsetFunction );
			generator.Setup( 1024, radius, radius + height );
			generator.SetSmallestStepSize( MinimumStepSize, MinimumStepSize );

			return generator;
		}

		/// <summary>
		/// Gets the current terrain generator. If there isn't one, a flat terrain generator is created.
		/// </summary>
		private TerrainGenerator SafeTerrainGenerator
		{
			get
			{
				if ( m_TerrainGenerator == null )
				{
					IPlanetHomogenousProceduralTerrainModel model = Planet.Model.GetModel<IPlanetHomogenousProceduralTerrainModel>( );
					if ( model == null )
					{
						throw new InvalidOperationException( "Expected a terrain model to be assigned before attempting to use the terrain generator" );
					}
					m_TerrainGenerator = CreateTerrainGenerator( Planet, model );
				}
				return m_TerrainGenerator;
			}
		}


		/// <summary>
		/// Patches are defined in a local space. This determines the planet-space parameters of a patch
		/// </summary>
		private void SetPatchPlanetParameters( ITerrainPatch patch )
		{
			float radius = Planet.Model.Radius.ToRenderUnits;

			Point3 edge = patch.LocalOrigin + ( patch.LocalUAxis / 2 );
			Point3 centre = edge + ( patch.LocalVAxis / 2 );

			Point3 plEdge = ( edge.ToVector3( ).MakeNormal( ) * radius ).ToPoint3( );
			Point3 plCentre = ( centre.ToVector3( ).MakeNormal( ) * radius ).ToPoint3( );

			patch.SetPlanetParameters( plCentre, plCentre.DistanceTo( plEdge ) );
		}

		#endregion

	}
}
