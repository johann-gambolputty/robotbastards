using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Models;
using Rb.Core.Maths;

namespace Poc1.Universe.Planets.Models
{
	/// <summary>
	/// Base class for planetary cloud models
	/// </summary>
	public class PlanetCloudModel : PlanetEnvironmentModel, IPlanetCloudModel
	{
		#region IPlanetCloudModel Members

		/// <summary>
		/// Gets/sets the cloud coverage range
		/// </summary>
		public Range<float> CoverageRange
		{
			get { return m_CoverageRange; }
			set
			{
				if ( m_CoverageRange != value )
				{
					m_CoverageRange = value;
					OnModelChanged( );
				}
			}
		}

		/// <summary>
		/// Gets/sets the minimum height of the cloud layer
		/// </summary>
		public Units.Metres CloudLayerMinHeight
		{
			get { return m_CloudLayerMinHeight; }
			set
			{
				if ( m_CloudLayerMinHeight != value )
				{
					m_CloudLayerMinHeight = value;
					OnModelChanged( );
				}
			}
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Assigns this model to a planet model
		/// </summary>
		protected override void AssignToModel( IPlanetModel planetModel, bool remove )
		{
			planetModel.CloudModel = remove ? null : this;
		}

		#endregion


		#region Private Members

		private Range<float> m_CoverageRange = new Range<float>( 0, 1 );
		private Units.Metres m_CloudLayerMinHeight = new Units.Metres( 7000 );

		#endregion

	}
}
