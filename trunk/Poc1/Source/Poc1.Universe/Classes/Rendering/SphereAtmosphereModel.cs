using Poc1.Universe.Interfaces.Rendering;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Atmosphere model for spherical planets
	/// </summary>
	public class SphereAtmosphereModel : IPlanetAtmosphereModel
	{
		/// <summary>
		/// The radius of the atmosphere (measured in uni-units)
		/// </summary>
		public long Radius
		{
			get { return m_Radius; }
			set { m_Radius = value; }
		}

		/// <summary>
		/// Gets/sets the coefficient for the atmosphere phase function (Heyney-Greenstein)
		/// </summary>
		/// <remarks>
		/// Good values are -0.75 to -0.99
		/// </remarks>
		public float PhaseCoefficient
		{
			get { return m_PhaseCoefficient; }
			set { m_PhaseCoefficient = value; }
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
			set { m_PhaseWeight = value; }
		}

		#region Private Members

		private float m_PhaseCoefficient = -0.9999f;
		private float m_PhaseWeight = 1.0f;
		private long m_Radius = UniUnits.Metres.ToUniUnits( 8000 );

		#endregion
	}
}
