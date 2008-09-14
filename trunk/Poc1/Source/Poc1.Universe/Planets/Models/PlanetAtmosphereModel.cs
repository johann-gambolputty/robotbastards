using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Models;
using IPlanet=Poc1.Universe.Interfaces.Planets.IPlanet;

namespace Poc1.Universe.Planets.Models
{
	/// <summary>
	/// Implementation of <see cref="IPlanetAtmosphereModel"/>
	/// </summary>
	public class PlanetAtmosphereModel : IPlanetAtmosphereModel
	{
		#region IPlanetEnvironmentModel Members

		/// <summary>
		/// Event, invoked when the atmosphere model changes
		/// </summary>
		public event System.EventHandler ModelChanged;

		/// <summary>
		/// Gets/sets the planet associated with this model
		/// </summary>
		public IPlanet Planet
		{
			get { return m_Planet; }
			set { m_Planet = value; }
		}

		#endregion

		#region IPlanetAtmosphereModel Members

		/// <summary>
		/// The thickness of the atmosphere
		/// </summary>
		public Units.Metres AtmosphereThickness
		{
			get { return m_AtmosphereThickness; }
			set
			{
				m_AtmosphereThickness = value;
				OnModelChanged( );
			}
		}

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
				m_PhaseCoefficient = value;
				OnModelChanged( );
			}
		}

		/// <summary>
		/// Gets/sets the weighting applied to the phase function.
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
				m_PhaseWeight = value;
				OnModelChanged( );
			}
		}

		/// <summary>
		/// Gets/sets the weighting applied to the mie phase function
		/// </summary>
		public float MiePhaseWeight
		{
			get { return m_MiePhaseWeight; }
			set { m_MiePhaseWeight = value; }
		}

		#endregion

		#region Private Members

		private Units.Metres m_AtmosphereThickness = new Units.Metres( 8000 );
		private float m_PhaseWeight = 1.0f;
		private float m_PhaseCoefficient = -0.9999f;
		private float m_MiePhaseWeight = 0.0f;
		private IPlanet m_Planet;

		/// <summary>
		/// Called when the model is changed
		/// </summary>
		private void OnModelChanged( )
		{
			if ( ModelChanged != null )
			{
				ModelChanged( this, null );
			}
		}

		#endregion
	}
}
