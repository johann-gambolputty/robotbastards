
using Poc1.Universe.Interfaces.Planets.Models.Templates;

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
		IPlanetProcTerrainTemplate Template
		{
			get; set;
		}
	}
}
