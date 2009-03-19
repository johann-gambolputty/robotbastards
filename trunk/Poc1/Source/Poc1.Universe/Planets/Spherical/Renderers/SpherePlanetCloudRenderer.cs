using System;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Renderers;
using Poc1.Universe.Interfaces.Planets.Spherical.Renderers;
using Rb.Assets.Interfaces;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Planets.Spherical.Renderers
{
	/// <summary>
	/// Renderer for sphere planet clouds
	/// </summary>
	public class SpherePlanetCloudRenderer : AbstractSpherePlanetEnvironmentRenderer, ISpherePlanetCloudRenderer
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SpherePlanetCloudRenderer( )
		{
			EffectAssetHandle effect = new EffectAssetHandle( "Effects/Planets/cloudLayer.cgfx", true );
			effect.OnReload += Effect_OnReload;

			m_Technique = new TechniqueSelector( effect, "DefaultTechnique" );
		}

		#region IPlanetCloudRenderer Members

		/// <summary>
		/// Sets up parameters for effects that use cloud rendering
		/// </summary>
		/// <param name="effect">Effect to set up</param>
		public void SetupCloudEffectParameters( IEffect effect )
		{
			effect.Parameters[ "CloudTexture" ].Set( SpherePlanet.PlanetModel.SphereCloudModel.CloudTexture );
			effect.Parameters[ "NextCloudTexture" ].Set( SpherePlanet.PlanetModel.SphereCloudModel.CloudTexture );	//	TODO: AP: ...
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders the cloud model
		/// </summary>
		/// <param name="context">Rendering context</param>
		public override void Render( IRenderContext context )
		{
			if ( Planet == null || Planet.PlanetModel.CloudModel == null )
			{
				return;
			}
			if ( m_CloudSphere == null || HasCloudModelChanged( ) )
			{
				m_CloudSphere = CreateCloudSphere( );
			}
			SetupCloudEffectParameters( m_Technique.Effect );
			context.ApplyTechnique( m_Technique, m_CloudSphere );
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Assigns/unassigns this renderer to/from a planet
		/// </summary>
		protected override void AssignToPlanet( IPlanetRenderer renderer, bool remove )
		{
			renderer.CloudRenderer = remove ? null : this;
			DisposableHelper.Dispose( m_CloudSphere );
			m_CloudSphere = null;
		}

		#endregion

		#region Private Members

		private readonly TechniqueSelector m_Technique;
		private IRenderable m_CloudSphere;
		private Units.Metres m_BuildRadius = new Units.Metres( 0 );

		/// <summary>
		/// Returns true if the cloud model has changed
		/// </summary>
		private bool HasCloudModelChanged( )
		{
			return m_BuildRadius != ( SpherePlanet.PlanetModel.Radius + SpherePlanet.PlanetModel.CloudModel.CloudLayerMinHeight );
		}

		/// <summary>
		/// Creates cloud sphere geometry
		/// </summary>
		private IRenderable CreateCloudSphere( )
		{
			m_BuildRadius = SpherePlanet.PlanetModel.Radius + SpherePlanet.PlanetModel.CloudModel.CloudLayerMinHeight;

			float radius = m_BuildRadius.ToRenderUnits;

			Graphics.Draw.StartCache( );
			Graphics.Draw.Sphere( null, Point3.Origin, radius, 50, 50 );
			return Graphics.Draw.StopCache( );
		}

		/// <summary>
		/// Called when the cloud effect is reloaded
		/// </summary>
		private void Effect_OnReload( ISource source )
		{
			//	Re-select the same technique from the same effect
			m_Technique.Select( m_Technique.Effect, m_Technique.Technique.Name );
		}

		#endregion
	}
}
