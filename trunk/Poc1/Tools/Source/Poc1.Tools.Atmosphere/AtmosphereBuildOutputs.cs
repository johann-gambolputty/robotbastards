using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Tools.Atmosphere
{
	/// <summary>
	/// Outputs from the atmosphere build process
	/// </summary>
	public class AtmosphereBuildOutputs
	{
		/// <summary>
		/// Sets up the build outputs
		/// </summary>
		/// <param name="scatteringTexture">Scattering texture data</param>
		public AtmosphereBuildOutputs( Texture3dData scatteringTexture, Texture2dData opticalDepthTexture )
		{
			m_ScatteringTexture = scatteringTexture;
			m_OpticalDepthTexture = opticalDepthTexture;
		}

		/// <summary>
		/// Gets the scattering lookup texture data
		/// </summary>
		public Texture3dData ScatteringTexture
		{
			get { return m_ScatteringTexture; }
		}

		/// <summary>
		/// Gets the optical depth lookup texture data
		/// </summary>
		public Texture2dData OpticalDepthTexture
		{
			get { return m_OpticalDepthTexture; }
		}

		#region Private Members

		private readonly Texture3dData m_ScatteringTexture;
		private readonly Texture2dData m_OpticalDepthTexture; 

		#endregion
	}
}
