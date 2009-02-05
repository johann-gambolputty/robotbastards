using System;
using Poc1.Universe.Interfaces;

namespace Poc1.Bob.Core.Interfaces.Planets
{
	/// <summary>
	/// View for sphere planet model templates and instances
	/// </summary>
	public interface ISpherePlanetModelTemplateView : IPlanetModelTemplateView
	{
		/// <summary>
		/// Event raised when the model radius slider is moved
		/// </summary>
		event Action<Units.Metres> ModelRadiusChanged;
	}
}
