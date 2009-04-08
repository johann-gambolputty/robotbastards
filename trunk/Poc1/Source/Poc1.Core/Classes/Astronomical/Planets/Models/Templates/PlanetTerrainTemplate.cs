using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;
using Rb.Core.Maths;

namespace Poc1.Core.Classes.Astronomical.Planets.Models.Templates
{
	/// <summary>
	/// Planet terrain template base class
	/// </summary>
	public class PlanetTerrainTemplate : PlanetEnvironmentModelTemplate, IPlanetTerrainTemplate
	{
		/// <summary>
		/// Creates an instance of this template
		/// </summary>
		public override void SetupInstance( IPlanetEnvironmentModel model, ModelTemplateInstanceContext context )
		{
			( ( IPlanetTerrainModel )model ).MaximumHeight = Range.Mid( MaximumHeightRange, context.Random.NextDouble( ) );
		}


		#region IPlanetTerrainTemplate Members

		/// <summary>
		/// Gets/sets the maximum height range for terrain
		/// </summary>
		public Range<Units.Metres> MaximumHeightRange
		{
			get { return m_MaximumHeightRange; }
			set
			{
				if ( m_MaximumHeightRange != value )
				{
					m_MaximumHeightRange = value;
					OnTemplateChanged( );
				}
			}
		}

		#endregion

		#region Private Members

		private Range<Units.Metres> m_MaximumHeightRange = new Range<Units.Metres>( new Units.Metres( 5000 ), new Units.Metres( 5000 ) );

		#endregion
	}
}
