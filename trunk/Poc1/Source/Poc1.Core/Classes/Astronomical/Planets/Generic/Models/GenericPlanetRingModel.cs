using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;

namespace Poc1.Core.Classes.Astronomical.Planets.Generic.Models
{
	/// <summary>
	/// Generic ring model
	/// </summary>
	/// <typeparam name="TPlanet">Planet type</typeparam>
	/// <typeparam name="TPlanetModel">Planet model type</typeparam>
	public class GenericPlanetRingModel<TPlanet, TPlanetModel> : GenericPlanetEnvironmentModel<TPlanet, TPlanetModel>, IPlanetRingModel
		where TPlanet : IPlanet
		where TPlanetModel : IPlanetModel
	{

		/// <summary>
		/// Gets/sets the width of the rings in metres
		/// </summary>
		public Units.Metres Width
		{
			get { return m_Width; }
			set
			{
				if ( m_Width != value )
				{
					m_Width = value;
					OnModelChanged( );
				}
			}
		}

		#region Private Members

		private Units.Metres m_Width;

		#endregion
	}
}
