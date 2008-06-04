using System.Collections.Generic;
using System.Drawing;
using Poc1.Particles.Interfaces;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.Particles.Classes
{
	public class ParticleDebugRenderer : IParticleRenderer
	{
		#region IParticleRenderer Members

		/// <summary>
		/// Renders a collection of particles
		/// </summary>
		public void RenderParticles( IRenderContext context, IParticleSystem particleSystem )
		{
			Graphics.Draw.Sphere( Graphics.Surfaces.Red, particleSystem.Frame.Translation, ParticleSize );

			ParticleFieldIterator posIter = new ParticleFieldIterator( particleSystem, ParticleBase.Position );

			for ( int particleIndex = 0; particleIndex < particleSystem.NumParticles; ++particleIndex )
			{
				Graphics.Draw.Billboard( ms_ParticleBrush, posIter.GetPosition( ), ParticleSize, ParticleSize );
			}
		}

		#endregion

		#region Private Members

		private const float ParticleSize = 0.1f;

		private readonly static DrawBase.IBrush ms_ParticleBrush;

		static ParticleDebugRenderer( )
		{
			ms_ParticleBrush = Graphics.Draw.NewBrush( Color.Aqua );
			ms_ParticleBrush.State.Blend = true;
			ms_ParticleBrush.State.SourceBlend = BlendFactor.One;
			ms_ParticleBrush.State.DestinationBlend = BlendFactor.One;
		}

		#endregion
	}
}
