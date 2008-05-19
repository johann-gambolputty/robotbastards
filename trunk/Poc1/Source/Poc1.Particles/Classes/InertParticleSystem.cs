using System;
using System.Collections.Generic;
using Poc1.Particles.Interfaces;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Particles.Classes
{
	/// <summary>
	/// Simple implementation of the inert particle system interface <see cref="IInertParticleSystem"/>
	/// </summary>
	class InertParticleSystem : ParticleSystem
	{
		/// <summary>
		/// Renders this particle system
		/// </summary>
		public override void Render( IRenderContext context )
		{
			if ( EnableSpawning )
			{
				int count = m_Random.Next( MinimumSpawnRate, MaximumSpawnRate );
				SpawnParticles( Utils.Min( MaximumNumberOfParticles - m_Particles.Count, count ) );
			}
		}

		protected struct Particle
		{
			public Point3 Position
			{
				get { return new Point3( m_X, m_Y, m_Z ); }
				set
				{
					m_X = value.X;
					m_Y = value.Y;
					m_Z = value.Z;
				}
			}

			public ushort Age
			{
				get { return m_Age; }
				set { m_Age = value; }
			}

			public byte Size
			{
				get { return m_Size; }
				set { m_Size = value; }
			}

			private float	m_X;
			private float	m_Y;
			private float	m_Z;
			private ushort	m_Age;
			private byte	m_Size;
		}

		protected virtual void SpawnParticles( int count )
		{
			for ( int i = 0; i < count; ++i )
			{
				Particle p = new Particle( );

				m_Particles.Add( p );
			}
		}

		private readonly Random m_Random = new Random( 0 );
		private readonly List<Particle> m_Particles = new List<Particle>();
	}
}
