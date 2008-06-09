using System;
using Poc1.Particles.Interfaces;
using Rb.Core.Maths;

namespace Poc1.Particles.Classes
{
	/// <summary>
	/// Spawns particles with random velocities distributed in a cone
	/// </summary>
	public class BlunderbussParticleSpawner : IParticleSpawner
	{
		/// <summary>
		/// Gets/sets the normalized direction vector that particles can be spawned around.
		/// </summary>
		public Vector3 Direction
		{
			get { return m_Direction; }
			set { m_Direction = value.MakeNormal( ); }
		}

		/// <summary>
		/// Gets/sets the angle (in degrees) around the direction vector that particles can be spawned in.
		/// </summary>
		public float Angle
		{
			get { return m_Angle; }
			set { m_Angle = value; }
		}

		//	TODO: AP: Add function to determine the speed

		public float MinSpeed
		{
			get { return m_MinSpeed; }
			set { m_MinSpeed = value; }
		}

		public float MaxSpeed
		{
			get { return m_MaxSpeed; }
			set { m_MaxSpeed = value; }
		}


		#region IParticleSpawner Members

		/// <summary>
		/// Spawns a given number of particles
		/// </summary>
		public unsafe void SpawnParticles( IParticleSystem particles, int count )
		{
			float s, t;
			SphericalCoordinates.FromNormalizedVector( Direction, out s, out t );

			float sAngle = ( float )( m_Rnd.NextDouble( ) * Angle ) - Angle / 2;
			float tAngle = ( float )( m_Rnd.NextDouble( ) * Angle ) - Angle / 2;

			s = Utils.Wrap( s + sAngle, 0, Constants.TwoPi );
			t = Utils.Wrap( t + tAngle, 0, Constants.Pi );

			Vector3 direction = SphericalCoordinates.ToVector( s, t );

			ISerialParticleBuffer sBuffer = ( ISerialParticleBuffer )particles.Buffer;
			for ( int i = 0; i < count; ++i )
			{
				int particleIndex = sBuffer.AddParticle( );
				float* pos = ( float* )sBuffer.GetField( ParticleBase.Position, particleIndex );
				pos[ 0 ] = particles.Frame.Translation.X;
				pos[ 1 ] = particles.Frame.Translation.Y;
				pos[ 2 ] = particles.Frame.Translation.Z;

				float speed = m_MinSpeed + ( float )( m_Rnd.NextDouble( ) * ( m_MaxSpeed - m_MinSpeed ) );
				Vector3 vec = direction * speed;

				float* vel = ( float* )sBuffer.GetField( ParticleBase.Velocity, particleIndex );
				vel[ 0 ] = vec.X;
				vel[ 1 ] = vec.Y;
				vel[ 2 ] = vec.Z;
			}
		}

		#endregion

		#region IParticleSystemComponent Members

		/// <summary>
		/// Called when this component is attached to a particle system
		/// </summary>
		public void Attach( IParticleSystem particles )
		{
			particles.Buffer.AddField( ParticleBase.Velocity, ParticleFieldType.Float32, 3, 0.0f );
		}

		#endregion

		#region Private Members

		private static readonly Random m_Rnd = new Random( );
		private Vector3 m_Direction = Vector3.XAxis;
		private float m_Angle		= 5.0f;
		private float m_MinSpeed	= 0;
		private float m_MaxSpeed	= 1;

		#endregion

	}
}
