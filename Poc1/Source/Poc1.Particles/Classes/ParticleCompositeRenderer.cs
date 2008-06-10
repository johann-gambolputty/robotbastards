using System.ComponentModel;
using Poc1.Particles.Interfaces;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Particles.Classes
{
	/// <summary>
	/// Composite Renderer
	/// </summary>
	[Browsable(false)]
	public class ParticleCompositeRenderer : ParticleSystemCompositeComponent<IParticleRenderer>, IParticleRenderer
	{
		#region IParticleRenderer Members

		/// <summary>
		/// Renders particles
		/// </summary>
		public void RenderParticles( IRenderContext context, IParticleSystem particleSystem )
		{
			foreach ( IParticleRenderer renderer in Components )
			{
				renderer.RenderParticles( context, particleSystem );
			}
		}

		#endregion
	}
}
