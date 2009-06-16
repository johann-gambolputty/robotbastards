
using Rb.Core.Maths;

namespace Poc1.Test.AtmosphereTest
{
	/// <summary>
	/// Defines parameters for the atmosphere calculator
	/// </summary>
	public class AtmosphereCalculatorModel
	{
		public AtmosphereCalculatorModel Clone( )
		{
			AtmosphereCalculatorModel clone = new AtmosphereCalculatorModel( );
			clone.m_Samples = m_Samples;
			clone.m_PlanetRadius = m_PlanetRadius;
			clone.m_AtmosphereRadius = m_AtmosphereRadius;
			clone.m_RayleighDensityScaleHeightFraction = m_RayleighDensityScaleHeightFraction;
			clone.m_MieDensityScaleHeightFraction = m_MieDensityScaleHeightFraction;
			clone.m_SunDirection = m_SunDirection;
			return clone;
		}

		public float PlanetRadius
		{
			get { return m_PlanetRadius; }
			set { m_PlanetRadius = value; }
		}

		public float AtmosphereRadius
		{
			get { return m_AtmosphereRadius; }
			set { m_AtmosphereRadius = value; }
		}

		public int Samples
		{
			get { return m_Samples; }
			set { m_Samples = value; }
		}

		public float RayleighDensityScaleHeightFraction
		{
			get { return m_RayleighDensityScaleHeightFraction; }
			set { m_RayleighDensityScaleHeightFraction = value; }
		}

		public float MieDensityScaleHeightFraction
		{
			get { return m_MieDensityScaleHeightFraction; }
			set { m_MieDensityScaleHeightFraction = value; }
		}

		public Vector3 SunDirection
		{
			get { return m_SunDirection; }
			set { m_SunDirection = value; }
		}

		/// <summary>
		/// Gets the 3 rayleigh coefficients
		/// </summary>
		public float[] RayleighCoefficients
		{
			get { return m_RayleighCoefficients; }
		}

		/// <summary>
		/// Gets the 3 mie coefficients
		/// </summary>
		public float[] MieCoefficients
		{
			get { return m_MieCoefficients; }
		}

		#region Private Members

		private Vector3 m_SunDirection;
		private int m_Samples;
		private float m_PlanetRadius;
		private float m_AtmosphereRadius;
		private float m_RayleighDensityScaleHeightFraction;
		private float m_MieDensityScaleHeightFraction;
		private readonly float[] m_RayleighCoefficients = new float[ 3 ];
		private readonly float[] m_MieCoefficients = new float[ 3 ];

		#endregion
	}
}
