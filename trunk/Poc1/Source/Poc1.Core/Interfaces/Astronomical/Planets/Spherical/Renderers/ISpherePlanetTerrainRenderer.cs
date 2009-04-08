using System.Drawing;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Spherical.Renderers
{
	/// <summary>
	/// Base interface for spherical planet terrain renderers. Provides support for planet marble texture builders
	/// </summary>
	public interface ISpherePlanetTerrainRenderer
	{
		/// <summary>
		/// Creates a face for the marble texture cube map
		/// </summary>
		Bitmap CreateMarbleTextureFace( CubeMapFace face, int width, int height );
	}
}
