
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;

namespace Poc1.Bob.Core.Interfaces.Planets.Terrain
{
	/// <summary>
	/// Homogenous procedural terrain view control
	/// </summary>
	public interface IHomogenousProcTerrainView
	{
		/// <summary>
		/// Gets/sets the model displayed by this view
		/// </summary>
		IPlanetHomogenousProceduralTerrainTemplate Template
		{
			get; set;
		}
	}
}
