using System;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces.Planets.Spherical.Renderers
{
	/// <summary>
	/// Builds textures for sphere planet marble renderers
	/// </summary>
	public interface ISpherePlanetMarbleTextureBuilder
	{
		/// <summary>
		/// Adds the request to build a texture for the specified planet onto a build queue
		/// </summary>
		void QueueBuild( ISpherePlanet planet, Action<ITexture> onComplete );

		/// <summary>
		/// Builds a texture. Blocking call.
		/// </summary>
		ITexture Build( ISpherePlanet planet );
	}
}
