using System;
using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Renderers;
using Poc1.Core.Interfaces.Rendering;
using Poc1.Core.Interfaces.Rendering.Cameras;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical.Renderers
{

	/// <summary>
	/// Sphere planet atmospheric scattering renderer
	/// </summary>
	public class SpherePlanetAtmosphereScatteringRenderer : SpherePlanetEnvironmentRenderer, IPlanetAtmosphereRenderer
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SpherePlanetAtmosphereScatteringRenderer( )
		{
			//	Load in atmosphere effect
			m_Techniques = new TechniqueSelector( "Effects/Planets/atmosphereShell.cgfx", true, "DefaultTechnique" );
		}


		/// <summary>
		/// Renders the atmosphere
		/// </summary>
		/// <param name="context">Rendering context</param>
		public override void Render( IUniRenderContext context )
		{
			if ( context.CurrentPass != UniRenderPass.FarObjects )
			{
				return;
			}
			SetupAtmosphereEffectParameters( context.Camera, m_Techniques.Effect, false, true );
			context.ApplyTechnique( m_Techniques, AtmosphereGeometry );
		}

		#region IPlanetAtmosphereRenderer Members

		/// <summary>
		/// Sets up an effect used to render objects as seen through this atmosphere
		/// </summary>
		/// <param name="camera">Current camera</param>
		/// <param name="effect">Effect to set up</param>
		/// <param name="farObject">Effect is set up for a far away object</param>
		/// <remarks>
		/// Will expect certain variables to be available in the effect
		/// </remarks>
		public void SetupObjectEffect( IUniCamera camera, IEffect effect, bool farObject )
		{
			SetupAtmosphereEffectParameters( camera, effect, true, farObject );
		}

		#endregion

		#region Private Members

		private IRenderable m_AtmosphereGeometry;
		private TechniqueSelector m_Techniques;
		private Units.AstroRenderUnits m_AtmosphereGeometryRadius;

		/// <summary>
		/// Gets the atmosphere geometry object
		/// </summary>
		private IRenderable AtmosphereGeometry
		{
			get
			{
				IPlanetAtmosphereScatteringModel model = GetModel<IPlanetAtmosphereScatteringModel>( );
				if ( model == null )
				{
					throw new InvalidOperationException( "Can't generate atmosphere geometry without an associated atmospheric scattering model" );
				}

				Units.AstroRenderUnits expectedRadius = Planet.Model.Radius.ToAstroRenderUnits + model.Thickness.ToAstroRenderUnits;

				if ( ( m_AtmosphereGeometry == null ) || ( m_AtmosphereGeometryRadius != expectedRadius ) )
				{
					float renderRadius = ( float )expectedRadius;
					Graphics.Draw.StartCache( );
					Graphics.Draw.Sphere( null, Point3.Origin, renderRadius, 60, 60 );
					m_AtmosphereGeometry = Graphics.Draw.StopCache( );
					m_AtmosphereGeometryRadius = expectedRadius;
				}
				return m_AtmosphereGeometry;
			}
		}

		/// <summary>
		/// Sets up parameters for effects that use atmospheric rendering
		/// </summary>
		/// <param name="camera">Current camera</param>
		/// <param name="effect">Effect to set up</param>
		/// <param name="objectRendering">True if the effect is being used to render an object in the atmosphere</param>
		/// <param name="farObject">If true, distances are passed to the effect in astro render units. Otherwise, render units are used.</param>
		private void SetupAtmosphereEffectParameters( IUniCamera camera, IEffect effect, bool objectRendering, bool farObject )
		{
			IPlanetAtmosphereScatteringModel model = GetModel<IPlanetAtmosphereScatteringModel>( );
			if ( model == null )
			{
				return;
			}
			if ( farObject )
			{
				SetupAstroRenderUnitAtmosphereEffectParameters( model, camera, effect );
			}
			else
			{
				SetupRenderUnitAtmosphereEffectParameters( model, camera, effect );
			}

			//	Set up parameters shared between astro and close atmosphere rendering
			effect.Parameters[ "AtmObjectColourOutput" ].Set( ( int )model.ObjectColourOutput );
			effect.Parameters[ "AtmHgCoeff" ].Set( model.PhaseCoefficient );
			effect.Parameters[ "AtmPhaseWeight" ].Set( model.PhaseWeight );
			effect.Parameters[ "ScatteringTexture" ].Set( model.ScatteringTexture );
			effect.Parameters[ "AtmMiePhaseWeight" ].Set( model.MiePhaseWeight );
			if ( objectRendering )
			{
				effect.Parameters[ "OpticalDepthTexture" ].Set( model.OpticalDepthTexture );
			}
		}

		private void SetupRenderUnitAtmosphereEffectParameters( IPlanetAtmosphereScatteringModel model, IUniCamera camera, IEffect effect )
		{
			Point3 localPos = Units.RenderUnits.MakeRelativePoint( Planet.Transform.Position, camera.Position );
			float planetRadius = Planet.Model.Radius.ToRenderUnits;
			float atmosphereRadius = model.Thickness.ToRenderUnits;
			float height = localPos.DistanceTo( Point3.Origin ) - planetRadius;
			float normHeight = height > atmosphereRadius ? 1 : ( height / atmosphereRadius );

			Vector3 viewDir = camera.Frame.ZAxis;
			effect.Parameters[ "AtmViewVec" ].Set( localPos.ToVector3( ).MakeNormal( ) );
		//	effect.Parameters[ "AtmViewPos" ].Set( atmPos );
			effect.Parameters[ "AtmViewPos" ].Set( localPos );
			effect.Parameters[ "AtmViewDir" ].Set( viewDir );
			effect.Parameters[ "AtmViewHeight" ].Set( normHeight );
			effect.Parameters[ "AtmInnerRadius" ].Set( planetRadius );
			effect.Parameters[ "AtmThickness" ].Set( atmosphereRadius );
			effect.Parameters[ "AtmOuterRadius" ].Set( planetRadius + atmosphereRadius );
		}

		private void SetupAstroRenderUnitAtmosphereEffectParameters( IPlanetAtmosphereScatteringModel model, IUniCamera camera, IEffect effect )
		{
			Point3 localPos = Units.AstroRenderUnits.MakeRelativePoint( Planet.Transform.Position, camera.Position );
			float planetRadius = ( float )Planet.Model.Radius.ToAstroRenderUnits;
			float atmosphereRadius = ( float )model.Thickness.ToAstroRenderUnits;
			float height = localPos.DistanceTo( Point3.Origin ) - planetRadius;
			float normHeight = height > atmosphereRadius ? 1 : ( height / atmosphereRadius );

			//	float clampedLength = planetRadius + clampedHeight;
			//	Vector3 groundVec = localPos.ToVector3( ).MakeLength( clampedLength );
			Point3 atmPos = localPos; // Point3.Origin + groundVec;

			Vector3 viewDir = camera.Frame.ZAxis;
			effect.Parameters[ "AtmViewPos" ].Set( atmPos );
			effect.Parameters[ "AtmViewVec" ].Set( atmPos.ToVector3( ).MakeNormal( ) );
			effect.Parameters[ "AtmViewDir" ].Set( viewDir );
			effect.Parameters[ "AtmViewHeight" ].Set( normHeight );
			effect.Parameters[ "AtmInnerRadius" ].Set( planetRadius );
			effect.Parameters[ "AtmThickness" ].Set( atmosphereRadius );
			effect.Parameters[ "AtmOuterRadius" ].Set( planetRadius + atmosphereRadius );
		}

		#endregion

	}

}
