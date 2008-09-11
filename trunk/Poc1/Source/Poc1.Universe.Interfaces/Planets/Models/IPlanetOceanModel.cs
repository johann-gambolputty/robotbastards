using System;
using System.Collections.Generic;
using System.Text;

namespace Poc1.Universe.Interfaces.Planets.Models
{
	/// <summary>
	/// Models a planet's oceans
	/// </summary>
	public interface IPlanetOceanModel
	{
		/// <summary>
		/// Event, invoked when the model changes
		/// </summary>
		event EventHandler ModelChanged;

		/// <summary>
		/// Gets/sets the sea level. If changed, the ModelChanged event is invoked.
		/// </summary>
		Units.Metres SeaLevel
		{
			get; set;
		}
	}
}
