using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;

namespace Poc1.Core.Classes.Astronomical.Planets.Models.Templates
{
	/// <summary>
	/// Template for planetary atmosphere model instances
	/// </summary>
	public class PlanetAtmosphereScatteringTemplate : PlanetAtmosphereTemplate, IPlanetAtmosphereScatteringTemplate
	{
		#region IPlanetAtmosphereScatteringTemplate Members

		/// <summary>
		/// Gets/sets the names of the atmospheres used by this template
		/// </summary>
		/// <remarks>
		/// The atmosphere name maps to the texture files used for in-game rendering, and the 
		/// data files used for editor build tasks.
		/// </remarks>
		public string[] AtmosphereNames
		{
			get { return m_AtmosphereNames; }
			set
			{
				if ( m_AtmosphereNames != value )
				{
					m_AtmosphereNames = value;
					OnTemplateChanged( );
				}
			}
		}

		#endregion

		#region Private Members

		private string[] m_AtmosphereNames;

		#endregion
	}
}
