using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Particles.Interfaces
{
	/// <summary>
	/// Manages rendering and updates for a set of particle systems
	/// </summary>
	public interface IParticleSystemManager : IRenderable
	{
		/// <summary>
		/// Adds a particle system to the manager
		/// </summary>
		void Add( IParticleSystem ps );

		/// <summary>
		/// Removes a particle system to the manager
		/// </summary>
		void Remove( IParticleSystem ps );
	}
}
