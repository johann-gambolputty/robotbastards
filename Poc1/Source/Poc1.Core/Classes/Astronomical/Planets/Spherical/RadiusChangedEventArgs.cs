
using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical
{
	/// <summary>
	/// Event arguments denoting a change in radius in an <see cref="ISpherePlanetModel"/>
	/// </summary>
	public class RadiusChangedEventArgs : ModelChangedEventArgs
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="oldRadius">Old radius</param>
		/// <param name="newRadius">New radius</param>
		public RadiusChangedEventArgs( Units.Metres oldRadius, Units.Metres newRadius )
		{
			m_OldRadius = oldRadius;
			m_NewRadius = newRadius;
		}

		/// <summary>
		/// Gets the old radius
		/// </summary>
		public Units.Metres OldRadius
		{
			get { return m_OldRadius; }
		}

		/// <summary>
		/// Gets the new radius
		/// </summary>
		public Units.Metres NewRadius
		{
			get { return m_NewRadius; }
		}

		#region Private Members

		private readonly Units.Metres m_OldRadius;
		private readonly Units.Metres m_NewRadius;

		#endregion
	}

}
