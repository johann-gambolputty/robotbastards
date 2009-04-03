using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Classes.Astronomical.Planets.Models.Templates
{
	/// <summary>
	/// Template for planetary atmosphere model instances
	/// </summary>
	public class PlanetAtmosphereScatteringTemplate : PlanetAtmosphereTemplate, IPlanetAtmosphereScatteringTemplate
	{
		#region IPlanetAtmosphereScatteringTemplate Members

		/// <summary>
		/// Gets/sets the name of the atmosphere
		/// </summary>
		/// <remarks>
		/// The atmosphere name maps to the texture files used for in-game rendering, and the 
		/// data files used for editor build tasks.
		/// </remarks>
		public string AtmosphereName
		{
			get { return m_AtmosphereName; }
			set
			{
				if ( m_AtmosphereName != value )
				{
					m_AtmosphereName = value;
					OnTemplateChanged( );
				}
			}
		}

		/// <summary>
		/// Gets the scattering lookup texture data
		/// </summary>
		public ITexture3d ScatteringTexture
		{
			get { return m_ScatteringTexture; }
			set
			{
				if ( m_ScatteringTexture != value )
				{
					m_ScatteringTexture = value;
					OnTemplateChanged( );
				}
			}
		}

		/// <summary>
		/// Gets the optical depth lookup texture data
		/// </summary>
		public ITexture2d OpticalDepthTexture
		{
			get { return m_OpticalDepthTexture; }
			set
			{
				if ( m_OpticalDepthTexture != value )
				{
					m_OpticalDepthTexture = value;
					OnTemplateChanged( );
				}
			}
		}

		#endregion

		#region Private Members

		private ITexture3d m_ScatteringTexture;
		private ITexture2d m_OpticalDepthTexture;
		private string m_AtmosphereName;


		#endregion
	}
}
