using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Particles.Interfaces
{
	/// <summary>
	/// Particle rendering interface
	/// </summary>
	public interface IParticleRenderer
	{
		/// <summary>
		/// Renders a collection of particles
		/// </summary>
		void RenderParticles( IRenderContext context, IParticleSystem particleSystem, System.Collections.IEnumerable particles );
	}
}
