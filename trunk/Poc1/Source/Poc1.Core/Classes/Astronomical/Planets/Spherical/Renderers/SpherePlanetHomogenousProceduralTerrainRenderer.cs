
using System;
using Poc1.Core.Classes.Astronomical.Planets.Spherical.Renderers.PackTextures;
using Poc1.Core.Classes.Astronomical.Planets.Spherical.Renderers.Patches;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Renderers.Patches;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical;
using Poc1.Core.Interfaces.Rendering;
using Poc1.Fast.Terrain;
using Rb.Core.Maths;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical.Renderers
{
	/// <summary>
	/// Renders homogenous procedural terrain for spherical planets
	/// </summary>
	public class SpherePlanetHomogenousProceduralTerrainRenderer : SpherePlanetEnvironmentRenderer, ITerrainPatchGenerator
	{
		/// <summary>
		/// Default construcor
		/// </summary>
		public SpherePlanetHomogenousProceduralTerrainRenderer( )
		{
			m_Renderer = new SpherePlanetTerrainPatchRenderer( this );
			m_Technique = new SpherePlanetPackTextureTechnique( null );
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
			m_Technique.Planet = Planet;
			m_Renderer.Render( context, context.Camera, m_Technique );
		}

		#region ITerrainPatchGenerator Members

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

		#region Private Members

		/// <summary>
		/// Terrain generator minimum step size. 4 samples are taken from the terrain geometry model, using
		/// this step size, around a central sample, to calculate the terrain normal
		/// </summary>
		private const float MinimumStepSize = 0.01f;

		private readonly SpherePlanetPackTextureTechnique m_Technique;
		private readonly SpherePlanetTerrainPatchRenderer m_Renderer;
		private TerrainGenerator m_TerrainGenerator;

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
		public void SetPatchPlanetParameters( ITerrainPatch patch )
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
