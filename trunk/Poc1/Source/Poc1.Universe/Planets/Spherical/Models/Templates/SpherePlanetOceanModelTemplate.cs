using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Models.Templates;
using Poc1.Universe.Planets.Models.Templates;
using Rb.Core.Maths;

namespace Poc1.Universe.Planets.Spherical.Models.Templates
{
	/// <summary>
	/// Sphere planet ocean model template
	/// </summary>
	public class SpherePlanetOceanModelTemplate : PlanetEnvironmentModelTemplate, IPlanetOceanModelTemplate
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

		private Range<Units.Metres> m_SeaLevel = new Range<Units.Metres>( new Units.Metres( 1000 ), new Units.Metres( 1000 ) );

		#endregion
	}
}
