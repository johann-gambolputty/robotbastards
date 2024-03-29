using System;
using Poc1.Universe.Classes.Cameras;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Renderers;
using Poc1.Universe.Interfaces.Planets.Spherical.Renderers;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Planets.Spherical.Renderers
{
	/// <summary>
	/// Atmosphere renderer for spherical planets
	/// </summary>
	public class SpherePlanetAtmosphereRenderer : AbstractSpherePlanetEnvironmentRenderer, ISpherePlanetAtmosphereRenderer
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SpherePlanetAtmosphereRenderer( )
		{
			//	Load in atmosphere effect
			m_Effect = new EffectAssetHandle( "Effects/Planets/atmosphereShell.cgfx", true );
			m_Effect.OnReload += Effect_OnReload;
			m_Techniques = new TechniqueSelector( m_Effect, "DefaultTechnique" );
		}

		#region ISpherePlanetAtmosphereRenderer Members

		/// <summary>
		/// Sets the lookup textures required by the atmosphere renderer
		/// </summary>
		/// <param name="scatteringTexture">Lookup table for in- and out-scattering coefficients</param>
		/// <param name="opticalDepthTexture">Lookup table for optical depth</param>
		public void SetLookupTextures( ITexture3d scatteringTexture, ITexture2d opticalDepthTexture )
		{
			m_ScatteringTexture = scatteringTexture;
			m_OpticalDepthTexture = opticalDepthTexture;
		}


		/// <summary>
		/// Sets up parameters for effects that use atmospheric rendering
		/// </summary>
		/// <param name="effect">Effect to set up</param>
		/// <param name="objectRendering">True if the effect is being used to render an object in the atmosphere</param>
		/// <param name="deepRender">If true, distances are passed to the effect in astro render units. Otherwise, render units are used.</param>
		public void SetupAtmosphereEffectParameters( IEffect effect, bool objectRendering, bool deepRender )
		{
			if ( Planet == null || Planet.PlanetModel.AtmosphereModel == null )
			{
				return;
			}
			IUniCamera camera = UniCamera.Current;

			if ( deepRender )
			{
				SetupAstroRenderUnitAtmosphereEffectParameters( camera, effect );
			}
			else
			{
				SetupRenderUnitAtmosphereEffectParameters( camera, effect );	
			}

			//	Set up parameters shared between astro and close atmosphere rendering
			IPlanetAtmosphereModel model = Planet.PlanetModel.AtmosphereModel;
			effect.Parameters[ "AtmHgCoeff" ].Set( model.PhaseCoefficient );
			effect.Parameters[ "AtmPhaseWeight" ].Set( model.PhaseWeight );
			effect.Parameters[ "ScatteringTexture" ].Set( m_ScatteringTexture );
			effect.Parameters[ "AtmMiePhaseWeight" ].Set( model.MiePhaseWeight );
			if ( objectRendering )
			{
				effect.Parameters[ "OpticalDepthTexture" ].Set( m_OpticalDepthTexture );
			}
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders the atmosphere
		/// </summary>
		/// <param name="context">Rendering context</param>
		public override void Render( IRenderContext context )
		{
			//	Atmosphere has no close rendering component
			throw new NotSupportedException( "Atmosphere only renders using DeepRender() method" );
		}

		#endregion

		#region IUniRenderable Members

		/// <summary>
		/// Renders the atmosphere
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void DeepRender( IRenderContext context )
		{
			if ( Planet == null || Planet.PlanetModel.AtmosphereModel == null )
			{
				return;
			}

			SetupAtmosphereEffectParameters( m_Techniques.Effect, false, true );
			context.ApplyTechnique( m_Techniques, AtmosphereGeometry );
		}

		#endregion

		#region Protected Members
		
		/// <summary>
		/// Assigns/unassigns this renderer to/from a planet
		/// </summary>
		protected override void AssignToPlanet( IPlanetRenderer renderer, bool remove )
		{
			renderer.AtmosphereRenderer = remove ? null : this;
		}

		#endregion

		#region Private Members

		private IRenderable			m_AtmosphereGeometry;
		private TechniqueSelector	m_Techniques;
		private ITexture3d			m_ScatteringTexture;
		private ITexture2d			m_OpticalDepthTexture;
		private EffectAssetHandle	m_Effect;

		/// <summary>
		/// Gets the atmosphere geometry object
		/// </summary>
		private IRenderable AtmosphereGeometry
		{
			get
			{
				if ( m_AtmosphereGeometry == null )
				{
					float renderRadius = ( float )( SpherePlanet.PlanetModel.Radius.ToAstroRenderUnits + SpherePlanet.PlanetModel.AtmosphereModel.AtmosphereThickness.ToAstroRenderUnits );
					Graphics.Draw.StartCache( );
					Graphics.Draw.Sphere( null, Point3.Origin, renderRadius, 60, 60 );
					m_AtmosphereGeometry = Graphics.Draw.StopCache( );
				}
				return m_AtmosphereGeometry;
			}
		}

		/// <summary>
		/// Called when the atmosphere effect is reloaded
		/// </summary>
		private void Effect_OnReload( Rb.Assets.Interfaces.ISource obj )
		{
			m_Techniques = new TechniqueSelector( m_Effect.Asset, m_Techniques.Name );
		}

		private void SetupRenderUnitAtmosphereEffectParameters( IUniCamera camera, IEffect effect )
		{
			Point3 localPos = Units.RenderUnits.MakeRelativePoint( Planet.Transform.Position, camera.Position );
			float planetRadius = SpherePlanet.PlanetModel.Radius.ToRenderUnits;
			float atmosphereRadius = SpherePlanet.PlanetModel.AtmosphereModel.AtmosphereThickness.ToRenderUnits;
			float height = localPos.DistanceTo( Point3.Origin ) - planetRadius;
			float clampedHeight = Utils.Clamp( height, 0, atmosphereRadius );
			float normHeight = clampedHeight / atmosphereRadius;

			float clampedLength = planetRadius + clampedHeight;
			Vector3 groundVec = localPos.ToVector3( ).MakeLength( clampedLength );
			Point3 atmPos = Point3.Origin + groundVec;

			Vector3 viewDir = UniCamera.Current.Frame.ZAxis;
			effect.Parameters[ "AtmViewPosLength" ].Set( atmPos.DistanceTo( Point3.Origin ) );
			effect.Parameters[ "AtmViewPos" ].Set( atmPos.X, atmPos.Y, atmPos.Z );
			effect.Parameters[ "AtmViewDir" ].Set( viewDir.X, viewDir.Y, viewDir.Z );
			effect.Parameters[ "AtmViewHeight" ].Set( normHeight );
			effect.Parameters[ "AtmInnerRadius" ].Set( planetRadius );
			effect.Parameters[ "AtmThickness" ].Set( atmosphereRadius );
			effect.Parameters[ "AtmOuterRadius" ].Set( planetRadius + atmosphereRadius );
		}

		private void SetupAstroRenderUnitAtmosphereEffectParameters( IUniCamera camera, IEffect effect )
		{
			Point3 localPos = Units.AstroRenderUnits.MakeRelativePoint( Planet.Transform.Position, camera.Position );
			float planetRadius = ( float )SpherePlanet.PlanetModel.Radius.ToAstroRenderUnits;
			float atmosphereRadius = ( float )Planet.PlanetModel.AtmosphereModel.AtmosphereThickness.ToAstroRenderUnits;
			float height = localPos.DistanceTo( Point3.Origin ) - planetRadius;
			float clampedHeight = Utils.Clamp( height, 0, atmosphereRadius );
			float normHeight = clampedHeight / atmosphereRadius;

		//	float clampedLength = planetRadius + clampedHeight;
		//	Vector3 groundVec = localPos.ToVector3( ).MakeLength( clampedLength );
			Point3 atmPos = localPos; // Point3.Origin + groundVec;

			Vector3 viewDir = UniCamera.Current.Frame.ZAxis;
			effect.Parameters[ "AtmViewPosLength" ].Set( atmPos.DistanceTo( Point3.Origin ) );
			effect.Parameters[ "AtmViewPos" ].Set( atmPos.X, atmPos.Y, atmPos.Z );
			effect.Parameters[ "AtmViewDir" ].Set( viewDir.X, viewDir.Y, viewDir.Z );
			effect.Parameters[ "AtmViewHeight" ].Set( normHeight );
			effect.Parameters[ "AtmInnerRadius" ].Set( planetRadius );
			effect.Parameters[ "AtmThickness" ].Set( atmosphereRadius );
			effect.Parameters[ "AtmOuterRadius" ].Set( planetRadius + atmosphereRadius );
		}

		#endregion

	}
}
