using Poc1.Core.Interfaces.Astronomical.Planets.Models;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Renderers
{
	/// <summary>
	/// Factory interface for creating renderers from environment models
	/// </summary>
	public interface IPlanetEnvironmentRendererFactory
	{
		/// <summary>
		/// Creates a renderer for a model
		/// </summary>
		/// <param name="model">Source model</param>
		/// <returns>Returns a renderer for a model. Returns null if no renderer is associated with the model</returns>
		IPlanetEnvironmentRenderer CreateModelRenderer( IPlanetEnvironmentModel model );
	}
}
