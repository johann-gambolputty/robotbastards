using System;
using System.Collections.Generic;
using System.Text;

namespace Poc1.Core.Interfaces.Astronomical.Planets
{
	/// <summary>
	/// Planet interface
	/// </summary>
	public interface IPlanet : IAstronomicalBody
	{
		/// <summary>
		/// Gets the planet model
		/// </summary>
		IPlanetModel Model
		{
			get;
		}

		/// <summary>
		/// Gets the planet renderer
		/// </summary>
		IPlanetRenderer Renderer
		{
			get;
		}
	}
}
