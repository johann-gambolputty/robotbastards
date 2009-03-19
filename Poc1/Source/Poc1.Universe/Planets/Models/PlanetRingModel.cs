using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Models;

namespace Poc1.Universe.Planets.Models
{
	/// <summary>
	/// Base class for planetary ring models
	/// </summary>
	public class PlanetRingModel : PlanetEnvironmentModel, IPlanetRingModel
	{
		#region IPlanetRingModel Members

		/// <summary>
		/// Gets/sets the width of the rings
		/// </summary>
		public Units.Metres Width
		{
			get { return m_RingWidth; }
			set
			{
				bool widthChanged = ( m_RingWidth.Value != value.Value );
				if ( widthChanged )
				{
					m_RingWidth = value;
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
			planetModel.RingModel = remove ? null : this;
		}

		#endregion

		#region Private Members

		private Units.Metres m_RingWidth = new Units.Metres( 8000 );

		#endregion
	}
}
