using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Models;

namespace Poc1.Universe.Planets.Models
{
	/// <summary>
	/// Base class for planetary cloud models
	/// </summary>
	public class PlanetCloudModel : IPlanetCloudModel
	{
		#region IPlanetEnvironmentModel Members

		/// <summary>
		/// Event, invoked when the model changes
		/// </summary>
		public event System.EventHandler ModelChanged;

		/// <summary>
		/// Gets/sets the associated planet
		/// </summary>
		public IPlanet Planet
		{
			get { return m_Planet; }
			set { m_Planet = value; }
		}

		#endregion

		#region Private Members

		private IPlanet m_Planet;

		#endregion
	}
}
