
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Renderers.PackTextures
{
	/// <summary>
	/// Terrain pack texture provider
	/// </summary>
	public interface IPackTextureProvider
	{
		/// <summary>
		/// Gets the terrain pack texture
		/// </summary>
		ITexture PackTexture
		{
			get;
		}

		/// <summary>
		/// Gets the pack lookup texture
		/// </summary>
		ITexture LookupTexture
		{
			get;
		}
	}
}
