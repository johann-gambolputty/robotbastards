using Poc1.Particles.Interfaces;

namespace Poc1.Particles.Classes
{
	class SimplePhysicsUpdater : IParticleUpdater
	{
		/// <summary>
		/// Bodgy friction coefficient (0 = no friction/resistance, 1 = particles stop immediately)
		/// </summary>
		public float Friction
		{
			get { return m_Friction; }
			set { m_Friction = value; }
		}

		#region IParticleUpdater Members

		/// <summary>
		/// Updates all particles
		/// </summary>
		public void Update( IParticleSystem ps )
		{
			ISerialParticleBuffer sBuffer = ( ISerialParticleBuffer )ps.Buffer;
			SerialParticleFieldIterator posIter = new SerialParticleFieldIterator( sBuffer, ParticleBase.Position );
			SerialParticleFieldIterator velIter = new SerialParticleFieldIterator( sBuffer, ParticleBase.Velocity );

			float fr = 1.0f - Friction;
			for ( int particleIndex = 0; particleIndex < sBuffer.NumActiveParticles; ++particleIndex )
			{
				posIter.MoveNext( );
				velIter.MoveNext( );

				posIter.Point3Value += velIter.Vector3Value;
				velIter.Vector3Value *= fr;
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
			particles.Buffer.AddField( ParticleBase.Velocity, ParticleFieldType.Float32, 3, 0.0f );
		}

		#endregion

		#region Private Members

		private float m_Friction = 0.5f;

		#endregion
	}
}
