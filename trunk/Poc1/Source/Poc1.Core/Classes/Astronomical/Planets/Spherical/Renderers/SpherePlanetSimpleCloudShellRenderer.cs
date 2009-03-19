using System;
using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical.Renderers
{
	/// <summary>
	/// Renders a cloud shell
	/// </summary>
	public class SpherePlanetSimpleCloudShellRenderer : SpherePlanetEnvironmentRenderer
	{
		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public override void Render( IUniRenderContext context )
		{
			ISimplePlanetCloudModel cloudModel = GetModel<ISimplePlanetCloudModel>( );
			if ( cloudModel == null )
			{
				DestroyCloudShell( );
				return;
			}
			if ( DoesCloudShellRequireRebuild( cloudModel ) )
			{
				BuildCloudShell( cloudModel );
			}
			throw new NotImplementedException( );
			//	SetupCloudEffectParameters( m_Technique.Effect );
			//	context.ApplyTechnique( m_Technique, m_CloudShell );
		}

		#region Private Members

		private Units.Metres m_BuildRadius = new Units.Metres( );
		private IRenderable m_CloudShell;

		/// <summary>
		/// Returns true if the cloud model parameters differ from those used to create the cloud shell renderable object
		/// </summary>
		private bool DoesCloudShellRequireRebuild( ISimplePlanetCloudModel cloudModel )
		{
			if ( m_CloudShell == null )
			{
				return true;
			}
			return m_BuildRadius != ( cloudModel.CloudLayerHeight + Planet.Model.Radius );
		}

		/// <summary>
		/// Creates a cloud shell renderable from a cloud model
		/// </summary>
		private void BuildCloudShell( ISimplePlanetCloudModel cloudModel )
		{
			DestroyCloudShell( );

			m_BuildRadius = ( cloudModel.CloudLayerHeight + Planet.Model.Radius );

			//	Graphics.Draw.StartCache( );
			//	Graphics.Draw.Sphere( null, Point3.Origin, BuildRadius.ToRenderUnits, 50, 50 );
			//	m_CloudShell = Graphics.Draw.StopCache( );
			throw new NotImplementedException( );
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
