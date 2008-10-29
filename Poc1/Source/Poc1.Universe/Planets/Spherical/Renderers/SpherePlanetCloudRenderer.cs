using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Spherical;
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
	public class SpherePlanetCloudRenderer : ISpherePlanetCloudRenderer
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

		#region IPlanetEnvironmentRenderer Members

		/// <summary>
		/// Gets/sets the planet associated with this renderer
		/// </summary>
		public IPlanet Planet
		{
			get { return m_SpherePlanet; }
			set
			{
				if ( m_SpherePlanet != null )
				{
					m_SpherePlanet.CloudModel.ModelChanged -= CloudModel_ModelChanged;
				}
				m_SpherePlanet = ( ISpherePlanet )value;
				DisposableHelper.Dispose( m_CloudSphere );
				m_CloudSphere = null;
				if ( m_SpherePlanet != null )
				{
					m_SpherePlanet.CloudModel.ModelChanged += CloudModel_ModelChanged;
				}
			}
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders the cloud model
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			if ( Planet == null || Planet.CloudModel == null )
			{
				return;
			}
			if ( m_CloudSphere == null )
			{
				m_CloudSphere = CreateCloudSphere( );
			}
			m_Technique.Effect.Parameters[ "CloudTexture" ].Set( m_SpherePlanet.SphereCloudModel.CloudTexture );
			m_Technique.Effect.Parameters[ "NextCloudTexture" ].Set( m_SpherePlanet.SphereCloudModel.CloudTexture );	//	TODO: AP: ...
			context.ApplyTechnique( m_Technique, m_CloudSphere );
		}

		#endregion

		#region Private Members

		private ISpherePlanet m_SpherePlanet;
		private readonly TechniqueSelector m_Technique;
		private IRenderable m_CloudSphere;

		/// <summary>
		/// Creates cloud sphere geometry
		/// </summary>
		private IRenderable CreateCloudSphere( )
		{
			float radius = ( m_SpherePlanet.Radius + Planet.CloudModel.CloudLayerMinHeight ).ToRenderUnits;

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

		/// <summary>
		/// Called when the cloud model is changed
		/// </summary>
		private void CloudModel_ModelChanged( object sender, System.EventArgs e )
		{
			DisposableHelper.Dispose( m_CloudSphere );
			m_CloudSphere = null;
		}

		#endregion
	}
}
