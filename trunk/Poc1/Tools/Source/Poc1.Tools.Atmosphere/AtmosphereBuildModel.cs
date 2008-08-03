
namespace Poc1.Tools.Atmosphere
{
	/// <summary>
	/// Atmosphere model, passed to <see cref="AtmosphereBuilder"/>
	/// </summary>
	public class AtmosphereBuildModel
	{

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
		public float InnerRadius
		{
			get { return m_InnerRadius; }
			set { m_InnerRadius = value; }
		}

		/// <summary>
		/// Gets/sets the outer radius (planet + atmosphere radius)
		/// </summary>
		public float OuterRadius
		{
			get { return m_OuterRadius; }
			set { m_OuterRadius = value; }
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

		#region Private Members

		private float m_OutscatterFudgeFactor = 0.2f;
		private float m_InscatterDistanceFudgeFactor = 10;
		private float m_OutscatterDistanceFudgeFactor = 0.3f;
		private float m_MieFudge = 0.0f;
		private float m_RayleighFudge = 1.0f;
		private float m_MieDensityScaleHeightFraction = 0.9f;
		private float m_RayleighDensityScaleHeightFraction = 0.9f;
		private readonly float[] m_SunIntensity = new float[ 3 ] { 1, 1, 1 };
		private float m_InnerRadius = 80000;
		private float m_OuterRadius = 86000;

		#endregion

	}
}
