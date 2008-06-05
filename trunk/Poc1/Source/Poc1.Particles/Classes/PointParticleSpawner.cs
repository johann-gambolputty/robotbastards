using Poc1.Particles.Interfaces;

namespace Poc1.Particles.Classes
{
	/// <summary>
	/// Spawns particles at a single point
	/// </summary>
	public class PointParticleSpawner : IParticleSpawner
	{
		#region IParticleSpawner Members

		/// <summary>
		/// Spawns a given number of particles
		/// </summary>
		public unsafe void SpawnParticles( IParticleSystem particles, int count )
		{
			ISerialParticleBuffer sBuffer = ( ISerialParticleBuffer )particles.Buffer;
			for ( int i = 0; i < count; ++i )
			{
				int particleIndex = sBuffer.AddParticle( );
				float* pos = ( float* )sBuffer.GetField( ParticleBase.Position, particleIndex );
				pos[ 0 ] = particles.Frame.Translation.X;
				pos[ 1 ] = particles.Frame.Translation.Y;
				pos[ 2 ] = particles.Frame.Translation.Z;
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
	}
}
