
using System.Collections.Generic;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces
{
	public interface IPlanet : IEntity, IRenderable
	{
		/// <summary>
		/// Gets the description of the orbit of this planet
		/// </summary>
		IOrbit Orbit
		{
			get;
		}

		/// <summary>
		/// Gets a list of moons that orbit this planet
		/// </summary>
		IList<IPlanet> Moons
		{
			get;
		}
	}
}
