using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;
using Rb.Core.Maths;

namespace Poc1.Core.Classes.Astronomical.Planets.Models.Templates
{
	/// <summary>
	/// Planet atmosphere template
	/// </summary>
	public class PlanetAtmosphereTemplate : PlanetEnvironmentModelTemplate, IPlanetAtmosphereTemplate
	{
		/// <summary>
		/// Gets/sets the atmospheric thickness range
		/// </summary>
		/// <remarks>
		/// Thickness is the height above the planet surface that the atmosphere terminates
		/// </remarks>
		public Range<Units.Metres> Thickness
		{
			get { return m_Thickness; }
			set { m_Thickness = value; }
		}

		/// <summary>
		/// Atmosphere model creation
		/// </summary>
		public override void SetupInstance( IPlanetEnvironmentModel model, ModelTemplateInstanceContext context )
		{
			IPlanetAtmosphereScatteringModel atmosphereModel = ( IPlanetAtmosphereScatteringModel )model;
			atmosphereModel.Thickness = Range.Mid( m_Thickness, ( float )context.Random.NextDouble( ) );
			//	IPlanetAtmosphereModel atmosphereModel = ( IPlanetAtmosphereModel )model;
		}

		#region Private Members

		private Range<Units.Metres> m_Thickness = new Range<Units.Metres>( new Units.Metres( 3773.58f ), new Units.Metres( 3773.58f ) );
	//	private Range<Units.Metres> m_Thickness = new Range<Units.Metres>( new Units.Metres( 20000 ), new Units.Metres( 20000 ) );
	//	private Range<Units.Metres> m_Thickness = new Range<Units.Metres>( new Units.Metres( 60000 ), new Units.Metres( 60000 ) );

		#endregion
	}
}
