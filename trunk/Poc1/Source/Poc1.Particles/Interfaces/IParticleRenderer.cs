using System.Collections.Generic;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Particles.Interfaces
{
	/// <summary>
	/// Particle rendering interface
	/// </summary>
	public interface IParticleRenderer
	{
		/// <summary>
		/// Renders a particle system
		/// </summary>
		void RenderParticles( IRenderContext context, IParticleSystem particleSystem );
	}
}
