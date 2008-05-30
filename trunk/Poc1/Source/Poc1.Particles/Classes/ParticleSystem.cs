using System;
using System.Collections.Generic;
using Poc1.Particles.Interfaces;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Particles.Classes
{
	/// <summary>
	/// Handy abstract base class for particle systems
	/// </summary
	public class ParticleSystem : IParticleSystem, IParticleFactory
	{
		#region IParticleSystem Members

		/// <summary>
		/// Particle system centre
		/// </summary>
		public Matrix44 Frame
		{
			get { return m_Frame; }
		}

		/// <summary>
		/// Gets/sets the maximum number of particles. The default for this is 256
		/// </summary>
		public int MaximumNumberOfParticles
		{
			get { return m_Particles.MaximumSize; }
			set { m_Particles.MaximumSize = value; }
		}

		/// <summary>
		/// Enables/disables particle spawning.
		/// </summary>
		public bool EnableSpawning
		{
			get { return m_EnableSpawning; }
			set { m_EnableSpawning = value; }
		}

		/// <summary>
		/// Gets/sets the object used to create new particles, and destroy old particles
		/// </summary>
		public IParticleFactory ParticleFactory
		{
			get { return this; }
		}

		/// <summary>
		/// Gets/sets the object used to determine the spawn rate for particles
		/// </summary>
		public ISpawnRate SpawnRate
		{
			get { return m_SpawnRate; }
			set { m_SpawnRate = value; }
		}

		/// <summary>
		/// The object that sets up the initial state of newly spawned particles
		/// </summary>
		public IParticleSpawner Spawner
		{
			get { return m_Spawner; }
			set { m_Spawner = value; }
		}

		/// <summary>
		/// The object that updates particle properties.
		/// </summary>
		public IParticleUpdater Updater
		{
			get { return m_Updater; }
			set { m_Updater = value; }
		}

		/// <summary>
		/// Gets/sets the object that determines the lifespan of particles. 
		/// </summary>
		public IParticleKiller Killer
		{
			get { return m_Killer; }
			set { m_Killer = value; }
		}

		/// <summary>
		/// Gets/sets the object that renders the particle system
		/// </summary>
		public IParticleRenderer Renderer
		{
			get { return m_Renderer; }
			set { m_Renderer = value; }
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders this particle system
		/// </summary>
		public void Render( IRenderContext context )
		{
			Update( );
			if ( Renderer == null )
			{
				return;
			}
			Renderer.RenderParticles( context, this, m_Particles );
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Updates this particle system
		/// </summary>
		protected virtual void Update( )
		{
			if ( !EnableSpawning )
			{
				return;
			}
			if ( Spawner != null )
			{
				int count = SpawnRate == null ? 0 : SpawnRate.GetNumberOfParticlesToSpawn( );
				Spawner.SpawnParticles( ParticleFactory, count, m_Particles );
			}
			if ( Killer != null )
			{
				Killer.KillParticles( ParticleFactory, m_Particles );
			}
			if ( Updater != null )
			{
				Updater.Update( this, m_Particles );
			}

			//	Age all the particles
			foreach ( ParticleBase particle in m_Particles )
			{
				++particle.Age;
			}
		}

		#endregion

		#region Private Members

		#region ParticleSet class

		private class ParticleSet : IList<IParticle>
		{
			public int MaximumSize
			{
				get { return m_Particles.Length; }
				set
				{
					IParticle[] particles = new IParticle[ value ];
					int max = value < m_Particles.Length ? value : m_Particles.Length;
					for ( int i = 0; i < max; ++i )
					{
						if ( m_Particles[ i ] != null )
						{
							particles[ i ] = m_Particles[ i ];
						}
					}
					m_Particles = particles;
					if ( m_Count > value )
					{
						m_Count = value;
					}
				}
			}

			private int m_Count;
			private int m_AddPosition;
			private IParticle[] m_Particles = new IParticle[ 256 ];

			#region IList Members

			public void Clear( )
			{
				for ( int i = 0; i < m_Particles.Length; ++i )
				{
					m_Particles[ i ] = null;
				}
				m_Count = 0;
			}

			public bool IsReadOnly
			{
				get { return false; }
			}

			public void RemoveAt( int index )
			{
				m_Particles[ index ] = null;
			}


			#endregion

			#region ICollection Members

			public int Count
			{
				get { return m_Count; }
			}

			#endregion

			#region IEnumerable Members

			public System.Collections.IEnumerator GetEnumerator( )
			{
				return ( ( IList<IParticle> )this ).GetEnumerator( );
			}

			#endregion

			#region IList<IParticle> Members

			public int IndexOf( IParticle item )
			{
				return Array.IndexOf( m_Particles, item );
			}

			public void Insert( int index, IParticle item )
			{
				throw new NotSupportedException( );
			}

			IParticle IList<IParticle>.this[ int index ]
			{
				get { return m_Particles[ index ]; }
				set { m_Particles[ index ] = value; }
			}

			#endregion

			#region ICollection<IParticle> Members

			public void Add( IParticle item )
			{
				m_Particles[ m_AddPosition ] = item;
				m_AddPosition = ( m_AddPosition + 1 ) % m_Particles.Length;
				if ( m_Count < m_Particles.Length )
				{
					++m_Count;
				}
			}

			public bool Contains( IParticle item )
			{
				return Array.IndexOf( m_Particles, item ) != -1;
			}

			public void CopyTo( IParticle[] array, int arrayIndex )
			{
				throw new NotImplementedException( );
			}

			public bool Remove( IParticle item )
			{
				if ( item == null )
				{
					return false;
				}
				int index = IndexOf( item );
				if ( index == -1 )
				{
					return false;
				}
				m_Particles[ index ] = null;
				--m_Count;
				return true;
			}

			#endregion

			#region IEnumerable<IParticle> Members

			IEnumerator<IParticle> IEnumerable<IParticle>.GetEnumerator( )
			{
				int current = m_AddPosition == 0 ? m_Particles.Length - 1 : m_AddPosition - 1;
				int count = 0;
				for ( int i = 0; i < m_Particles.Length; ++i )
				{
					if ( count >= m_Count )
					{
						break;
					}
					if ( m_Particles[ current ] != null )
					{
						++count;
						yield return m_Particles[ current ];
					}
					current = current == 0 ? m_Particles.Length - 1 : current - 1;
				}
			}

			#endregion
		}

		#endregion

		private readonly ParticleSet	m_Particles		= new ParticleSet();
		private readonly Matrix44		m_Frame				= new Matrix44( );
		private bool					m_EnableSpawning	= true;
		private IParticleSpawner		m_Spawner			= ms_DefaultSpawner;
		private ISpawnRate				m_SpawnRate			= ms_DefaultSpawnRate;
		private IParticleKiller			m_Killer			= ms_DefaultKiller;
		private IParticleUpdater 		m_Updater			= ms_DefaultUpdater;
		private IParticleRenderer		m_Renderer;

		private readonly static IParticleSpawner	ms_DefaultSpawner;
		private readonly static IParticleUpdater	ms_DefaultUpdater;
		private readonly static IParticleKiller		ms_DefaultKiller;
		private readonly static ISpawnRate			ms_DefaultSpawnRate;

		static ParticleSystem( )
		{
			ms_DefaultSpawner	= null;
			ms_DefaultUpdater	= null;
			ms_DefaultKiller	= null;
			ms_DefaultSpawnRate = new RandomSpawnRate( 1 );
		}

		#endregion


		#region IParticleFactory Members

		/// <summary>
		/// Creates a new particle
		/// </summary>
		public IParticle CreateParticle( )
		{
			ParticleBase particle = new ParticleBase( );
			particle.Position = Frame.Translation;
			m_Particles.Add( particle );
			return particle;
		}

		/// <summary>
		/// Destroys an existing particle
		/// </summary>
		public void DestroyParticle( IParticle particle )
		{
			m_Particles.Remove( particle );
		}

		#endregion
	}
}
