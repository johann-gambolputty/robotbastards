using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Classes.Astronomical.Planets.Models
{
	/// <summary>
	/// Planet atmosphere scattering model
	/// </summary>
	public class PlanetAtmosphereScatteringModel : PlanetAtmosphereModel, IPlanetAtmosphereScatteringModel
	{
		#region IPlanetAtmosphereScatteringModel Members

		/// <summary>
		/// Gets/sets object colour output
		/// </summary>
		public ScatteringAtmosphereObjectColourOutput ObjectColourOutput
		{
			get { return m_ObjectColourOutput; }
			set { m_ObjectColourOutput = value; }
		}

		private ScatteringAtmosphereObjectColourOutput m_ObjectColourOutput;

		/// <summary>
		/// Gets/sets the atmosphere phase coefficient
		/// </summary>
		/// <remarks>
		/// Good values are -0.75 to -0.99
		/// </remarks>
		public float PhaseCoefficient
		{
			get { return m_PhaseCoefficient; }
			set
			{
				if ( m_PhaseCoefficient != value )
				{
					m_PhaseCoefficient = value;
					OnModelChanged( );
				}
			}
		}

		/// <summary>
		/// Gets/sets the weighting applied to the rayleigh phase function.
		/// </summary>
		/// <remarks>
		/// A weighting of zero means that the phase function has no effect, and the
		/// displayed colours are from in-scatter/out-scatter only.
		/// </remarks>
		public float PhaseWeight
		{
			get { return m_PhaseWeight; }
			set
			{
				if ( m_PhaseWeight != value )
				{
					m_PhaseWeight = value;
					OnModelChanged( );
				}
			}
		}

		/// <summary>
		/// Gets/sets the weighting applied to the mie phase function.
		/// </summary>
		public float MiePhaseWeight
		{
			get { return m_MiePhaseWeight; }
			set
			{
				if ( m_MiePhaseWeight != value )
				{
					m_MiePhaseWeight = value;
					OnModelChanged( );
				}
			}
		}

		/// <summary>
		/// Gets/sets the scattering lookup texture data
		/// </summary>
		public ITexture3d ScatteringTexture
		{
			get { return m_ScatteringTexture; }
			set
			{
				if ( m_ScatteringTexture != value )
				{
					m_ScatteringTexture = value;
					OnModelChanged( );
				}
			}
		}

		/// <summary>
		/// Gets/sets the optical depth lookup texture data
		/// </summary>
		public ITexture2d OpticalDepthTexture
		{
			get { return m_OpticalDepthTexture; }
			set
			{
				if ( m_OpticalDepthTexture != value )
				{
					m_OpticalDepthTexture = value;
					OnModelChanged( );
				}
			}
		}

		#endregion

		#region Private Members

		private float m_PhaseCoefficient	= -0.75f;
		private float m_PhaseWeight			= 1.0f;
		private float m_MiePhaseWeight		= 1.0f;
		private ITexture3d m_ScatteringTexture;
		private ITexture2d m_OpticalDepthTexture;

		#endregion
	}
}
