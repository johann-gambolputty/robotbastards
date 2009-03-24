using Poc1.Core.Classes.Astronomical.Planets.Models.Templates;
using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;
using Rb.Core.Maths;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical.Models.Templates
{
	/// <summary>
	/// Sphere planet ocean model template
	/// </summary>
	public class SpherePlanetOceanTemplate : PlanetEnvironmentModelTemplate, IPlanetOceanTemplate
	{
		#region IPlanetOceanModelTemplate Members

		/// <summary>
		/// Gets/sets the range of sea levels
		/// </summary>
		public Range<Units.Metres> SeaLevel
		{
			get { return m_SeaLevel; }
			set { m_SeaLevel = value; }
		}

		#endregion

		/// <summary>
		/// Sets up an ocean model instance
		/// </summary>
		public override void SetupInstance( IPlanetEnvironmentModel model, ModelTemplateInstanceContext context )
		{
			IPlanetOceanModel oceanModel = ( IPlanetOceanModel )model;
			oceanModel.SeaLevel = Range.Mid( SeaLevel, ( float )context.Random.NextDouble( ) );
		}

		#region Private Members

		private Range<Units.Metres> m_SeaLevel = new Range<Units.Metres>( new Units.Metres( 0 ), new Units.Metres( 10000 ) );

		#endregion
	}
}
