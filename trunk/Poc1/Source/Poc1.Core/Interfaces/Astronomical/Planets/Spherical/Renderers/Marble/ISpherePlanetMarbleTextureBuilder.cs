using System;
using Rb.Core.Threading;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Spherical.Renderers.Marble
{
	/// <summary>
	/// Builds textures for sphere planet marble renderers
	/// </summary>
	public interface ISpherePlanetMarbleTextureBuilder
	{
		/// <summary>
		/// Returns true if the marble texture needs to be rebuilt
		/// </summary>
		bool RequiresRebuild( ISpherePlanet planet );

		/// <summary>
		/// Adds the request to build a texture for the specified planet onto a build queue
		/// </summary>
		void QueueBuild( IWorkItemQueue queue, ISpherePlanet planet, Action<ITexture> onComplete );

		/// <summary>
		/// Builds a texture. Blocking call.
		/// </summary>
		ITexture Build( ISpherePlanet planet, IProgressMonitor progressMonitor );
	}
}
