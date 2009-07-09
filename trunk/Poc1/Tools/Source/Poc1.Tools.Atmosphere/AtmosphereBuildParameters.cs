namespace Poc1.Tools.Atmosphere
{
	/// <summary>
	/// Atmospheric builder parameters
	/// </summary>
	public class AtmosphereBuildParameters
	{
		/// <summary>
		/// Gets/sets the number of samples to take when integrating the density function
		/// </summary>
		public int AttenuationSamples
		{
			get { return m_AttenuationSamples; }
			set { m_AttenuationSamples = value; }
		}

		/// <summary>
		/// Gets/sets the number of height samples to take when building the optical depth lookup table
		/// </summary>
		public int HeightSamples
		{
			get { return m_HeightSamples; }
			set { m_HeightSamples = value; }
		}

		/// <summary>
		/// Gets/sets the number of sun angle samples to take when building the optical depth lookup table
		/// </summary>
		public int SunAngleSamples
		{
			get { return m_SunAngleSamples; }
			set { m_SunAngleSamples = value; }
		}

		/// <summary>
		/// Gets/sets the number of view angle samples to take when building the optical depth lookup table
		/// </summary>
		public int ViewAngleSamples
		{
			get { return m_ViewAngleSamples; }
			set { m_ViewAngleSamples = value; }
		}

		/// <summary>
		/// Gets/sets the resolution of the optical depth texture
		/// </summary>
		public int OpticalDepthResolution
		{
			get { return m_OpticalDepthResolution; }
			set { m_OpticalDepthResolution = value; }
		}

		#region Private Members

		private int m_AttenuationSamples		= 10;
		private int m_HeightSamples				= 32;
		private int m_SunAngleSamples			= 32;
		private int m_ViewAngleSamples			= 32;
		private int m_OpticalDepthResolution	= 128;

		#endregion

	}
}
