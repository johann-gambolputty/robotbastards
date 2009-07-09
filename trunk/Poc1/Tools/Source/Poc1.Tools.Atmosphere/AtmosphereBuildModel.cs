
using System;

namespace Poc1.Tools.Atmosphere
{
	/// <summary>
	/// Atmosphere model, passed to <see cref="AtmosphereBuilder"/>
	/// </summary>
	public class AtmosphereBuildModel
	{
		public AtmosphereBuildModel( )
		{
			//	Older rayleigh scattering coefficient values taken from http://patarnott.com/atms749/pdf/RayleighScatteringForAtmos.pdf
			//	Current rayleigh scattering coefficient values taken from http://www-ljk.imag.fr/Publications/Basilic/com.lmc.publi.PUBLI_Article@11e7cdda2f7_f64b69/article.pdf
		//	RayleighCoefficients[ 0 ] = 5.8e-6f;
		//	RayleighCoefficients[ 1 ] = 13.5e-6f;
		//	RayleighCoefficients[ 2 ] = 33.1e-6f;
			RayleighCoefficients[ 0 ] = 5.8e-3f;
			RayleighCoefficients[ 1 ] = 13.5e-3f;
			RayleighCoefficients[ 2 ] = 33.1e-3f;

			//	Pre-calculated
		//	MieCoefficients[ 0 ] = 0.0006687082f;
		//	MieCoefficients[ 1 ] = 0.000759282964f;
		//	MieCoefficients[ 2 ] = 0.001252207f;
			MieCoefficients[ 0 ] = 0.0004f;
			MieCoefficients[ 1 ] = 0.0004f;
			MieCoefficients[ 2 ] = 0.0004f;
		}

		/// <summary>
		/// Gets/sets the scale height fraction for mie scattering
		/// </summary>
		public float MieDensityScaleHeightFraction
		{
			get { return m_MieDensityScaleHeightFraction; }
			set { m_MieDensityScaleHeightFraction = value; }
		}

		/// <summary>
		/// Gets/sets the scale height fraction for rayleigh scattering
		/// </summary>
		public float RayleighDensityScaleHeightFraction
		{
			get { return m_RayleighDensityScaleHeightFraction; }
			set { m_RayleighDensityScaleHeightFraction = value; }
		}

		/// <summary>
		/// Gets the sun intensity (RGB)
		/// </summary>
		public float[] SunIntensity
		{
			get { return m_SunIntensity; }
		}

		/// <summary>
		/// Gets/sets the inner radius (planet radius)
		/// </summary>
		public float InnerRadiusMetres
		{
			get { return m_InnerRadius; }
			set { m_InnerRadius = value; }
		}

		/// <summary>
		/// Gets/sets the outer radius (planet + atmosphere radius)
		/// </summary>
		public float AtmosphereThicknessMetres
		{
			get { return m_Thickness; }
			set { m_Thickness = value; }
		}

		/// <summary>
		/// Gets/sets the Mie scattering fudge factor
		/// </summary>
		public float MieFudgeFactor
		{
			get { return m_MieFudge; }
			set { m_MieFudge = value; }
		}

		/// <summary>
		/// Gets/sets the Rayleigh scattering fudge factor
		/// </summary>
		public float RayleighFudgeFactor
		{
			get { return m_RayleighFudge; }
			set { m_RayleighFudge = value; }
		}

		/// <summary>
		/// Gets/sets the fudge factor applied to each density sample when integrating outscatter
		/// </summary>
		public float OutscatterDistanceFudgeFactor
		{
			get { return m_OutscatterDistanceFudgeFactor; }
			set { m_OutscatterDistanceFudgeFactor = value; }
		}

		/// <summary>
		/// Gets/sets the fudge factor applied to each density sample when integrating inscatter
		/// </summary>
		public float InscatterDistanceFudgeFactor
		{
			get { return m_InscatterDistanceFudgeFactor; }
			set { m_InscatterDistanceFudgeFactor = value; }
		}

		/// <summary>
		/// Gets/sets the fudge factor applied to the outscatter (exp(-ts-tv))
		/// </summary>
		public float OutscatterFudgeFactor
		{
			get { return m_OutscatterFudgeFactor; }
			set { m_OutscatterFudgeFactor = value; }
		}

		/// <summary>
		/// Gets/sets the ground radius multiplier
		/// </summary>
		public float GroundRadiusMultiplier
		{
			get { return m_GroundRadiusMultiplier; }
			set { m_GroundRadiusMultiplier = value; }
		}

		/// <summary>
		/// Gets the rayleigh scattering coefficient array
		/// </summary>
		public float[] RayleighCoefficients
		{
			get { return m_RayleighCoefficients; }
		}

		/// <summary>
		/// Gets the mie scattering coefficient array
		/// </summary>
		public float[] MieCoefficients
		{
			get { return m_MieCoefficients; }
		}

		/// <summary>
		/// Sets up rayleigh and mie scattering coefficients
		/// </summary>
		public void SetScatteringCoefficients( double airIndexOfRefraction, double airNumberDensity )
		{
			//
			//	BetaM = 0.434 * c * PI * ( 4 * PI * PI ) * K / ( Lambda * Lambda )
			//	with
			//	c = ( 0.6544 * T - 0.6510 ) * 10e-16
			//	and T is the turbidity factor
			//	and K ~= 0.67
			//
			double turbidity = 1;
			double c = ( 0.6544 * turbidity - 0.6510 ) * 10e-16;
			double k = 0.67;
			double iR2 = ( airIndexOfRefraction * airIndexOfRefraction ) - 1;
			double mieNum = c * 4 * Math.PI * Math.PI * Math.PI * k;
			double rayleighNum = ( 8 * Math.PI * Math.PI * Math.PI * iR2 * iR2 ) / ( 3 * airNumberDensity );

			for ( int component = 0; component < 3; ++component )
			{
				double wv = s_Wavelengths[ component ];
				double wv2 = wv * wv;
				double wv4 = wv2 * wv2;

				m_RayleighCoefficients[ component ] = ( float )( rayleighNum / wv4 );
				m_MieCoefficients[ component ] = ( float )( mieNum / wv2 );
			}
		}
		#region Private Members

		private float m_OutscatterFudgeFactor = 1.0f;
		private float m_InscatterDistanceFudgeFactor = 1.0f;
		private float m_OutscatterDistanceFudgeFactor = 1.0f;
		private float m_MieFudge = 1.0f;
		private float m_RayleighFudge = 1.0f;
		private float m_MieDensityScaleHeightFraction = 0.02f;
		private float m_RayleighDensityScaleHeightFraction = 0.13f;
		private readonly float[] m_SunIntensity = new float[ 3 ] { 1, 1, 1 };
	//	private float m_InnerRadius = 6360000;
	//	private float m_Thickness = 60000;
		private float m_InnerRadius = 6360;
		private float m_Thickness = 60;
		private float m_GroundRadiusMultiplier = 1.0f;
		private readonly float[] m_RayleighCoefficients = new float[ 3 ];
		private readonly float[] m_MieCoefficients = new float[ 3 ];

		//private readonly static double s_DefaultAirIndexOfRefraction = 1.0003;
		//private static readonly double s_DefaultAirNumberDensity = 2.504e25;
		private readonly static double[] s_Wavelengths = new double[ 3 ] { 650e-9, 610e-9, 475e-9 };


		#endregion

	}
}
