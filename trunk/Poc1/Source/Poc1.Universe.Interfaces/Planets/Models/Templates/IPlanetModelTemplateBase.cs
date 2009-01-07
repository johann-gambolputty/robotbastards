using System;

namespace Poc1.Universe.Interfaces.Planets.Models.Templates
{
	/// <summary>
	/// Shared interface for all planet model templates
	/// </summary>
	public interface IPlanetModelTemplateBase
	{
		/// <summary>
		/// Event, raised when the template changes
		/// </summary>
		event EventHandler TemplateChanged;
	}
}
