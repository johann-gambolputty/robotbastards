
using System;
using Rb.Core.Maths;

namespace Poc1.Test.AtmosphereTest
{
	/// <summary>
	/// Defines parameters for the atmosphere calculator
	/// </summary>
	public class AtmosphereCalculatorModel
	{
		public AtmosphereCalculatorModel( )
		{
			ResetComponentScales( );
		}

		public AtmosphereCalculatorModel Clone( )
		{
			AtmosphereCalculatorModel clone = new AtmosphereCalculatorModel( );
			clone.m_Samples = m_Samples;
			clone.m_PlanetRenderRadius = m_PlanetRenderRadius;
			clone.m_AtmosphereRenderRadius = m_AtmosphereRenderRadius;
			clone.m_RayleighDensityScaleHeightFraction = m_RayleighDensityScaleHeightFraction;
			clone.m_MieDensityScaleHeightFraction = m_MieDensityScaleHeightFraction;
			clone.m_SunDirection = m_SunDirection;
			clone.m_RayleighCoefficients[ 0 ] = m_RayleighCoefficients[ 0 ];
			clone.m_RayleighCoefficients[ 1 ] = m_RayleighCoefficients[ 1 ];
			clone.m_RayleighCoefficients[ 2 ] = m_RayleighCoefficients[ 2 ];
			clone.m_MieCoefficients[ 0 ] = m_MieCoefficients[ 0 ];
			clone.m_MieCoefficients[ 1 ] = m_MieCoefficients[ 1 ];
			clone.m_MieCoefficients[ 2 ] = m_MieCoefficients[ 2 ];
			return clone;
		}

		public float PlanetRenderRadius
		{
			get { return m_PlanetRenderRadius; }
			set { m_PlanetRenderRadius = value; }
		}

		public float AtmosphereRenderRadius
		{
			get { return m_AtmosphereRenderRadius; }
			set { m_AtmosphereRenderRadius = value; }
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

		public void ResetComponentScales( )
		{
			//
			//	BetaM = 0.434 * c * PI * ( 4 * PI * PI ) * K / ( Lambda * Lambda )
			//	with
			//	c = ( 0.6544 * T - 0.6510 ) * 10e-16
			//	and T is the turbidity factor
			//	and K is almost equal to 0.67
			//
			double turbidity = 1;
			double c = ( 0.6544 * turbidity - 0.6510 ) * 10e-16;
			double k = 0.67;
			double iR2 = ( s_AirIndexOfRefraction * s_AirIndexOfRefraction ) - 1;
			double mieNum = c  * 4 * Math.PI * Math.PI * Math.PI * k;
			double rayleighNum = ( 8 * Math.PI * Math.PI * Math.PI * iR2 * iR2 ) / ( 3 * s_AirNumberDensity );

			for ( int component = 0; component < 3; ++component )
			{
				double wv = s_Wavelengths[ component ];
				double wv2 = wv * wv;
				double wv4 = wv2 * wv2;

				m_RayleighCoefficients[ component ] = ( float )( rayleighNum / wv4 );
				m_MieCoefficients[ component ] = ( float )( mieNum / wv2 );
			}

			//	Values taken from http://www-ljk.imag.fr/Publications/Basilic/com.lmc.publi.PUBLI_Article@11e7cdda2f7_f64b69/article.pdf
			m_RayleighCoefficients[ 0 ] = 5.8e-6f;
			m_RayleighCoefficients[ 1 ] = 13.5e-6f;
			m_RayleighCoefficients[ 2 ] = 33.1e-6f;

			//	Values taken from http://patarnott.com/atms749/pdf/RayleighScatteringForAtmos.pdf
		//	m_RayleighCoefficients[ 0 ] = 0.03539f;
		//	m_RayleighCoefficients[ 1 ] = 0.06613f;
		//	m_RayleighCoefficients[ 2 ] = 0.23522f;
		//	m_RayleighCoefficients[ 0 ] = 0.0000004f;
		//	m_RayleighCoefficients[ 1 ] = 0.0000006f;
		//	m_RayleighCoefficients[ 2 ] = 0.00000243f;
		}

		#region Private Members

		private readonly static double s_AirIndexOfRefraction = 1.0003;
		private static readonly double s_AirNumberDensity = 2.504e25;
		private readonly static double[] s_Wavelengths = new double[ 3 ] { 650e-9, 610e-9, 475e-9 };

		private Vector3 m_SunDirection = Vector3.ZAxis;
		private int m_Samples = 100;
		private float m_PlanetRenderRadius;
		private float m_AtmosphereRenderRadius;
		private float m_PlanetRadius = 6360000;
		private float m_AtmosphereRadius = 6420000;

		private float m_RayleighDensityScaleHeightFraction = 0.13f;
		private float m_MieDensityScaleHeightFraction = 0.05f;
		private readonly float[] m_RayleighCoefficients = new float[ 3 ];
		private readonly float[] m_MieCoefficients = new float[ 3 ];

		#endregion
	}
}
