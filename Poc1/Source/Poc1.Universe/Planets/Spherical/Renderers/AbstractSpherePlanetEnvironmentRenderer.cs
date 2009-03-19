
using Poc1.Universe.Interfaces.Planets.Spherical;
using Poc1.Universe.Planets.Renderers;

namespace Poc1.Universe.Planets.Spherical.Renderers
{
	/// <summary>
	/// Abstract class for spherical planet environment renderers
	/// </summary>
	public abstract class AbstractSpherePlanetEnvironmentRenderer : AbstractPlanetEnvironmentRenderer
	{
		/// <summary>
		/// Gets the sphere planet attached to the renderer that contains this object
		/// </summary>
		public ISpherePlanet SpherePlanet
		{
			get { return ( ISpherePlanet )Planet; }
		}
	}
}
