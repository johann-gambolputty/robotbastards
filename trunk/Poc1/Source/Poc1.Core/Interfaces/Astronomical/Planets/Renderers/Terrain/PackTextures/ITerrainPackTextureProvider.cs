
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Renderers.Terrain.PackTextures
{
	/// <summary>
	/// Terrain pack texture provider
	/// </summary>
	public interface ITerrainPackTextureProvider
	{
		/// <summary>
		/// Gets the terrain pack texture
		/// </summary>
		ITexture2d PackTexture
		{
			get;
		}

		/// <summary>
		/// Gets the pack lookup texture
		/// </summary>
		ITexture2d LookupTexture
		{
			get;
		}
	}
}
