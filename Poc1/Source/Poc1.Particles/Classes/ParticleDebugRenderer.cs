using System.Drawing;
using Poc1.Particles.Interfaces;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.Particles.Classes
{
	public class ParticleDebugRenderer : IParticleRenderer
	{
		/// <summary>
		/// Gets/sets the rendered size of the particles 
		/// </summary>
		public float ParticleSize
		{
			get { return m_ParticleSize; }
			set { m_ParticleSize = value; }
		}

		#region IParticleRenderer Members

		/// <summary>
		/// Renders a collection of particles
		/// </summary>
		public void RenderParticles( IRenderContext context, IParticleSystem particleSystem )
		{
			ISerialParticleBuffer sBuffer = ( ISerialParticleBuffer )particleSystem.Buffer;
			SerialParticleFieldIterator posIter = new SerialParticleFieldIterator( sBuffer, ParticleBase.Position );

			for ( int particleIndex = 0; particleIndex < sBuffer.NumActiveParticles; ++particleIndex )
			{
				posIter.MoveNext( );
				Graphics.Draw.Billboard( s_ParticleBrush, posIter.Point3Value, ParticleSize, ParticleSize );
			}
		}

		#endregion

		#region IParticleSystemComponent Members

		/// <summary>
		/// Called when this component is attached to a particle system
		/// </summary>
		public void Attach( IParticleSystem particles )
		{
			particles.Buffer.AddField( ParticleBase.Position, ParticleFieldType.Float32, 3, 0.0f );
		}

		#endregion

		#region Private Members

		private float m_ParticleSize = 0.1f;

		private readonly static DrawBase.IBrush s_ParticleBrush;

		static ParticleDebugRenderer( )
		{
			s_ParticleBrush = Graphics.Draw.NewBrush( Color.Firebrick );
			s_ParticleBrush.State.Blend = true;
			s_ParticleBrush.State.SourceBlend = BlendFactor.One;
			s_ParticleBrush.State.DestinationBlend = BlendFactor.One;
			s_ParticleBrush.State.DepthTest = true;
			s_ParticleBrush.State.DepthWrite = true;
		}

		#endregion

	}
}
