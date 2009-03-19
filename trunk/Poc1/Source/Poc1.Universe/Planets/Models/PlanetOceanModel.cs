using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Models;

namespace Poc1.Universe.Planets.Models
{
	/// <summary>
	/// Planet ocean model implementation
	/// </summary>
	public class PlanetOceanModel : PlanetEnvironmentModel, IPlanetOceanModel
	{
		#region IPlanetOceanModel Members

		/// <summary>
		/// Gets/sets the sea level. If changed, the ModelChanged event is invoked.
		/// </summary>
		public Units.Metres SeaLevel
		{
			get { return m_SeaLevel; }
			set
			{
				if ( m_SeaLevel != value )
				{
					m_SeaLevel = value;
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
			planetModel.OceanModel = remove ? null : this;
		}

		#endregion

		#region Private Members

		private Units.Metres m_SeaLevel = new Units.Metres( 0 );

		#endregion
	}
}
