using System;
using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Rendering;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical.Renderers
{
	/// <summary>
	/// Renders a cloud shell
	/// </summary>
	public class SpherePlanetSimpleCloudShellRenderer : SpherePlanetEnvironmentRenderer
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SpherePlanetSimpleCloudShellRenderer( ) :
			this( "Effects/Planets/cloudLayer.cgfx" )
		{
		}
		
		/// <summary>
		/// Setup constructor
		/// </summary>
		public SpherePlanetSimpleCloudShellRenderer( string effectPath )
		{
			EffectAssetHandle effect = new EffectAssetHandle( effectPath, true );
		//	effect.OnReload += Effect_OnReload;
			m_Technique = new TechniqueSelector( effect, "DefaultTechnique" );

			m_TextureBuilder = new CloudCubeMapTextureBuilder( );
		}

		/// <summary>
		/// Sets up parameters for effects that use cloud rendering
		/// </summary>
		/// <param name="effect">Effect to set up</param>
		public void SetupCloudEffectParameters( IEffect effect )
		{
			//	TODO: AP: ...
			effect.Parameters[ "CloudTexture" ].Set( m_TextureBuilder.CurrentTexture );
			effect.Parameters[ "NextCloudTexture" ].Set( m_TextureBuilder.NextTexture );
		}

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public override void Render( IUniRenderContext context )
		{
			IPlanetSimpleCloudModel simpleCloudModel = GetModel<IPlanetSimpleCloudModel>( );
			if ( simpleCloudModel == null )
			{
				DestroyCloudShell( );
				return;
			}
			if ( DoesCloudShellRequireRebuild( simpleCloudModel ) )
			{
				BuildCloudShell( simpleCloudModel );
			}
			SetupCloudEffectParameters( m_Technique.Effect );
			context.ApplyTechnique( m_Technique, m_CloudShell );
		}

		#region Private Members

		private Units.Metres m_BuildRadius = new Units.Metres( );
		private IRenderable m_CloudShell;
		private readonly TechniqueSelector m_Technique;
		private readonly CloudCubeMapTextureBuilder m_TextureBuilder;

		/// <summary>
		/// Returns true if the cloud model parameters differ from those used to create the cloud shell renderable object
		/// </summary>
		private bool DoesCloudShellRequireRebuild( IPlanetSimpleCloudModel simpleCloudModel )
		{
			if ( m_CloudShell == null )
			{
				return true;
			}
			return m_BuildRadius != ( simpleCloudModel.CloudLayerHeight + Planet.Model.Radius );
		}

		/// <summary>
		/// Creates a cloud shell renderable from a cloud model
		/// </summary>
		private void BuildCloudShell( IPlanetSimpleCloudModel simpleCloudModel )
		{
			DestroyCloudShell( );

			m_BuildRadius = ( simpleCloudModel.CloudLayerHeight + Planet.Model.Radius );

			Graphics.Draw.StartCache( );
			Graphics.Draw.Sphere( null, Point3.Origin, m_BuildRadius.ToRenderUnits, 50, 50 );
			m_CloudShell = Graphics.Draw.StopCache( );
		}

		/// <summary>
		/// Destroys the current cloud shell
		/// </summary>
		private void DestroyCloudShell( )
		{
			if ( m_CloudShell is IDisposable )
			{
				( ( IDisposable )m_CloudShell ).Dispose( );
				m_CloudShell = null;
			}
		}

		#endregion
	}

}
