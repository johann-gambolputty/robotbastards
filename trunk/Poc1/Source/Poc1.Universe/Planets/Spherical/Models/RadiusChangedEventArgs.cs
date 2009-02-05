using System;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Spherical.Models;

namespace Poc1.Universe.Planets.Spherical.Models
{
	/// <summary>
	/// Event object passed to <see cref="IPlanetModel.ModelChanged"/> when <see cref="ISpherePlanetModel.Radius"/> changes
	/// </summary>
	public class RadiusChangedEventArgs : EventArgs
	{
	}
}
