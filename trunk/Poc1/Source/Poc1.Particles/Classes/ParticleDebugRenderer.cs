
using Poc1.Particles.Interfaces;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Particles.Classes
{
	public class ParticleDebugRenderer : IParticleRenderer
	{
		#region IParticleRenderer Members

		public void RenderParticles( IRenderContext context, IParticleSystem particleSystem, System.Collections.IEnumerable particles )
		{
			Graphics.Draw.Sphere( Graphics.Surfaces.Red, particleSystem.Frame.Translation, 0.1f );
		}

		#endregion
	}
}
