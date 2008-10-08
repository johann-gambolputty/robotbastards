using System.Drawing;
using Poc1.Universe.Interfaces.Planets.Models;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces.Planets.Spherical.Models
{
	/// <summary>
	/// Sphere planet terrain model interface
	/// </summary>
	public interface ISpherePlanetTerrainModel : IPlanetTerrainModel
	{
		/// <summary>
		/// Creates a face for the marble texture cube map
		/// </summary>
		Bitmap CreateMarbleTextureFace( CubeMapFace face, int resolution );
	}
}
