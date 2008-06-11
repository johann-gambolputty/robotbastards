using Poc1.Particles.Interfaces;
using Rb.Core.Maths;

namespace Poc1.Particles.Classes
{
	public class SimplePhysicsUpdater : IParticleUpdater
	{
		/// <summary>
		/// Bodgy friction coefficient (0 = no friction/resistance, 1 = particles stop immediately)
		/// </summary>
		public float Friction
		{
			get { return m_Friction; }
			set { m_Friction = value; }
		}

		/// <summary>
		/// Gets the gravity coefficient (0=no gravity, 1=standard gravity). Value is magnitude of acceleration vector (m/s-2)
		/// </summary>
		public float Gravity
		{
			get { return m_Gravity; }
			set { m_Gravity = value; }
		}

		#region IParticleUpdater Members

		/// <summary>
		/// Updates all particles
		/// </summary>
		public void Update( IParticleSystem ps, float updateTime )
		{
			ISerialParticleBuffer sBuffer = ( ISerialParticleBuffer )ps.Buffer;
			SerialParticleFieldIterator posIter = new SerialParticleFieldIterator( sBuffer, ParticleBase.Position );
			SerialParticleFieldIterator velIter = new SerialParticleFieldIterator( sBuffer, ParticleBase.Velocity );

			Vector3 g = Vector3.YAxis * -Gravity * updateTime;

			float fr = 1.0f - Friction;
			for ( int particleIndex = 0; particleIndex < sBuffer.NumActiveParticles; ++particleIndex )
			{
				posIter.MoveNext( );
				velIter.MoveNext( );

				posIter.Point3Value += velIter.Vector3Value;
				posIter.Point3Value += g;
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

		private float m_Friction = 0.2f;
		private float m_Gravity = 0.0f;

		#endregion
	}
}
